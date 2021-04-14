using System;
using System.IO;

namespace Dota2Patcher
{
    static class Patcher
    {
        private static readonly byte[][][] sigEngine = {
            new byte[2][] {
                new byte[] { 0x74, 0x54, 0x48, 0x8B, 0x0D, 0xCC, 0xCC, 0xCC, 0xCC, 0x48, 0x8B, 0x01, 0xFF, 0x90, 0xCC, 0xCC, 0xCC, 0xCC, 0x84, 0xC0, 0x74, 0x40, 0x48, 0x8B, 0x0D },
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
        
        public static void Patch()
        {
            string pathToEngine = Program.path_to_engine2.ToString();
            byte[] engineBytes = File.ReadAllBytes(pathToEngine);
            
            foreach (byte[][] find in sigEngine)
            {
                if (engineBytes != null)
                    engineBytes = ReplaceBytes(engineBytes, /* original */ find[0], /* cracked */ find[1]);
            }
            if (engineBytes != null)
                File.WriteAllBytes(pathToEngine, engineBytes);
        }

    }
}
