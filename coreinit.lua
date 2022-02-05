local EnableConvars = function(s)
  local cmd = "sv_cheats 1; dota_use_particle_fow 0; fog_enable 0; fow_client_nofiltering 1; dota_camera_distance 1500; r_farz 3000;";
  if SendToServerConsole then SendToServerConsole(cmd) else SendToConsole(cmd) end
end

if SendToServerConsole then
  ListenToGameEvent("dota_player_pick_hero", EnableConvars, nil)
end
