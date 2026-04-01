; Typist - Inno Setup Installer Script
; -----------------------------------------------------------------------------
; IMPORTANT PRE-REQUISITE:
; Before compiling this script in Inno Setup, you must publish the application.
; Open a terminal in the 'Typist' root directory and run:
;
; dotnet publish ./Typist.Desktop/Typist.Desktop.csproj -c Release --self-contained false
;
; (Or if you want a standalone single-file executable, use:)
; dotnet publish ./Typist.Desktop/Typist.Desktop.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
; -----------------------------------------------------------------------------

#define MyAppName "Typist"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Typist Developer"
#define MyAppExeName "Typist.Desktop.exe"
#define MyAppIcon "..\Typist.Desktop\AppIcon.ico"

; NOTE: Make sure this path matches your exact `dotnet publish` output. 
; If you used the `-r win-x64` flag, your path will look like: "..\Typist.Desktop\bin\Release\net8.0-windows\win-x64\publish"
#define PublishDir "..\Typist.Desktop\bin\Release\net8.0-windows\publish"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications!
AppId={{DA4A92F2-8703-4B9F-8380-6BB0DF98E6DE}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}

; Default install directory is Program Files \ Typist
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
AllowNoIcons=yes

; The installer executable will be created in the installer/Output folder
OutputDir=.\Output
OutputBaseFilename=Typist_Setup_v{#MyAppVersion}
SetupIconFile={#MyAppIcon}

Compression=lzma2/max
SolidCompression=yes
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
WizardStyle=modern

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
; Copy the main executable directly
Source: "{#PublishDir}\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
; Copy all other published assets (dlls, runtimes, assets)
Source: "{#PublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
; Creates the Start Menu shortcut
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
; Creates the Desktop shortcut if the user checks the box
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
; Gives the user an option to run the app immediately after installation finishes
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
