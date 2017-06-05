; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Remote Server Controller"
#define MyAppVersion "0.2017.156.14232"
#define MyAppYear "2017"
#define MyAppPublisher "Karel Prajs"
#define MyAppURL "https://github.com/prajs-org/remote-server-controller"
#define MyAppExeName "RemoteServerController.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{F301F98B-72DF-4338-9B02-25DFD57763B3}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppCopyright=Copyright (C) {#MyAppYear} {#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=E:\Work\prajs-org\remote-server-controller\RemoteServerController\license.txt
OutputBaseFilename=RemoteServerControllerSetup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "E:\Work\prajs-org\remote-server-controller\RemoteServerController\RscHost\bin\Release\RemoteServerController.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Work\prajs-org\remote-server-controller\RemoteServerController\RscHost\bin\Release\RemoteServerController.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Work\prajs-org\remote-server-controller\RemoteServerController\RscHost\bin\Release\RscConfig.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Work\prajs-org\remote-server-controller\RemoteServerController\RscHost\bin\Release\RscCore.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Work\prajs-org\remote-server-controller\RemoteServerController\RscHost\bin\Release\RscInterface.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Work\prajs-org\remote-server-controller\RemoteServerController\RscHost\bin\Release\RscLog.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Work\prajs-org\remote-server-controller\RemoteServerController\RscHost\bin\Release\log4net.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Work\prajs-org\remote-server-controller\RemoteServerController\RscHost\bin\Release\log4net.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Work\prajs-org\remote-server-controller\RemoteServerController\license.txt"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\RemoteServerController.exe"; Parameters: "/INSTALLSERVICE";

[UninstallRun]
Filename: "{app}\RemoteServerController.exe"; Parameters: "/UNINSTALLSERVICE";


