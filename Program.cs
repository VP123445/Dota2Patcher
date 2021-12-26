using Microsoft.Win32;
using System;
using System.IO;

namespace Dota2Patcher {
    class Program {
        static void Main() {
            Console.WriteLine("Patching...");
            if(Determine_dota_path())
                if(Patcher.Patch())
                    Console.WriteLine("[+] Patched");
            Console.ReadLine();
        }

        internal static DirectoryInfo path_to_dota;

        internal static bool Determine_dota_path() {
            RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
            RegistryKey registryKey2 = registryKey1.OpenSubKey("Software\\Classes\\dota2\\Shell\\Open\\Command");

            if (registryKey1 == null) {
                Console.WriteLine("[-] Registry Read error => [OpenBaseKey]");
                return false;
            }

            if (registryKey2 == null) {
                Console.WriteLine("[-] Can't find Dota 2 path in registry");
                return false;
            }

            object obj = registryKey2.GetValue("");
            if (obj == null) {
                Console.WriteLine("[-] Can't read Dota 2 path from registry");
                return false;
            }

            path_to_dota = new DirectoryInfo(obj.ToString().Split(new string[1] { "\"" }, StringSplitOptions.None)[1].Split(new string[1] { "game\\bin\\win" }, StringSplitOptions.None)[0]);

            return true;
        }
    }
}
