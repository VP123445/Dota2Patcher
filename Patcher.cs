using System;
using System.IO;

namespace Dota2Patcher
{
    static class Patcher
    {
        private static readonly byte[][][] sig = {
            new byte[2][] {
                new byte[] { 0x75, 0xCC, 0x48, 0x8B, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xBA, 0xCC, 0xCC, 0xCC, 0xCC, 0x8B, 0x08, 0xFF, 0x15, 0xCC, 0xCC, 0xCC, 0xCC, 0x84, 0xC0, 0x0F, 0x84, 0xCC, 0xCC, 0xCC, 0xCC, 0x83, 0x3F, 0xCC, 0x7E, 0xCC, 0x48, 0x8B, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x48, 0x8B, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x4C, 0x8D, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x4C, 0x8B, 0xCC, 0xBA, 0xCC, 0xCC, 0xCC, 0xCC, 0x8B, 0x08, 0xFF, 0x15, 0xCC, 0xCC, 0xCC, 0xCC, 0xE9, 0xCC, 0xCC, 0xCC, 0xCC, 0x48, 0x8B, 0xCC, 0xBA },
                new byte[] { 0xEB }
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

        internal static void Patch()
        {
            string pathToDLL = Program.path_to_dota.ToString() + "game\\bin\\win64\\engine2.dll";
            byte[] DllBytes = File.ReadAllBytes(pathToDLL);
            
            foreach (byte[][] find in sig)
            {
                if (DllBytes != null)
                    DllBytes = ReplaceBytes(DllBytes, find[0], find[1]);
            }
            if (DllBytes != null)
                File.WriteAllBytes(pathToDLL, DllBytes);
        }

    }
}
