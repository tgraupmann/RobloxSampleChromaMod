[Setup]
AppName=Roblox Chroma RGB Mod
AppVerName=Roblox Chroma Mod RGB 1.1
AppPublisher=Tim Graupmann
AppPublisherURL=https://github.com/tgraupmann/RobloxSampleChromaMod
AppSupportURL=https://discord.gg/g7vZDkbnKT
AppUpdatesURL=https://github.com/tgraupmann/RobloxSampleChromaMod
DefaultDirName={pf32}\Razer\RobloxChromaRGBMod
DefaultGroupName=Razer\RobloxChromaRGBMod
OutputBaseFilename=RobloxChromaRGBModSetup
SetupIconFile=release_icon.ico
UninstallDisplayIcon=release_icon.ico
UninstallDisplayName=Uninstall Roblox Chroma RGB Mod
Compression=lzma
SolidCompression=yes
InfoBeforeFile=eula.txt
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "bin\Release\CChromaEditorLibrary.dll"; DestDir: "{pf32}\Razer\RobloxChromaRGBMod"; CopyMode: alwaysoverwrite
Source: "bin\Release\CChromaEditorLibrary64.dll"; DestDir: "{pf32}\Razer\RobloxChromaRGBMod"; CopyMode: alwaysoverwrite
Source: "bin\Release\WPF_RobloxChromaMod.exe"; DestDir: "{pf32}\Razer\RobloxChromaRGBMod"; CopyMode: alwaysoverwrite
Source: "bin\Release\WPF_RobloxChromaMod.exe.config"; DestDir: "{pf32}\Razer\RobloxChromaRGBMod"; CopyMode: alwaysoverwrite
Source: "Animations\*"; DestDir: "{pf32}\Razer\RobloxChromaRGBMod\Animations"; CopyMode: alwaysoverwrite

[Icons]
Name: "{group}\Roblox Chroma RGB Mod"; Filename: "{pf32}\Razer\RobloxChromaRGBMod\WPF_RobloxChromaMod.exe"; WorkingDir: "{app}";
Name: "{commondesktop}\Roblox Chroma RGB Mod"; Filename: "{pf32}\Razer\RobloxChromaRGBMod\WPF_RobloxChromaMod.exe"; WorkingDir: "{app}";
Name: "{group}\Uninstall Roblox Chroma RGB Mod"; Filename: "{uninstallexe}"

[Run]

Filename: "{pf32}\Razer\RobloxChromaRGBMod\WPF_RobloxChromaMod.exe"; Description: "Launch Roblox Chroma RGB Mod"; Flags: postinstall skipifsilent runasoriginaluser nowait; WorkingDir: "{app}"
