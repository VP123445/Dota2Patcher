using Microsoft.Win32;
using System;
using System.IO;

namespace Dota2Patcher
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Patching...");
            Determine_dota_path();
            Patcher.Patch();
            Console.WriteLine("Patched!");
            Console.ReadLine();
        }

        internal static DirectoryInfo path_to_dota;

        internal static void Determine_dota_path()
        {
            using (RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
            {
                using (RegistryKey registryKey2 = registryKey1.OpenSubKey("Software\\Classes\\dota2\\Shell\\Open\\Command"))
                {
                    object obj = registryKey2.GetValue("");
                    if (!(obj.GetType().Name == "String"))
                        return;
                    path_to_dota = new DirectoryInfo(obj.ToString().Split(new string[1] {
            "\""
          }, StringSplitOptions.None)[1].Split(new string[1] {
            "game\\bin\\win"
          }, StringSplitOptions.None)[0]);
                }
            }
        }

    }
}
