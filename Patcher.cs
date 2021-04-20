using System;
using System.IO;

namespace Dota2Patcher
{
    static class Patcher
    {
        private static readonly byte[][][] sigEngine = {
            // Unlock cheat-protected commands
            new byte[2][] {
                new byte[] { 0x74, 0x54, 0x48, 0x8B, 0x0D, 0xCC, 0xCC, 0xCC, 0xCC, 0x48, 0x8B, 0x01, 0xFF, 0x90, 0xCC, 0xCC, 0xCC, 0xCC, 0x84, 0xC0, 0x74, 0x40, 0x48, 0x8B, 0x0D },
                new byte[] { 0xEB }
            },
            // sv_cheats 1
            new byte[2][] {
                new byte[] { 0x83, 0x78, 0x58, 0x00, 0x74, 0xCC, 0xBA, 0xCC, 0xCC, 0xCC, 0xCC, 0x48, 0x8D, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xE8, 0xCC, 0xCC, 0xCC, 0xCC, 0x84, 0xC0 },
                new byte[] { 0x83, 0x78, 0x58, 0x01 }
            },
            // fog_enable 0
            new byte[2][] {
                new byte[] { 0xB0, 0x01, 0xEB, 0xCC, 0x32, 0xC0, 0x88, 0x85 },
                new byte[] { 0xB0, 0x00 }
            }
        };

        private static readonly byte[][][] sigClient = {
            // dota_camera_distance 1350
            new byte[2][] {
                new byte[] { 0x31, 0x32, 0x30, 0x30 },
                new byte[] { 0x31, 0x33, 0x35, 0x30 }
            },
            // dota_use_particle_fow 1
            new byte[2][] {
                new byte[] { 0x83, 0x78, 0x58, 0x00, 0x48, 0x8B, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x0F, 0x95, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x83, 0x78, 0x58, 0xCC, 0x48, 0x8B, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x0F, 0x95 },
                new byte[] { 0x83, 0x78, 0x58, 0x01 }
            }
        };

        private static int FindBytes(byte[] src, byte[] find)
        {
            int index = -1;
            int matchIndex = 0;
            
            for (int i = 0; i < src.Length; i++)
            {
                if (find[matchIndex] == 0xCC || src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }
            }
            return index;
        }
        
        private static byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {
            byte[] dst = null;
            int index = FindBytes(src, search);
            if (index >= 0)
            {
                dst = new byte[src.Length];
                Buffer.BlockCopy(src, 0, dst, 0, index);
                Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                Buffer.BlockCopy(
                    src,
                    index + repl.Length,
                    dst,
                    index + repl.Length,
                    src.Length - (index + repl.Length));
            }
            return dst;
        }
        
        private static void Patch_Engine()
        {
            string pathToDLL = Program.path_to_engine2.ToString() + "game\\bin\\win64\\engine2.dll";
            byte[] DllBytes = File.ReadAllBytes(pathToDLL);
            
            foreach (byte[][] find in sigEngine)
            {
                if (DllBytes != null)
                    DllBytes = ReplaceBytes(DllBytes, find[0], find[1]);
            }
            if (DllBytes != null)
                File.WriteAllBytes(pathToDLL, DllBytes);
        }

        private static void Patch_Client()
        {
            string pathToDLL = Program.path_to_engine2.ToString() + "game\\dota\\bin\\win64\\client.dll";
            byte[] DllBytes = File.ReadAllBytes(pathToDLL);

            foreach (byte[][] find in sigClient)
            {
                if (DllBytes != null)
                    DllBytes = ReplaceBytes(DllBytes, find[0], find[1]);
            }
            if (DllBytes != null)
                File.WriteAllBytes(pathToDLL, DllBytes);
        }

        public static void Patch()
        {
            Patch_Engine();
            Patch_Client();
        }

    }
}
