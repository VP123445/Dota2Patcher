#pragma once
#include <Windows.h>
#include <iostream>

namespace Paths {
	bool get_dota_path_from_reg(std::string* path);
	bool get_dota_path(std::string* path);
}

namespace Patcher {
	int find_offset(char* array, int array_length, BYTE* pattern, int pattern_length);
	int find_offset(std::string file_path, BYTE* pattern, int pattern_size);
	bool get_byte_array(std::string file_path, char** ret_array, int* file_size);
	void apply_patch(std::string file_path, int patch_offset, BYTE replace[], int bytes_to_replace);

	bool patch_gameinfo(bool revert);
	bool patch_sv_cheats(bool revert);
}

namespace Globals {
	inline std::string dota_path;

	// Patch
	inline BYTE gameinfo_pattern[] = { 0x44, 0x38, 0x67, 0x39, 0x74, 0x39 };
	inline BYTE gameinfo_replace[] = { 0xEB };
	// Revert
	inline BYTE gameinfo_revert_pattern[] = { 0x44, 0x38, 0x67, 0x39, 0xEB, 0x39 };
	inline BYTE gameinfo_revert_replace[] = { 0x74 };

	inline int gameinfo_offset = 4;

	inline BYTE sv_cheats_pattern[] = { 0x74, 0x54, 0x48, 0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x01, 0xFF, 0x90, 0x00, 0x00, 0x00, 0x00, 0x84, 0xC0, 0x74, 0x40, 0x48, 0x8B, 0x0D };
	inline BYTE sv_cheats_replace[] = { 0xEB };

	inline BYTE sv_cheats_revert_pattern[] = { 0xEB, 0x54, 0x48, 0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x01, 0xFF, 0x90, 0x00, 0x00, 0x00, 0x00, 0x84, 0xC0, 0x74, 0x40, 0x48, 0x8B, 0x0D };
	inline BYTE sv_cheats_revert_replace[] = { 0x74 };
}
