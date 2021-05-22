# Dota2Patcher
## Usage

* Start `Dota2Patcher.exe`
	* Enter manually path to DotA 2 (include 'dota 2 beta') if can't auto detect path
* Wait while files of DotA2 will be patched (will be writted `Patched`)
* Close `Dota2Patcher.exe`
* Done ;)

## **NOTICE: You need to patch after almost every update of the game client!**

## Removing from DotA 2

* Just [Verify Integrity of Game Files](https://support.steampowered.com/kb/2037-QEUH-3335/verify-integrity-of-game-cache?l=english)
* All patched files will be removed

### Popular convars (cheat commands):

* `dota_use_particle_fow`: default `1`
	* `0` - Show hidden spells (particles) and teleports in map's fog
		* ex. `dota_use_particle_fow 0`
* `fog_enable`: default `1`
	* `0` - Remove fog
		* ex. `fog_enable 0`
* `fow_client_nofiltering`: default `0`
	* `1` - Remove anti-aliasing of fog
		* ex. `fow_client_nofiltering 1`
* `dota_camera_distance`: default `1134`
	* `*any number*` - change camera distance
		* ex. `dota_camera_distance 1600`
* `r_farz`: default `-1`
	* `18000` - Override the far clipping plane
	* You need multiply x2 of camera distance or just set `18000`
		* ex. `dota_camera_distance 1600` -> `r_farz 3200`
* `dota_range_display`: default `0`
	* `*any number*` - Displays a ring around the hero at the specified radius
		* ex. `dota_range_display 1200`
* `cl_weather`: default `0`
	* `*any number*`(1-10) - Change weather
		* ex. `cl_weather 8`

### Raw

* just copy raw list and past to console after pathing

```
dota_use_particle_fow 0;
fog_enable 0;
fow_client_nofiltering 1;
dota_camera_distance 1600;
r_farz 18000;
dota_range_display 1200;
cl_weather 8;
```
