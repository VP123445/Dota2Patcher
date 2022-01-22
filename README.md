# NEW: [gameinfo.gi CRC file check](https://www.reddit.com/r/DotA2/comments/s46mj8/dota_2_update_for_11422_11522_utc) bypass
Allows you to use vpk mods like well known [No-Bling](https://github.com/No-Bling/DOTA).  

### How To:
* Download [latest Dota2Patcher release](https://github.com/Wolf49406/Dota2Patcher/releases/latest);
* Launch it, select `[1] Patch All` or `[3] Patch Gameinfo`;
* Drop desired vpk to your mods folder inside `\dota 2 beta\game\` (ex: `\dota 2 beta\game\dota_tempcontent`);
* Open `\dota 2 beta\game\dota\gameinfo.gi` with text editor;
* Change line 39 to `Game dota_tempcontent`
* And line 40 to `Mod dota_tempcontent`

gameinfo.gi should look like this:  
![image](https://user-images.githubusercontent.com/26302888/150647061-251ac247-18f8-407c-847b-3657947f43b2.png)


# sv_cheats unlock
Allows you to use any cheat-protected commands in multiplayer (ex: `dota_camera_distance 1500`)

### Popular convars (cheat commands):
* `sv_cheats`: default `0`
	* `1` - Unlock convars thats have cheat flag (use it first!)
		* ex. `sv_cheats 1`
* `dota_use_particle_fow`: default `1`
	* `0` - Show hidden spells (particles) and teleports in map's fog
		* ex. `dota_use_particle_fow 0`
* `fog_enable`: default `1`
	* `0` - Remove fog
		* ex. `fog_enable 0`
* `fow_client_nofiltering`: default `0`
	* `1` - Remove anti-aliasing of fog
		* ex. `fow_client_nofiltering 1`
* `dota_camera_distance`: default `1200`
	* `*any number*` - change camera distance
		* ex. `dota_camera_distance 1500`
* `r_farz`: default `-1`
	* `18000` - Override the far clipping plane
	* You need multiply x2 of camera distance or just set `18000`
		* ex. `dota_camera_distance 1500` -> `r_farz 3000`
* `dota_range_display`: default `0`
	* `*any number*` - Displays a ring around the hero at the specified radius
		* ex. `dota_range_display 1200`
* `cl_weather`: default `0`
	* `*any number*`(1-10) - Change weather
		* ex. `cl_weather 8`

### Raw

* just copy raw list and past to console

```
sv_cheats1; dota_use_particle_fow 0; fog_enable 0; fow_client_nofiltering 1; dota_camera_distance 1500; r_farz 3000;
```

# Removing from Dota 2
* From launcher menu select `[4] Revert Patches`
