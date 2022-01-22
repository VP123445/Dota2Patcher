#include "Dota2Patcher.h"
#include <fstream>

int Patcher::find_offset(char* array, int array_length, BYTE* pattern, int pattern_length) {
    for (int haystack_index = 0; haystack_index < array_length; haystack_index++) {
        bool needle_found = true;
        for (size_t needle_index = 0; needle_index < pattern_length; needle_index++) {
            char haystack_character = array[haystack_index + needle_index];
            char needle_character = pattern[needle_index];
            if (needle_character == 0x00 || haystack_character == needle_character)
                continue;
            else {
                needle_found = false;
                break;
            }
        }
        if (needle_found)
            return haystack_index;
    }

    return 0;
}

bool Patcher::get_byte_array(std::string file_path, char** ret_array, int* file_size) {
    std::ifstream file(file_path, std::ios::binary | std::ios::ate);
    int file_length = static_cast<int>(file.tellg());
    if (file_length == 0) {
        std::cout << "[-] File Size is NULL!" << std::endl;
        return false;
    }

    char* array = new char[file_length];
    file.rdbuf()->pubseekoff(0, std::ios_base::beg);
    file.read(array, file_length);
    file.close();

    *file_size = file_length;
    *ret_array = array;

    return true;
}

int Patcher::find_offset(std::string file_path, BYTE* pattern, int pattern_size) {
    char* array = nullptr;
    int file_size = 0;

    if (!get_byte_array(file_path, &array, &file_size))
        return 0;

    int patch_offset = find_offset(array, file_size, pattern, pattern_size);

    return patch_offset;
}

void Patcher::apply_patch(std::string file_path, int patch_offset, BYTE replace[], int bytes_to_replace) {
    FILE* pFile;
    fopen_s(&pFile, file_path.c_str(), "r+b");

    for (int i = 0; i < bytes_to_replace; i++) {
        fseek(pFile, patch_offset + i, SEEK_SET);
        fputc(replace[i], pFile);
    }

    fclose(pFile);
}

// client.dll - gameinfo.gi CRC check bypass
bool Patcher::patch_gameinfo(bool revert) {
    std::string client_path = Globals::dota_path + "dota\\bin\\win64\\client.dll";

    BYTE* gameinfo_pattern = Globals::gameinfo_pattern;
    BYTE* gameinfo_replace = Globals::gameinfo_replace;
    if (revert) {
        gameinfo_pattern = Globals::gameinfo_revert_pattern;
        gameinfo_replace = Globals::gameinfo_revert_replace;
    }

    int client_patch_offset = Patcher::find_offset(client_path, gameinfo_pattern, sizeof(Globals::gameinfo_pattern));
    if (!client_patch_offset) {
        std::cout << "[-] Gameinfo Bypass Offset is NULL!" << std::endl;
        return false;
    }

    Patcher::apply_patch(client_path, client_patch_offset + Globals::gameinfo_offset, gameinfo_replace, sizeof(Globals::gameinfo_replace));

    return true;
}

// engine.dll - sv_cheats bypass
bool Patcher::patch_sv_cheats(bool revert) {
    std::string engine_path = Globals::dota_path + "bin\\win64\\engine2.dll";

    BYTE* sv_cheats_pattern = Globals::sv_cheats_pattern;
    BYTE* sv_cheats_replace = Globals::sv_cheats_replace;
    if (revert) {
        sv_cheats_pattern = Globals::sv_cheats_revert_pattern;
        sv_cheats_replace = Globals::sv_cheats_revert_replace;
    }

    int engine_patch_offset = Patcher::find_offset(engine_path, sv_cheats_pattern, sizeof(Globals::sv_cheats_pattern));
    if (!engine_patch_offset) {
        std::cout << "[-] Sv_cheats Bypass Offset is NULL!" << std::endl;
        return false;
    }

    Patcher::apply_patch(engine_path, engine_patch_offset, sv_cheats_replace, sizeof(Globals::sv_cheats_replace));

    return true;
}
