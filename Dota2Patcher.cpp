#include "Dota2Patcher.h"

void DrawMenu() {
	std::cout << "[Dota2Patcher]" << std::endl;
	std::cout << "\n[Enter number]" << std::endl;
	std::cout << "[1] Patch All" << std::endl;
	std::cout << "[2] Unlock Dota Plus" << std::endl;
	std::cout << "[3] Patch sv_cheats" << std::endl;
	std::cout << "[4] Patch Gameinfo" << std::endl;
	std::cout << "[5] Revert Patches" << std::endl;
	std::cout << "\n>>> ";

	int USER_INPUT;
	std::cin >> USER_INPUT;

	switch (USER_INPUT) {
	case 1:
		if (Patcher::patch_dota_plus(false))
			std::cout << "[+] Dota Plus Unlocked" << std::endl;
		if (Patcher::patch_sv_cheats(false))
			std::cout << "[+] Sv_cheats Patched" << std::endl;
		if (Patcher::patch_gameinfo(false))
			std::cout << "[+] Gameinfo Patched" << std::endl;
		break;
	case 2:
		if (Patcher::patch_dota_plus(false))
			std::cout << "[+] Dota Plus Unlocked" << std::endl;
		break;
	case 3:
		if (Patcher::patch_sv_cheats(false))
			std::cout << "[+] Sv_cheats Patched" << std::endl;
		break;
	case 4:
		if (Patcher::patch_gameinfo(false))
			std::cout << "[+] Gameinfo Patched" << std::endl;
		break;
	case 5:
		if (Patcher::patch_dota_plus(true))
			std::cout << "[+] Dota Plus Reverted" << std::endl;
		if (Patcher::patch_sv_cheats(true))
			std::cout << "[+] Sv_cheats Reverted" << std::endl;
		if (Patcher::patch_gameinfo(true))
			std::cout << "[+] Gameinfo Reverted" << std::endl;
		break;
	default:
		DrawMenu();
		break;
	}
}

int main() {
	if (Patcher::CheckUpdate()) {
		std::cout << "Update required" << std::endl;
		system("pause");
		return 0;
	}

    if (!Paths::get_dota_path(&Globals::dota_path)) {
        system("pause");
        return 0;
    }

    DrawMenu();

    system("pause");
    return 0;
}
