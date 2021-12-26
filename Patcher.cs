using System;
using System.IO;

// You need IDA and Function String Associate plugin
// Open engine2.dll, go to Edit => Plugins => Function String Associate
// View => Open subviews => Strings
// Ctrl+F, search for "SV: Convar '%s' is cheat protected, change ignored."
// Put cursor on "aSvConvarSIsChe", press X, open first xref, open function
// Go to start of the function, scroll down and search for similar code

// .text:0000000180158617                               loc_180158617:                          ; CODE XREF: sub_180158160+4AE↑j
// .text:0000000180158617 48 8B D6                                      mov     rdx, rsi
// .text:000000018015861A 48 8D 0D 57 84 34 00                          lea     rcx, aSS_15     ; "%s = %s\n"
// .text:0000000180158621 FF 15 61 5F 2A 00                             call    cs:?ConMsg@@YAXPEBDZZ ; ConMsg(char const *,...)
// .text:0000000180158627 E9 4F 01 00 00                                jmp     loc_18015877B
// .text:000000018015862C                               ; ---------------------------------------------------------------------------
// .text:000000018015862C
// .text:000000018015862C                               loc_18015862C:                          ; CODE XREF: sub_180158160+440↑j
// .text:000000018015862C BA 00 40 00 00                                mov     edx, 4000h
// .text:0000000180158631 41 FF D0                                      call    r8
// .text:0000000180158634 84 C0                                         test    al, al <<---- HERE
// .text:0000000180158636 74 6E                                         jz      short loc_1801586A6

// You need to change "test al, al" to "test al, 0" (84 C0 to A8 00)
// First "al" is probably sv_cheats ConVar value.
// Second is "cheat status" of your command. I guess.

namespace Dota2Patcher {
    static class Patcher {
        private static readonly byte[][][] SigToPatch = {
            new byte[2][] {
                new byte[] { 0x84, 0xC0, 0x74, 0xCC, 0xE8, 0xCC, 0xCC, 0xCC, 0xCC, 0x84, 0xC0, 0x75, 0xCC, 0xE8 },
                new byte[] { 0xA8, 0x00 }
            }
        };

        private static readonly byte[][][] SigToValidatePatch = {
            new byte[1][] {
                new byte[] { 0xA8, 0x00, 0x74, 0xCC, 0xE8, 0xCC, 0xCC, 0xCC, 0xCC, 0x84, 0xC0, 0x75, 0xCC, 0xE8 }
            }
        };

        private static int FindBytes(byte[] src, byte[] find) {
            int index = -1;
            int matchIndex = 0;
            
            for (int i = 0; i < src.Length; i++) {
                if (find[matchIndex] == 0xCC || src[i] == find[matchIndex]) {
                    if (matchIndex == (find.Length - 1)) {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                    matchIndex = 1;
                else
                    matchIndex = 0;
            }
            return index;
        }
        
        private static byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl) {
            byte[] dst = null;
            int index = FindBytes(src, search);
            if (index == -1)
                return null;

            if (index >= 0) {
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

        internal static bool AlreadyPatched(byte[] src, byte[] search) {
            int index = FindBytes(src, search);
            if (index == -1)
                return false;

            return true;
        }

        internal static bool Patch() {
            string pathToDLL = Program.path_to_dota.ToString() + "game\\bin\\win64\\engine2.dll";
            byte[] DllBytesOrig;
            byte[] DllBytesRepl = null;

            try {
                DllBytesOrig = File.ReadAllBytes(pathToDLL);
            }
            catch (Exception Ex) {
                Console.WriteLine("[-] Can't open DLL. Error: \n\n" + Ex);
                return false;
            }

            foreach (byte[][] find in SigToPatch) {
                DllBytesRepl = ReplaceBytes(DllBytesOrig, find[0], find[1]);
            }

            if (DllBytesRepl == null) {
                foreach (byte[][] find in SigToValidatePatch) {
                    bool Patched = AlreadyPatched(DllBytesOrig, find[0]);
                    if (Patched) { 
                        Console.WriteLine("[+] Already patched");
                        return false;
                    }
                }

                Console.WriteLine("[-] Patch signature broken!");
                return false;
            }

            try {
                File.WriteAllBytes(pathToDLL, DllBytesRepl);
            }
            catch (Exception Ex) {
                Console.WriteLine("[-] Can't write patched DLL. Error: \n\n" + Ex);
                return false;
            }

            return true;
        }
    }
}
