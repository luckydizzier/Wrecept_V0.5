[Setup]
AppName=Wrecept
AppVersion=1.0.0
DefaultDirName={pf}\Wrecept
DefaultGroupName=Wrecept
OutputBaseFilename=WreceptInstaller
Compression=lzma
SolidCompression=yes

[Files]
Source: "publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Wrecept"; Filename: "{app}\Wrecept.exe"
Name: "{group}\Uninstall Wrecept"; Filename: "{uninstallexe}"

