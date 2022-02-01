#pragma once
#include <Windows.h>
#include <iostream>

namespace Paths {
	bool get_dota_path_from_reg(std::string* path);
	bool get_dota_path(std::string* path);
}

namespace Patcher {
	void CheckUpdate();
	int find_offset(char* array, int array_length, BYTE* pattern, int pattern_length);
	int find_offset(std::string file_path, BYTE* pattern, int pattern_size);
	bool get_byte_array(std::string file_path, char** ret_array, int* file_size);
	void apply_patch(std::string file_path, int patch_offset, BYTE replace[], int bytes_to_replace);

	bool patch_gameinfo(bool revert);
	bool patch_sv_cheats(bool revert);
}

namespace Globals {
	inline std::string local_version = "v3.0.4.2";
	inline std::string dota_path;

	inline BYTE gameinfo_pattern[] = { 0x74, 0x00, 0x48, 0x8B, 0x00, 0x48, 0x3B, 0x00, 0x0F, 0x95, 0x00, 0xFF, 0x50, 0x00, 0x48, 0x89, 0x00, 0x00, 0x48, 0x8D };
	inline BYTE sv_cheats_pattern[] = { 0x74, 0x54, 0x48, 0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x01, 0xFF, 0x90, 0x00, 0x00, 0x00, 0x00, 0x84, 0xC0, 0x74, 0x40, 0x48, 0x8B, 0x0D };
}

// How to update gameinfo_pattern

// Open x64dbg, attach to Dota process. 
// Press Ctrl+G, type "client.base", press Enter.
// Right click on assembly > Search for > Current module > String references
// Wait for it to load, search for "PlayDisabled_LocalFiles"

// 00007FFA7613425F | 48:817D AF 1C1E0000      | cmp qword ptr ss:[rbp-51],1E1C          | << 1E1C = file size
// 00007FFA76134267 | 75 1C                    | jne client.7FFA76134285                 |
// 00007FFA76134269 | 0FB605 B12D3102          | movzx eax,byte ptr ds:[7FFA78447021]    |
// 00007FFA76134270 | B9 01000000              | mov ecx,1                               |
// 00007FFA76134275 | 817D 7F 4FFC524A         | cmp dword ptr ss:[rbp+7F],4A52FC4F      | << 4A52FC4F = file CRC32
// 00007FFA7613427C | 0F44C1                   | cmove eax,ecx                           |
// 00007FFA7613427F | 8805 9C2D3102            | mov byte ptr ds:[7FFA78447021],al       |
// 00007FFA76134285 | 4D:85FF                  | test r15,r15                            |
// 00007FFA76134288 | 74 17                    | je client.7FFA761342A1                  |
// 00007FFA7613428A | 41:83BF 00030000 0B      | cmp dword ptr ds:[r15+300],B            |
// 00007FFA76134292 | 75 0D                    | jne client.7FFA761342A1                 |
// 00007FFA76134294 | 48:8D4C24 38             | lea rcx,qword ptr ss:[rsp+38]           |
// 00007FFA76134299 | FF15 41949B00            | call qword ptr ds:[<&?Purge@CUtlString@ |
// 00007FFA7613429F | EB 3F                    | jmp client.7FFA761342E0                 |
// 00007FFA761342A1 | 807F 39 00               | cmp byte ptr ds:[rdi+39],0              |
// 00007FFA761342A5 | 74 39                    | je client.7FFA761342E0                  |
// 00007FFA761342A7 | 803D 732D3102 00         | cmp byte ptr ds:[7FFA78447021],0        |
// 00007FFA761342AE | 75 30                    | jne client.7FFA761342E0                 |
// 00007FFA761342B0 | 83BF 08010000 08         | cmp dword ptr ds:[rdi+108],8            |
// 00007FFA761342B7 | 74 27                    | je client.7FFA761342E0                  |
// 00007FFA761342B9 | 48:8B4B 38               | mov rcx,qword ptr ds:[rbx+38]           |
// 00007FFA761342BD | 48:85C9                  | test rcx,rcx                            |
// 00007FFA761342C0 | 74 10                    | je client.7FFA761342D2                  | << Patch here. je > jmp (74 > EB)
// 00007FFA761342C2 | 48:8B01                  | mov rax,qword ptr ds:[rcx]              |
// 00007FFA761342C5 | 48:3BCB                  | cmp rcx,rbx                             |
// 00007FFA761342C8 | 0F95C2                   | setne dl                                |
// 00007FFA761342CB | FF50 20                  | call qword ptr ds:[rax+20]              |
// 00007FFA761342CE | 48:8973 38               | mov qword ptr ds:[rbx+38],rsi           |
// 00007FFA761342D2 | 48:8D05 DF519601         | lea rax,qword ptr ds:[7FFA77A994B8]     | 00007FFA77A994B8:"PlayDisabled_LocalFiles"