using Il2CppDumper;
using Newtonsoft.Json;
using System;
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

namespace Il2CppDumperGui
{
    public partial class FrmMain : Form
    {

        public FrmMain()
        {
            InitializeComponent();
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
        #endregion

        #region Load/Save
        private void FrmMain_Load(object sender, EventArgs e)
        {
            Text += $@" - {Assembly.GetExecutingAssembly().GetName().Version}";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        #endregion

        #region Button EventArgs

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (openBin.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = openBin.FileName;
                txtCode.Clear();
                txtMeta.Clear();

                if (Properties.Settings.Default.AutoSetDir)
                {
                    txtDir.Text = Path.GetDirectoryName(txtFile.Text) + @"\dumped\";
                }
            }
        }

        private void btnDat_Click(object sender, EventArgs e)
        {
            if (openDat.ShowDialog() == DialogResult.OK)
            {
                txtDat.Text = openDat.FileName;
                txtCode.Clear();
                txtMeta.Clear();
            }
        }

        private void btnDir_Click(object sender, EventArgs e)
        {
            if (openDir.ShowDialog() == DialogResult.OK)
            {
                txtDir.Text = openDir.SelectedPath + @"\";
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", txtDir.Text);
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

            if (string.IsNullOrWhiteSpace(txtFile.Text))
            {
                WriteOutput("Executable file is not selected", Color.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDat.Text))
            {
                WriteOutput("Metadata-global.dat file is not selected", Color.Red);
                return;
            }

            if (!Directory.Exists(txtDir.Text))
            {
                WriteOutput("Output directory does not exist", Color.Red);
                try
                {
                    Directory.CreateDirectory($"{txtDir.Text}");
                    WriteOutput($"Create directory at {txtDir.Text}", Color.LimeGreen);
                }
                catch
                {
                    WriteOutput("Can not create directory", Color.Red);
                    return;
                }
            }

            FormState(State.Running);

            await Task.Factory.StartNew(() =>
            {
                Dumper(txtFile.Text, txtDat.Text, txtDir.Text);
            });


            FormState(State.Idle);
        }

        #endregion

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
            var guiPath = AppDomain.CurrentDomain.BaseDirectory;

            if (Properties.Settings.Default.ghidra)
            {
                if (File.Exists(guiPath + "ghidra.py"))
                {
                    if (!File.Exists(txtDir.Text + "ghidra.py"))
                    {
                        WriteOutput("ghidra.py does not exist", Color.Red);
                        try
                        {
                            File.Copy(guiPath + "ghidra.py", txtDir.Text + "ghidra.py");
                            WriteOutput($"Create ghidra.py at {txtDir.Text}", Color.LimeGreen);
                        }
                        catch
                        {
                            WriteOutput("Can not create ghidra.py", Color.Red);
                            return;
                        }
                    }
                }
            }
            if (Properties.Settings.Default.ghidra_with_struct)
            {
                if (File.Exists(guiPath + "ghidra_with_struct.py"))
                {
                    if (!File.Exists(txtDir.Text + "ghidra_with_struct.py"))
                    {
                        WriteOutput("ghidra_with_struct.py does not exist", Color.Red);
                        try
                        {
                            File.Copy(guiPath + "ghidra_with_struct.py", txtDir.Text + "ghidra_with_struct.py");
                            WriteOutput($"Create ghidra_with_struct.py at {txtDir.Text}", Color.LimeGreen);
                        }
                        catch
                        {
                            WriteOutput("Can not create ghidra_with_struct.py", Color.Red);
                            return;
                        }
                    }
                }
            }
            if (Properties.Settings.Default.ida)
            {
                if (File.Exists(guiPath + "ida.py"))
                {
                    if (!File.Exists(txtDir.Text + "ida.py"))
                    {
                        WriteOutput("ida.py does not exist", Color.Red);
                        try
                        {
                            File.Copy(guiPath + "ida.py", txtDir.Text + "ida.py");
                            WriteOutput($"Create ida.py at {txtDir.Text}", Color.LimeGreen);
                        }
                        catch
                        {
                            WriteOutput("Can not create ida.py", Color.Red);
                            return;
                        }
                    }
                }
            }
            if (Properties.Settings.Default.ida_py3)
            {
                if (File.Exists(guiPath + "ida_py3.py"))
                {
                    if (!File.Exists(txtDir.Text + "ida_py3.py"))
                    {
                        WriteOutput("ida_py3.py does not exist", Color.Red);
                        try
                        {
                            File.Copy(guiPath + "ida_py3.py", txtDir.Text + "ida_py3.py");
                            WriteOutput($"Create ida_py3.py at {txtDir.Text}", Color.LimeGreen);
                        }
                        catch
                        {
                            WriteOutput("Can not create ida_py3.py", Color.Red);
                            return;
                        }
                    }
                }
            }
            if (Properties.Settings.Default.ida_with_struct)
            {
                if (File.Exists(guiPath + "ida_with_struct.py"))
                {
                    if (!File.Exists(txtDir.Text + "ida_with_struct.py"))
                    {
                        WriteOutput("ida_with_struct.py does not exist", Color.Red);
                        try
                        {
                            File.Copy(guiPath + "ida_with_struct.py", txtDir.Text + "ida_with_struct.py");
                            WriteOutput($"Create ida_with_struct.py at {txtDir.Text}", Color.LimeGreen);
                        }
                        catch
                        {
                            WriteOutput("Can not create ida_with_struct.py", Color.Red);
                            return;
                        }
                    }
                }
            }
            if (Properties.Settings.Default.ida_with_struct_py3)
            {
                if (File.Exists(guiPath + "ida_with_struct_py3.py"))
                {
                    if (!File.Exists(txtDir.Text + "ida_with_struct_py3.py"))
                    {
                        WriteOutput("ida_with_struct_py3.py does not exist", Color.Red);
                        try
                        {
                            File.Copy(guiPath + "ida_with_struct_py3.py", txtDir.Text + "ida_with_struct_py3.py");
                            WriteOutput($"Create ida_with_struct_py3.py at {txtDir.Text}", Color.LimeGreen);
                        }
                        catch
                        {
                            WriteOutput("Can not create ida_with_struct_py3.py", Color.Red);
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
                _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Application.StartupPath + Path.DirectorySeparatorChar + @"config.json"));
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
                    WriteOutput("Select Platform: ");
                    for (var i = 0; i < machofat.fats.Length; i++)
                    {
                        var fat = machofat.fats[i];
                        WriteOutput(fat.magic == 0xFEEDFACF ? $"{i + 1}.64bit " : $"{i + 1}.32bit ");
                    }
                    WriteOutput("");
                    var key = Console.ReadKey(true);
                    var index = int.Parse(key.KeyChar.ToString()) - 1;
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
            il2Cpp.SetProperties(version, metadata.maxMetadataUsages);
            WriteOutput($"Il2Cpp Version: {il2Cpp.Version}");
            if (il2Cpp.Version >= 27 && il2Cpp is ElfBase elf && elf.IsDumped)
            {
                WriteOutput("Input global-metadata.dat dump address:");
                metadata.Address = Convert.ToUInt64(Console.ReadLine(), 16);
            }


            WriteOutput("Searching...");
            try
            {
                var flag = il2Cpp.PlusSearch(metadata.methodDefs.Count(x => x.methodIndex >= 0), metadata.typeDefs.Length);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (!flag && il2Cpp is PE)
                    {
                        WriteOutput("Use custom PE loader");
                        il2Cpp = PELoader.Load(il2CppPath);
                        il2Cpp.SetProperties(version, metadata.maxMetadataUsages);
                        flag = il2Cpp.PlusSearch(metadata.methodDefs.Count(x => x.methodIndex >= 0), metadata.typeDefs.Length);
                    }
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
                    var codeRegistration = Convert.ToUInt64(Console.ReadLine(), 16);
                    WriteOutput("Input MetadataRegistration: ");
                    var metadataRegistration = Convert.ToUInt64(Console.ReadLine(), 16);
                    il2Cpp.Init(codeRegistration, metadataRegistration);
                    return true;
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
            decompiler.Decompile(_config, outputDir, 1);
            WriteOutput("Done!");
            if (_config.GenerateStruct)
            {
                WriteOutput("Generate struct...");
                var scriptGenerator = new StructGenerator(executor);
                scriptGenerator.WriteScript(outputDir, 1);
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

        #endregion

        #region Drag/Drop
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
                if (Properties.Settings.Default.AutoSetDir)
                {
                    txtDir.Text = outputPath;
                }
                else
                {
                    outputPath = txtDir.Text + Path.GetFileNameWithoutExtension(files[0]) + "_dumped\\";
                }

                

                foreach (var file in files)
                {
                    var ext = Path.GetExtension(file);
                    if(ext.Equals(".so"))
                    {
                        txtFile.Text = file;
                    }
                    if (ext.Equals(".dat"))
                    {
                        txtDat.Text = file;
                    }
                    else if (ext.Equals(".apk"))
                    {
                        rbLog.Text = "";
                        if (files.Length > 1)
                        {
                            WriteOutput("Dumping Il2Cpp from splitted APKs...", Color.Cyan);
                            await APKSplitDump(file, outputPath);
                        }
                        else
                            await APKDump(file, outputPath);
                    }
                    else if (ext.Equals(".apks") || ext.Equals(".xapk"))
                    {
                        rbLog.Text = "";
                        await APKsDump(file, outputPath);
                    }
                    else if (ext.Equals(".ipa"))
                    {
                        rbLog.Text = "";
                        await iOSDump(file, outputPath);
                    }
                    else
                    {
                        txtFile.Text = file;
                    }

                    if (Properties.Settings.Default.AutoSetDir)
                    {
                        txtDir.Text = Path.GetDirectoryName(file) + @"\dumped\";
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
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                Path.GetExtension(file);
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void FrmMain_DragOver(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                Path.GetExtension(file);
            }
        }

        private string FileDir(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            return path;
        }

        private void DeleteFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        #endregion

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

        #endregion

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
        #endregion

        #region Form Controller
        private void FormState(State state)
        {
            if (state == State.Running)
            {
                btnDump.Text = @"Dumping...";
                EnableController(this, false);
            }
            else
            {
                btnDump.Text = @"Dump";
                EnableController(this, true);
            }
        }
        private void EnableController(Form form, bool value)
        {
            foreach (var obj in form.Controls)
            {
                var control = (Control)obj;
                if (control.GetType().Name == "Button" ||
                    control.GetType().Name == "TextBox" ||
                    control.GetType().Name == "RadioButton" ||
                    control.GetType().Name == "RichTextBox")
                {
                    control.Enabled = value;
                }
                if (control.GetType().Name == "GroupBox" ||
                    control.GetType().Name == "Panel" ||
                    control.GetType().Name == "TableLayoutPanel" ||
                    control.GetType().Name == "TabPage")
                {
                    EnableController(control, value);
                }
            }
        }
        private void EnableController(Control control, bool value)
        {
            foreach (var obj in control.Controls)
            {
                var control2 = (Control)obj;
                if (control2.GetType().Name == "Button" ||
                    control2.GetType().Name == "TextBox" ||
                    control2.GetType().Name == "RadioButton" ||
                    control2.GetType().Name == "RichTextBox")
                {
                    control2.Enabled = value;
                }
                if (control2.GetType().Name == "GroupBox" ||
                    control2.GetType().Name == "Panel" ||
                    control2.GetType().Name == "TableLayoutPanel" ||
                    control2.GetType().Name == "TabPage")
                {
                    EnableController(control, value);
                }
            }
        }

        #endregion

        #region Auto Dump
        async Task iOSDump(string file, string outputPath)
        {
            await Task.Factory.StartNew(() =>
            {
                using var archive = ZipFile.OpenRead(file);
                var ipaBinaryFolder = archive.Entries.FirstOrDefault(f => f.FullName.StartsWith("Payload/") && f.FullName.EndsWith(".app/") && f.FullName.Count(x => x == '/') == 2);

                if (ipaBinaryFolder != null)
                {
                    var myRegex3 = new Regex(@"(?<=Payload\/)(.*?)(?=.app\/)", RegexOptions.None);
                    var match = myRegex3.Match(ipaBinaryFolder.FullName);

                    var ipaBinaryName = match.ToString();
                    var metadataFile = archive.Entries.FirstOrDefault(f => f.FullName == $"Payload/{ipaBinaryName}.app/Data/Managed/Metadata/global-metadata.dat");
                    var binaryFile = archive.Entries.FirstOrDefault(f => f.FullName == $"Payload/{ipaBinaryName}.app/{ipaBinaryName}");
                    if (metadataFile != null)
                    {
                        metadataFile.ExtractToFile(tempPath + "global-metadata.dat", true);
                        if (rad64.Checked)
                        {
                            WriteOutput("Dumping ARM64...", Color.Chartreuse);

                            if (Properties.Settings.Default.extBin)
                                binaryFile.ExtractToFile(FileDir(outputPath + $"/{ipaBinaryName}"), true);
                            binaryFile.ExtractToFile(tempPath + "arm64", true);
                            Dumper(tempPath + "arm64", tempPath + "global-metadata.dat", FileDir(outputPath + "\\"));
                        }
                        else
                        {
                            WriteOutput("Dumping ARMv7...", Color.Chartreuse);

                            if (Properties.Settings.Default.extBin)
                                binaryFile.ExtractToFile(FileDir(outputPath + $"/{ipaBinaryName}"), true);
                            binaryFile.ExtractToFile(tempPath + "armv7", true);
                            Dumper(tempPath + "armv7", tempPath + "global-metadata.dat", FileDir(outputPath + "\\"));
                        }
                    }
                    else
                        WriteOutput("This IPA does not contain an IL2CPP application", Color.Yellow);
                }
                else
                {
                    WriteOutput("Failed to extract required file. Please extract the files manually", Color.Yellow);
                }
                archive.Dispose();
            });
        }

        async Task APKDump(string file, string outputPath)
        {
            await Task.Factory.StartNew(() =>
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
                    metadataFile.ExtractToFile(tempPath + "global-metadata.dat", true);

                    foreach (var entry in archive.Entries)
                    {
                        if (entry.FullName.Equals(@"lib/armeabi-v7a/libil2cpp.so"))
                        {
                            WriteOutput("Dumping ARMv7...", Color.Chartreuse);

                            if (Properties.Settings.Default.extBin)
                                entry.ExtractToFile(FileDir(outputPath + "\\ARMv7\\libil2cpp.so"), true);
                            entry.ExtractToFile(tempPath + "libil2cpparmv7", true);
                            Dumper(tempPath + "libil2cpparmv7", tempPath + "global-metadata.dat", FileDir(outputPath + "\\ARMv7\\"));
                        }

                        if (entry.FullName.Equals(@"lib/arm64-v8a/libil2cpp.so"))
                        {
                            WriteOutput("Dumping ARM64...", Color.Chartreuse);

                            if (Properties.Settings.Default.extBin)
                                entry.ExtractToFile(FileDir(outputPath + "\\ARM64\\libil2cpp.so"), true);
                            entry.ExtractToFile(tempPath + "libil2cpparm64", true);
                            Dumper(tempPath + "libil2cpparm64", tempPath + "global-metadata.dat", FileDir(outputPath + "\\ARM64\\"));
                        }

                        if (entry.FullName.Equals(@"lib/x86/libil2cpp.so"))
                        {
                            WriteOutput("Dumping x86...", Color.Chartreuse);

                            if (Properties.Settings.Default.extBin)
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

        async Task APKSplitDump(string file, string outputPath)
        {
            await Task.Factory.StartNew(() =>
            {
                using var archive = ZipFile.OpenRead(file);
                var binaryFile = archive.Entries.FirstOrDefault(f => f.Name.Contains("libil2cpp.so"));
                var metadataFile = archive.Entries.FirstOrDefault(f => f.FullName == "assets/bin/Data/Managed/Metadata/global-metadata.dat");

                if (metadataFile != null)
                {
                    Debug.WriteLine("Extracted global-metadata.dat to temp");
                    metadataFile.ExtractToFile(tempPath + "global-metadata.dat", true);
                }

                if (binaryFile != null)
                {
                    Debug.WriteLine("Extracted libil2cpp.so to temp");
                    binaryFile.ExtractToFile(tempPath + "libil2cpp.so", true);
                }

                if (File.Exists(tempPath + "libil2cpp.so") && File.Exists(tempPath + "global-metadata.dat"))
                {
                    Dumper(tempPath + "libil2cpp.so", tempPath + "global-metadata.dat", FileDir(outputPath + "\\"));
                }
                archive.Dispose();
            });
        }

        async Task APKsDump(string file, string outputPath)
        {
            await Task.Factory.StartNew(() =>
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

                        if (binaryFile != null)
                        {
                           binaryFile.ExtractToFile(tempPath + "libil2cpp.so", true);

                            foreach (var entry in entryBase.Entries)
                            {
                                if (entry.FullName.Equals(@"lib/armeabi-v7a/libil2cpp.so"))
                                {
                                    WriteOutput("Dumping ARMv7...", Color.Chartreuse);

                                    if (Properties.Settings.Default.extBin)
                                        entry.ExtractToFile(FileDir(outputPath + "\\ARMv7\\libil2cpp.so"), true);
                                    entry.ExtractToFile(tempPath + "libil2cpparmv7", true);
                                    Dumper(tempPath + "libil2cpparmv7", tempPath + "global-metadata.dat", FileDir(outputPath + "\\ARMv7\\"));
                                }

                                if (entry.FullName.Equals(@"lib/arm64-v8a/libil2cpp.so"))
                                {
                                    WriteOutput("Dumping ARM64...", Color.Chartreuse);

                                    if (Properties.Settings.Default.extBin)
                                        entry.ExtractToFile(FileDir(outputPath + "\\ARM64\\libil2cpp.so"), true);
                                    entry.ExtractToFile(tempPath + "libil2cpparm64", true);
                                    Dumper(tempPath + "libil2cpparm64", tempPath + "global-metadata.dat", FileDir(outputPath + "\\ARM64\\"));
                                }

                                if (entry.FullName.Equals(@"lib/x86/libil2cpp.so"))
                                {
                                    WriteOutput("Dumping x86...", Color.Chartreuse);

                                    if (Properties.Settings.Default.extBin)
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
        #endregion
    }
}
