using System;
using System.IO;
using Il2CppDumperGui;

namespace Il2CppDumper
{
    public abstract class ElfBase : Il2Cpp
    {
        public bool IsDumped;
        public ulong DumpAddr;

        protected ElfBase(Stream stream) : base(stream) { }

        public void GetDumpAddress()
        {
            Program.frmMain.WriteOutput("Detected this may be a dump file.");
            Program.frmMain.WriteOutput("Input il2cpp dump address or input 0 to force continue:");
            string result = null;
            InputBox.Show("Input il2cpp dump address or leave empty to force continue:", "",
                ref result);
            if (result != null)
            {
                DumpAddr = Convert.ToUInt64(result, 16);
                Program.frmMain.WriteOutput("Inputted address: " + DumpAddr.ToString("X"));
            }
            
            if (DumpAddr != 0)
            {
                IsDumped = true;
            }
        }
    }
}
