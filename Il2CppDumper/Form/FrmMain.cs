using Il2CppDumper.Properties;
using Il2CppDumperGui;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

// https://github.com/AndnixSH/Il2CppDumper-GUI

namespace Il2CppDumper
{
    public partial class FrmMain : Form
    {
        public FrmMain(string[] args = null)
        {
            InitializeComponent();
            if (args != null)
            {
                RunDump(args);
            }
        }

        #region Variable

        public enum State
        {
            Idle,
            Running
        }

        private readonly string realPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
        private readonly string tempPath = Path.GetTempPath() + "\\";
        private static Config _config;
        private static string _soFile, _datFile, _outputDir;

        private string SoFile
        {
            get => _soFile;

            set
            {
                if (_soFile == value) return;
                _soFile = value;
                ChangeText(txtSoFile, value);
            }
        }

        private string DatFile
        {
            get => _datFile;

            set
            {
                if (_datFile == value) return;
                _datFile = value;
                ChangeText(txtDat, value);
            }
        }

        private string OutputDir
        {
            get => _outputDir;

            set
            {
                if (_outputDir == value) return;
                _outputDir = value;
                ChangeText(txtOutputDir, value);
            }
        }

        #endregion Variable

        #region Utility

        private static void ChangeText(TextBox textBox, string text)
        {
            try
            {
                textBox.Text = text;
            }
            catch
            {
                //
            }
        }

        #endregion Utility

        #region Load/Save

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Text += $" - {Assembly.GetExecutingAssembly().GetName().Version}";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            LoadLocation();
            cbArch.SelectedIndex = Settings.Default.Arch;
        }

        private void LoadLocation()
        {
            if (Settings.Default.Location == new Point(0, 0))
            {
                CenterToScreen();
            }
            else
            {
                Location = Settings.Default.Location;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Arch = cbArch.SelectedIndex;
            CloseLocation();
        }

        private void CloseLocation()
        {
            Settings.Default.Location = Location;
            Settings.Default.Save();
        }

        #endregion Load/Save

        #region Button EventArgs

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (openBin.ShowDialog() == DialogResult.OK)
            {
                SoFile = openBin.FileName;
                txtCode.Clear();
                txtMeta.Clear();

                if (Settings.Default.AutoSetDir)
                {
                    OutputDir = Path.GetDirectoryName(SoFile) + @"\dumped\";
                }
            }
        }

        private void btnDat_Click(object sender, EventArgs e)
        {
            if (openDat.ShowDialog() == DialogResult.OK)
            {
                DatFile = openDat.FileName;
                txtCode.Clear();
                txtMeta.Clear();
            }
        }

        private void btnDir_Click(object sender, EventArgs e)
        {
            if (openDir.ShowDialog() == DialogResult.OK)
            {
                OutputDir = openDir.SelectedPath + @"\";
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", OutputDir);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var frmSettings = new FrmSettings();
            {
                frmSettings.Location = new Point(Location.X + 150, Location.Y + 120);
                frmSettings.ShowDialog();
                frmSettings.Dispose();
            }
        }

        private async void btnDump_ClickAsync(object sender, EventArgs e)
        {
            rbLog.Clear();

            if (string.IsNullOrWhiteSpace(SoFile))
            {
                WriteOutput("Executable file is not selected", Color.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(DatFile))
            {
                WriteOutput("Metadata-global.dat file is not selected", Color.Red);
                return;
            }

            if (!Directory.Exists(OutputDir))
            {
                WriteOutput("Output directory does not exist", Color.Red);
                try
                {
                    Directory.CreateDirectory($"{OutputDir}");
                    WriteOutput($"Create directory at {OutputDir}", Color.LimeGreen);
                }
                catch
                {
                    WriteOutput("Can not create directory", Color.Red);
                    return;
                }
            }

            FormState(State.Running);

            await Task.Factory.StartNew(() => Dumper(SoFile, DatFile, OutputDir)).ConfigureAwait(false);

            FormState(State.Idle);
        }

        #endregion Button EventArgs

        #region Dump

        private void Dumper(string file, string metadataPath, string outputPath)
        {
            try
            {
                FileDir(outputPath);
                if (Init(file, metadataPath, out var metadata, out var il2Cpp))
                {
                    Dump(metadata, il2Cpp, outputPath);
                    CopyScripts();
                }
            }
            catch (Exception ex)
            {
                WriteOutput($"{ex.Message}", Color.Red);
            }
        }

        private void CopyScripts()
        {
            var guiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts\\");

            foreach (SettingsProperty currentProperty in Settings.Default.Properties)
            {
                if (currentProperty.PropertyType.Name == "Boolean")
                {
                    var fileName = currentProperty.Name + ".py";
                    var source = guiPath + fileName;
                    var dest = OutputDir + fileName;
                    if (Settings.Default[currentProperty.Name].ToString() == "True"
                        && File.Exists(source) && !File.Exists(dest))
                    {
                        WriteOutput(fileName + "does not exist", Color.Red);
                        try
                        {
                            File.Copy(source, dest);
                            WriteOutput($"Create {fileName} at {OutputDir}", Color.LimeGreen);
                        }
                        catch
                        {
                            WriteOutput($"Can not create {fileName}", Color.Red);
                            return;
                        }
                    }
                }
            }
        }

        private bool Init(string il2CppPath, string metadataPath, out Metadata metadata, out Il2Cpp il2Cpp)
        {
            WriteOutput("Read config...", Color.Black);
            if (File.Exists(realPath + "config.json"))
            {
                _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Application.StartupPath + Path.DirectorySeparatorChar + "config.json"));
            }
            else
            {
                _config = new Config();
                WriteOutput("config.json file does not exist. Using defaults", Color.Yellow);
            }

            WriteOutput("Initializing metadata...");
            var metadataBytes = File.ReadAllBytes(metadataPath);
            metadata = new Metadata(new MemoryStream(metadataBytes));
            WriteOutput($"Metadata Version: {metadata.Version}");

            WriteOutput("Initializing il2cpp file...");
            var il2CppBytes = File.ReadAllBytes(il2CppPath);
            var il2CppMagic = BitConverter.ToUInt32(il2CppBytes, 0);
            var il2CppMemory = new MemoryStream(il2CppBytes);
            switch (il2CppMagic)
            {
                default:
                    WriteOutput("ERROR: il2cpp file not supported.");
                    throw new NotSupportedException("ERROR: il2cpp file not supported.");
                case 0x6D736100:
                    var web = new WebAssembly(il2CppMemory);
                    il2Cpp = web.CreateMemory();
                    break;

                case 0x304F534E:
                    var nso = new NSO(il2CppMemory);
                    il2Cpp = nso.UnCompress();
                    break;

                case 0x905A4D: //PE
                    il2Cpp = new PE(il2CppMemory);
                    break;

                case 0x464c457f: //ELF
                    if (il2CppBytes[4] == 2) //ELF64
                    {
                        il2Cpp = new Elf64(il2CppMemory);
                    }
                    else
                    {
                        il2Cpp = new Elf(il2CppMemory);
                    }
                    break;

                case 0xCAFEBABE: //FAT Mach-O
                case 0xBEBAFECA:
                    var machofat = new MachoFat(new MemoryStream(il2CppBytes));
                    var fatMagic = "";
                    for (var i = 0; i < machofat.fats.Length; i++)
                    {
                        var fat = machofat.fats[i];
                        fatMagic += fat.magic == 0xFEEDFACF ? $"{i + 1}.64bit\n" : $"{i + 1}.32bit\n";
                    }
                    var key = "";
                    if (InputBox.Show(fatMagic, "Select Platform:", ref key) != DialogResult.OK)
                    {
                        il2Cpp = null;
                        return false;
                    }
                    var index = int.Parse(key) - 1;
                    var magic = machofat.fats[index % 2].magic;
                    il2CppBytes = machofat.GetMacho(index % 2);
                    il2CppMemory = new MemoryStream(il2CppBytes);
                    if (magic == 0xFEEDFACF)
                        goto case 0xFEEDFACF;
                    else
                        goto case 0xFEEDFACE;
                case 0xFEEDFACF: // 64bit Mach-O
                    il2Cpp = new Macho64(il2CppMemory);
                    break;

                case 0xFEEDFACE: // 32bit Mach-O
                    il2Cpp = new Macho(il2CppMemory);
                    break;
            }
            var version = _config.ForceIl2CppVersion ? _config.ForceVersion : metadata.Version;
            il2Cpp.SetProperties(version, metadata.metadataUsagesCount);
            WriteOutput($"Il2Cpp Version: {il2Cpp.Version}");
            if (_config.ForceDump || il2Cpp.CheckDump())
            {
                if (il2Cpp is ElfBase elf)
                {
                    var value = "";
                    if (InputBox.Show("Input il2cpp dump address or input 0 to force continue:", "Detected this may be a dump file", ref value) != DialogResult.OK) return false;
                    var dumpAddr = Convert.ToUInt64(value, 16);
                    if (dumpAddr != 0)
                    {
                        WriteOutput($"il2cpp dump address: {dumpAddr}");
                        il2Cpp.ImageBase = dumpAddr;
                        il2Cpp.IsDumped = true;
                        if (!_config.NoRedirectedPointer)
                        {
                            elf.Reload();
                        }
                    }
                }
                else
                {
                    il2Cpp.IsDumped = true;
                }
            }

            WriteOutput("Searching...");
            try
            {
                var flag = il2Cpp.PlusSearch(metadata.methodDefs.Count(x => x.methodIndex >= 0), metadata.typeDefs.Length, metadata.imageDefs.Length);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !flag && il2Cpp is PE)
                {
                    WriteOutput("Use custom PE loader");
                    il2Cpp = PELoader.Load(il2CppPath);
                    il2Cpp.SetProperties(version, metadata.metadataUsagesCount);
                    flag = il2Cpp.PlusSearch(metadata.methodDefs.Count(x => x.methodIndex >= 0), metadata.typeDefs.Length, metadata.imageDefs.Length);
                }
                if (!flag)
                {
                    flag = il2Cpp.Search();
                }
                if (!flag)
                {
                    flag = il2Cpp.SymbolSearch();
                }
                if (!flag)
                {
                    WriteOutput("ERROR: Can't use auto mode to process file, try manual mode.");
                    WriteOutput("Input CodeRegistration: ");

                    var codeValue = "";
                    if (InputBox.Show(@"Input CodeRegistration: ", "", ref codeValue) != DialogResult.OK) return false;
                    var codeRegistration = Convert.ToUInt64(codeValue, 16);
                    WriteOutput($"CodeRegistration: {codeValue}");

                    var metadataValue = "";
                    if (InputBox.Show("Input MetadataRegistration: ", "", ref metadataValue) != DialogResult.OK) return false;
                    var metadataRegistration = Convert.ToUInt64(metadataValue, 16);
                    WriteOutput($"MetadataRegistration: {metadataValue}");

                    il2Cpp.Init(codeRegistration, metadataRegistration);
                }
                if (il2Cpp.Version >= 27 && il2Cpp.IsDumped)
                {
                    var typeDef = metadata.typeDefs[0];
                    var il2CppType = il2Cpp.types[typeDef.byvalTypeIndex];
                    metadata.ImageBase = il2CppType.data.typeHandle - metadata.header.typeDefinitionsOffset;
                }
            }
            catch (Exception e)
            {
                WriteOutput(e.Message);
                WriteOutput("ERROR: An error occurred while processing.");
                return false;
            }
            return true;
        }

        private void Dump(Metadata metadata, Il2Cpp il2Cpp, string outputDir)
        {
            WriteOutput("Dumping...");
            var executor = new Il2CppExecutor(metadata, il2Cpp);
            var decompiler = new Il2CppDecompiler(executor);
            decompiler.Decompile(_config, outputDir);
            WriteOutput("Done!");
            if (_config.GenerateStruct)
            {
                WriteOutput("Generate struct...");
                var scriptGenerator = new StructGenerator(executor);
                scriptGenerator.WriteScript(outputDir);
                WriteOutput("Done!");
            }
            if (_config.GenerateDummyDll)
            {
                WriteOutput("Generate dummy dll...");
                DummyAssemblyExporter.Export(executor, outputDir, _config.DummyDllAddToken);
                WriteOutput("Done!");
                Directory.SetCurrentDirectory(realPath); //Fix read-only directory permission
            }
        }

        #endregion Dump

        #region Drag/Drop

        private async void RunDump(IReadOnlyList<string> files)
        {
            try
            {
                FormState(State.Running);

                if (files.Count > 1)
                {
                    DeleteFile(tempPath + "global-metadata.dat");
                    DeleteFile(tempPath + "libil2cpp.so");
                }
                var outputPath = Path.GetDirectoryName(files[0]) + "\\" + Path.GetFileNameWithoutExtension(files[0]) + "_dumped\\";
                if (Settings.Default.AutoSetDir)
                {
                    OutputDir = outputPath;
                }
                else
                {
                    outputPath = OutputDir + Path.GetFileNameWithoutExtension(files[0]) + "_dumped\\";
                }

                foreach (var file in files)
                {
                    switch (Path.GetExtension(file))
                    {
                        case ".so":
                            SoFile = file;
                            break;

                        case ".dat":
                            DatFile = file;
                            break;

                        case ".apk":
                            {
                                rbLog.Text = "";
                                if (files.Count > 1)
                                {
                                    WriteOutput("Dumping Il2Cpp from splitted APKs...", Color.Cyan);
                                    await APKSplitDump(file, outputPath).ConfigureAwait(false);
                                }
                                else
                                {
                                    await APKDump(file, outputPath).ConfigureAwait(false);
                                }

                                break;
                            }
                        case ".apks":
                        case ".xapk":
                            rbLog.Text = "";
                            await APKsDump(file, outputPath).ConfigureAwait(false);
                            break;

                        case ".ipa":
                            rbLog.Text = "";
                            await iOSDump(file, outputPath).ConfigureAwait(false);
                            break;

                        default:
                            SoFile = file;
                            break;
                    }

                    if (Settings.Default.AutoSetDir)
                    {
                        OutputDir = Path.GetDirectoryName(file) + @"\dumped\";
                    }
                }
            }
            catch (Exception ex)
            {
                WriteOutput($"{ex.Message}", Color.Red);
            }
            FormState(State.Idle);
        }

        private async void FrmMain_DragDropAsync(object sender, DragEventArgs e)
        {
            try
            {
                FormState(State.Running);

                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 1)
                {
                    DeleteFile(tempPath + "global-metadata.dat");
                    DeleteFile(tempPath + "libil2cpp.so");
                }
                var outputPath = Path.GetDirectoryName(files[0]) + "\\" + Path.GetFileNameWithoutExtension(files[0]) + "_dumped\\";
                if (Settings.Default.AutoSetDir)
                {
                    OutputDir = outputPath;
                }
                else
                {
                    outputPath = OutputDir + Path.GetFileNameWithoutExtension(files[0]) + "_dumped\\";
                }

                foreach (var file in files)
                {
                    switch (Path.GetExtension(file))
                    {
                        case ".so":
                            SoFile = file;
                            break;

                        case ".dat":
                            DatFile = file;
                            break;

                        case ".apk":
                            {
                                rbLog.Text = "";
                                if (files.Length > 1)
                                {
                                    WriteOutput("Dumping Il2Cpp from splitted APKs...", Color.Cyan);
                                    await APKSplitDump(file, outputPath).ConfigureAwait(false);
                                }
                                else
                                {
                                    await APKDump(file, outputPath).ConfigureAwait(false);
                                }

                                break;
                            }
                        case ".apks":
                        case ".xapk":
                            rbLog.Text = "";
                            await APKsDump(file, outputPath).ConfigureAwait(false);
                            break;

                        case ".ipa":
                            rbLog.Text = "";
                            await iOSDump(file, outputPath).ConfigureAwait(false);
                            break;

                        default:
                            SoFile = file;
                            break;
                    }

                    if (Settings.Default.AutoSetDir)
                    {
                        OutputDir = Path.GetDirectoryName(file) + @"\dumped\";
                    }
                }
            }
            catch (Exception ex)
            {
                WriteOutput($"{ex.Message}", Color.Red);
            }
            FormState(State.Idle);
        }

        private void FrmMain_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            e.Effect = DragDropEffects.Copy;
        }

        private static string FileDir(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            }
            return path;
        }

        private static void DeleteFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        #endregion Drag/Drop

        #region Copy to clipboard

        private void menuCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rbLog.SelectedText);
        }

        private void rbLog_TextChanged(object sender, EventArgs e)
        {
            rbLog.SelectionStart = rbLog.Text.Length;
            rbLog.ScrollToCaret();
        }

        #endregion Copy to clipboard

        #region Logging

        public static void AppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
            box.ScrollToCaret();
        }

        private void TextToLogs(string str, Color color)
        {
            Invoke(new MethodInvoker(delegate
            {
                AppendText(rbLog, str, color);
            }));
        }

        public void WriteOutput(string str, Color color)
        {
            Invoke(new MethodInvoker(delegate
            {
                TextToLogs(str + Environment.NewLine, color);
            }));
        }

        public void WriteOutput(string str)
        {
            Invoke(new MethodInvoker(delegate
            {
                TextToLogs(str + Environment.NewLine, Color.Black);
            }));
        }

        #endregion Logging

        #region Form Controller

        private void FormState(State state)
        {
            try
            {
                if (state == State.Running)
                {
                    btnDump.Text = "Dumping...";
                    EnableController(this, false);
                }
                else
                {
                    btnDump.Text = "Dump";
                    EnableController(this, true);
                }
            }
            catch (Exception e)
            {
                WriteOutput("Error: " + e.Message, Color.Red);
            }
        }

        private void EnableController(Form form, bool value)
        {
            foreach (var obj in form.Controls)
            {
                var control = (Control)obj;
                switch (control.GetType().Name)
                {
                    case "Button":
                    case "TextBox":
                    case "RadioButton":
                    case "RichTextBox":
                        control.Enabled = value;
                        break;

                    case "GroupBox":
                    case "Panel":
                    case "TableLayoutPanel":
                    case "TabPage":
                        EnableController(control, value);
                        break;
                }
            }
        }

        private void EnableController(Control control, bool value)
        {
            foreach (var obj in control.Controls)
            {
                var control2 = (Control)obj;
                switch (control2.GetType().Name)
                {
                    case "Button":
                    case "TextBox":
                    case "RadioButton":
                    case "RichTextBox":
                        control2.Enabled = value;
                        break;

                    case "GroupBox":
                    case "Panel":
                    case "TableLayoutPanel":
                    case "TabPage":
                        EnableController(control, value);
                        break;
                }
            }
        }

        #endregion Form Controller

        #region Auto Dump

        private Task iOSDump(string file, string outputPath)
        {
            return Task.Factory.StartNew(() =>
            {
                using var archive = ZipFile.OpenRead(file);
                var ipaBinaryFolder = archive.Entries.FirstOrDefault(f => f.FullName.StartsWith("Payload/") && f.FullName.Contains(".app/") && f.FullName.Count(x => x == '/') == 2);

                if (ipaBinaryFolder != null)
                {
                    var myRegex3 = new Regex(@"(?<=Payload\/)(.*?)(?=.app\/)", RegexOptions.None);
                    var match = myRegex3.Match(ipaBinaryFolder.FullName);

                    var ipaBinaryName = match.ToString();
                    var metadataFile = archive.Entries.FirstOrDefault(f => f.FullName == $"Payload/{ipaBinaryName}.app/Data/Managed/Metadata/global-metadata.dat");
                    var binaryFile = archive.Entries.FirstOrDefault(f => f.FullName == $"Payload/{ipaBinaryName}.app/Frameworks/UnityFramework.framework/UnityFramework");
                    if (binaryFile == null)
                    {
                        binaryFile = archive.Entries.FirstOrDefault(f => f.FullName == $"Payload/{ipaBinaryName}.app/{ipaBinaryName}");
                    }
                    if (metadataFile != null && binaryFile != null)
                    {
                        if (Settings.Default.extDat)
                            metadataFile.ExtractToFile(FileDir(outputPath + "\\global-metadata.dat"), true);
                        metadataFile.ExtractToFile(tempPath + "global-metadata.dat", true);
                        if (Settings.Default.machO64)
                        {
                            WriteOutput("Dumping ARM64...", Color.Chartreuse);

                            if (Settings.Default.extBin)
                                binaryFile.ExtractToFile(FileDir(outputPath + $"/{ipaBinaryName}"), true);
                            binaryFile.ExtractToFile(tempPath + "arm64", true);
                            Dumper(tempPath + "arm64", tempPath + "global-metadata.dat", FileDir(outputPath + "\\"));
                        }
                        else
                        {
                            WriteOutput("Dumping ARMv7...", Color.Chartreuse);

                            if (Settings.Default.extBin)
                                binaryFile.ExtractToFile(FileDir(outputPath + $"/{ipaBinaryName}"), true);
                            binaryFile.ExtractToFile(tempPath + "armv7", true);
                            Dumper(tempPath + "armv7", tempPath + "global-metadata.dat", FileDir(outputPath + "\\"));
                        }
                    }
                    else
                    {
                        WriteOutput("This IPA does not contain an IL2CPP application", Color.Yellow);
                    }
                }
                else
                {
                    WriteOutput("Failed to extract required file. Please extract the files manually", Color.Yellow);
                }
                archive.Dispose();
            });
        }

        private Task APKDump(string file, string outputPath)
        {
            return Task.Factory.StartNew(() =>
            {
                using var archive = ZipFile.OpenRead(file);
                var binaryFile = archive.Entries.FirstOrDefault(f => f.Name.Contains("libil2cpp.so"));
                var metadataPath = archive.Entries.FirstOrDefault(f => f.FullName.Contains("assets/bin/Data/Managed/etc/"));
                var metadataFile = archive.Entries.FirstOrDefault(f => f.FullName == "assets/bin/Data/Managed/Metadata/global-metadata.dat");

                if (binaryFile == null && metadataPath != null)
                {
                    WriteOutput("This APK does not contain lib folder. APK has been splitted", Color.Yellow);
                    return;
                }
                if (binaryFile != null && metadataPath == null)
                {
                    WriteOutput("This APK contains il2cpp but does not contain global-metadata.dat. It may be protected or APK has been splitted", Color.Yellow);
                    return;
                }

                if (metadataFile != null)
                {
                    if (Settings.Default.extDat)
                        metadataFile.ExtractToFile(FileDir(outputPath + "\\global-metadata.dat"), true);
                    metadataFile.ExtractToFile(tempPath + "global-metadata.dat", true);

                    foreach (var entry in archive.Entries)
                    {
                        if (entry.FullName.Equals("lib/armeabi-v7a/libil2cpp.so") && cbArch.SelectedIndex is 0 or 1)
                        {
                            WriteOutput("Dumping ARMv7...", Color.Chartreuse);

                            if (Settings.Default.extBin)
                                entry.ExtractToFile(FileDir(outputPath + "\\ARMv7\\libil2cpp.so"), true);
                            entry.ExtractToFile(tempPath + "libil2cpparmv7", true);
                            Dumper(tempPath + "libil2cpparmv7", tempPath + "global-metadata.dat", FileDir(outputPath + "\\ARMv7\\"));
                        }

                        if (entry.FullName.Equals("lib/arm64-v8a/libil2cpp.so") && cbArch.SelectedIndex is 0 or 2)
                        {
                            WriteOutput("Dumping ARM64...", Color.Chartreuse);

                            if (Settings.Default.extBin)
                                entry.ExtractToFile(FileDir(outputPath + "\\ARM64\\libil2cpp.so"), true);
                            entry.ExtractToFile(tempPath + "libil2cpparm64", true);
                            Dumper(tempPath + "libil2cpparm64", tempPath + "global-metadata.dat", FileDir(outputPath + "\\ARM64\\"));
                        }

                        if (entry.FullName.Equals("lib/x86/libil2cpp.so") && cbArch.SelectedIndex is 0)
                        {
                            WriteOutput("Dumping x86...", Color.Chartreuse);

                            if (Settings.Default.extBin)
                                entry.ExtractToFile(FileDir(outputPath + "\\x86\\libil2cpp.so"), true);
                            entry.ExtractToFile(tempPath + "libil2cppx86", true);
                            Dumper(tempPath + "libil2cppx86", tempPath + "global-metadata.dat", FileDir(outputPath + "\\x86\\"));
                        }
                    }
                }
                else
                {
                    WriteOutput("This APK does not contain an IL2CPP application", Color.Yellow);
                }
                archive.Dispose();
            });
        }

        private Task APKSplitDump(string file, string outputPath)
        {
            return Task.Factory.StartNew(() =>
            {
                using var archive = ZipFile.OpenRead(file);
                var binaryFile = archive.Entries.FirstOrDefault(f => f.Name.Contains("libil2cpp.so"));
                var metadataFile = archive.Entries.FirstOrDefault(f => f.FullName == "assets/bin/Data/Managed/Metadata/global-metadata.dat");

                metadataFile?.ExtractToFile(tempPath + "global-metadata.dat", true);
                if (Settings.Default.extDat)
                    metadataFile?.ExtractToFile(FileDir(outputPath + "\\global-metadata.dat"), true);
                binaryFile?.ExtractToFile(tempPath + "libil2cpp.so", true);
                if (Settings.Default.extBin)
                    binaryFile?.ExtractToFile(FileDir(outputPath + "\\libil2cpp.so"), true);
                if (File.Exists(tempPath + "libil2cpp.so") && File.Exists(tempPath + "global-metadata.dat"))
                {
                    Dumper(tempPath + "libil2cpp.so", tempPath + "global-metadata.dat", FileDir(outputPath + "\\"));
                }
                archive.Dispose();
            });
        }

        private Task APKsDump(string file, string outputPath)
        {
            return Task.Factory.StartNew(() =>
            {
                using var archive = ZipFile.OpenRead(file);
                foreach (var entryApks in archive.Entries)
                {
                    if (entryApks.FullName.EndsWith(".apk", StringComparison.OrdinalIgnoreCase))
                    {
                        var apkFile = Path.Combine(tempPath, entryApks.FullName);
                        entryApks.ExtractToFile(apkFile, true);
                        using var entryBase = ZipFile.OpenRead(apkFile);
                        var binaryFile = entryBase.Entries.FirstOrDefault(f => f.Name.Contains("libil2cpp.so"));
                        var metadataFile = entryBase.Entries.FirstOrDefault(f => f.FullName == "assets/bin/Data/Managed/Metadata/global-metadata.dat");

                        metadataFile?.ExtractToFile(tempPath + "global-metadata.dat", true);
                        if (Settings.Default.extDat)
                            metadataFile?.ExtractToFile(FileDir(outputPath + "\\global-metadata.dat"), true);

                        if (binaryFile != null)
                        {
                            binaryFile.ExtractToFile(tempPath + "libil2cpp.so", true);

                            foreach (var entry in entryBase.Entries)
                            {
                                if (entry.FullName.Equals("lib/armeabi-v7a/libil2cpp.so") && cbArch.SelectedIndex is 0 or 1)
                                {
                                    WriteOutput("Dumping ARMv7...", Color.Chartreuse);

                                    if (Settings.Default.extBin)
                                        entry.ExtractToFile(FileDir(outputPath + "\\ARMv7\\libil2cpp.so"), true);
                                    entry.ExtractToFile(tempPath + "libil2cpparmv7", true);
                                    Dumper(tempPath + "libil2cpparmv7", tempPath + "global-metadata.dat", FileDir(outputPath + "\\ARMv7\\"));
                                }

                                if (entry.FullName.Equals(@"lib/arm64-v8a/libil2cpp.so") && cbArch.SelectedIndex is 0 or 2)
                                {
                                    WriteOutput("Dumping ARM64...", Color.Chartreuse);

                                    if (Settings.Default.extBin)
                                        entry.ExtractToFile(FileDir(outputPath + "\\ARM64\\libil2cpp.so"), true);
                                    entry.ExtractToFile(tempPath + "libil2cpparm64", true);
                                    Dumper(tempPath + "libil2cpparm64", tempPath + "global-metadata.dat", FileDir(outputPath + "\\ARM64\\"));
                                }

                                if (entry.FullName.Equals(@"lib/x86/libil2cpp.so") && cbArch.SelectedIndex is 0)
                                {
                                    WriteOutput("Dumping x86...", Color.Chartreuse);

                                    if (Settings.Default.extBin)
                                        entry.ExtractToFile(FileDir(outputPath + "\\x86\\libil2cpp.so"), true);
                                    entry.ExtractToFile(tempPath + "libil2cppx86", true);
                                    Dumper(tempPath + "libil2cppx86", tempPath + "global-metadata.dat", FileDir(outputPath + "\\x86\\"));
                                }
                            }
                        }
                        entryBase.Dispose();
                        File.Delete(apkFile);
                    }
                }
                archive.Dispose();
            });
        }

        #endregion Auto Dump
    }
}