using System;
using System.Text;
using Windows_Key_Builder.Web.Models;

namespace Windows_Key_Builder.Web.Services
{
    public static class XmlGenerator
    {
        public static string GenerateUnattendXml(UnattendConfig config)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<unattend xmlns=\"urn:schemas-microsoft-com:unattend\" xmlns:wcm=\"http://schemas.microsoft.com/WMIConfig/2002/State\">");
            sb.AppendLine();

            GenerateWindowsPESettings(sb, config);
            GenerateSpecializeSettings(sb, config);
            GenerateOobeSystemSettings(sb, config);

            sb.AppendLine("</unattend>");
            return sb.ToString();
        }

        private static void GenerateWindowsPESettings(StringBuilder sb, UnattendConfig config)
        {
            sb.AppendLine("    <settings pass=\"windowsPE\">");

            if (config.BypassTPM)
            {
                sb.AppendLine("        <component name=\"Microsoft-Windows-Setup\" processorArchitecture=\"amd64\" publicKeyToken=\"31bf3856ad364e35\" language=\"neutral\" versionScope=\"nonSxS\">");
                sb.AppendLine("            <RunSynchronous>");

                int order = 1;

                sb.AppendLine($"                <RunSynchronousCommand wcm:action=\"add\">");
                sb.AppendLine($"                    <Order>{order++}</Order>");
                sb.AppendLine("                    <Path>reg add \"HKLM\\SYSTEM\\Setup\\LabConfig\" /v BypassTPMCheck /t REG_DWORD /d 1 /f</Path>");
                sb.AppendLine("                </RunSynchronousCommand>");

                sb.AppendLine($"                <RunSynchronousCommand wcm:action=\"add\">");
                sb.AppendLine($"                    <Order>{order++}</Order>");
                sb.AppendLine("                    <Path>reg add \"HKLM\\SYSTEM\\Setup\\LabConfig\" /v BypassSecureBootCheck /t REG_DWORD /d 1 /f</Path>");
                sb.AppendLine("                </RunSynchronousCommand>");

                sb.AppendLine($"                <RunSynchronousCommand wcm:action=\"add\">");
                sb.AppendLine($"                    <Order>{order++}</Order>");
                sb.AppendLine("                    <Path>reg add \"HKLM\\SYSTEM\\Setup\\LabConfig\" /v BypassRAMCheck /t REG_DWORD /d 1 /f</Path>");
                sb.AppendLine("                </RunSynchronousCommand>");

                sb.AppendLine($"                <RunSynchronousCommand wcm:action=\"add\">");
                sb.AppendLine($"                    <Order>{order++}</Order>");
                sb.AppendLine("                    <Path>reg add \"HKLM\\SYSTEM\\Setup\\LabConfig\" /v BypassStorageCheck /t REG_DWORD /d 1 /f</Path>");
                sb.AppendLine("                </RunSynchronousCommand>");

                sb.AppendLine($"                <RunSynchronousCommand wcm:action=\"add\">");
                sb.AppendLine($"                    <Order>{order++}</Order>");
                sb.AppendLine("                    <Path>reg add \"HKLM\\SYSTEM\\Setup\\LabConfig\" /v BypassCPUCheck /t REG_DWORD /d 1 /f</Path>");
                sb.AppendLine("                </RunSynchronousCommand>");

                sb.AppendLine("            </RunSynchronous>");
                sb.AppendLine("        </component>");
            }

            sb.AppendLine("        <component name=\"Microsoft-Windows-International-Core-WinPE\" processorArchitecture=\"amd64\" publicKeyToken=\"31bf3856ad364e35\" language=\"neutral\" versionScope=\"nonSxS\">");
            sb.AppendLine($"            <SetupUILanguage><UILanguage>{config.Language}</UILanguage></SetupUILanguage>");
            sb.AppendLine($"            <InputLocale>{config.Keyboard}</InputLocale>");
            sb.AppendLine($"            <SystemLocale>{config.Language}</SystemLocale>");
            sb.AppendLine($"            <UILanguage>{config.Language}</UILanguage>");
            sb.AppendLine($"            <UserLocale>{config.Language}</UserLocale>");
            sb.AppendLine("        </component>");

            sb.AppendLine("        <component name=\"Microsoft-Windows-Setup\" processorArchitecture=\"amd64\" publicKeyToken=\"31bf3856ad364e35\" language=\"neutral\" versionScope=\"nonSxS\">");

            if (config.AutoPartition)
            {
                GenerateDiskConfiguration(sb, config);
            }

            sb.AppendLine("            <ImageInstall>");
            sb.AppendLine("                <OSImage>");
            sb.AppendLine("                    <InstallTo>");
            sb.AppendLine("                        <DiskID>0</DiskID>");
            sb.AppendLine("                        <PartitionID>3</PartitionID>");
            sb.AppendLine("                    </InstallTo>");
            sb.AppendLine("                </OSImage>");
            sb.AppendLine("            </ImageInstall>");

            sb.AppendLine("            <UserData>");
            sb.AppendLine("                <AcceptEula>true</AcceptEula>");
            sb.AppendLine("                <ProductKey>");
            sb.AppendLine($"                    <Key>{config.ProductKey}</Key>");
            sb.AppendLine("                </ProductKey>");
            sb.AppendLine("            </UserData>");

            sb.AppendLine("        </component>");
            sb.AppendLine("    </settings>");
            sb.AppendLine();
        }

        private static void GenerateDiskConfiguration(StringBuilder sb, UnattendConfig config)
        {
            sb.AppendLine("            <DiskConfiguration>");
            sb.AppendLine("                <Disk wcm:action=\"add\">");
            sb.AppendLine("                    <DiskID>0</DiskID>");
            sb.AppendLine("                    <WillWipeDisk>true</WillWipeDisk>");
            sb.AppendLine("                    <CreatePartitions>");

            if (config.PartitionStyle == "GPT")
            {
                sb.AppendLine("                        <CreatePartition wcm:action=\"add\">");
                sb.AppendLine("                            <Order>1</Order>");
                sb.AppendLine("                            <Type>EFI</Type>");
                sb.AppendLine($"                            <Size>{config.EfiPartitionSize}</Size>");
                sb.AppendLine("                        </CreatePartition>");

                sb.AppendLine("                        <CreatePartition wcm:action=\"add\">");
                sb.AppendLine("                            <Order>2</Order>");
                sb.AppendLine("                            <Type>MSR</Type>");
                sb.AppendLine("                            <Size>16</Size>");
                sb.AppendLine("                        </CreatePartition>");

                sb.AppendLine("                        <CreatePartition wcm:action=\"add\">");
                sb.AppendLine("                            <Order>3</Order>");
                sb.AppendLine("                            <Type>Primary</Type>");
                sb.AppendLine("                            <Extend>true</Extend>");
                sb.AppendLine("                        </CreatePartition>");
            }
            else
            {
                sb.AppendLine("                        <CreatePartition wcm:action=\"add\">");
                sb.AppendLine("                            <Order>1</Order>");
                sb.AppendLine("                            <Type>Primary</Type>");
                sb.AppendLine("                            <Extend>true</Extend>");
                sb.AppendLine("                        </CreatePartition>");
            }

            sb.AppendLine("                    </CreatePartitions>");
            sb.AppendLine("                    <ModifyPartitions>");

            if (config.PartitionStyle == "GPT")
            {
                sb.AppendLine("                        <ModifyPartition wcm:action=\"add\">");
                sb.AppendLine("                            <Order>1</Order>");
                sb.AppendLine("                            <PartitionID>1</PartitionID>");
                sb.AppendLine("                            <Label>System</Label>");
                sb.AppendLine("                            <Format>FAT32</Format>");
                sb.AppendLine("                        </ModifyPartition>");

                sb.AppendLine("                        <ModifyPartition wcm:action=\"add\">");
                sb.AppendLine("                            <Order>2</Order>");
                sb.AppendLine("                            <PartitionID>2</PartitionID>");
                sb.AppendLine("                        </ModifyPartition>");

                sb.AppendLine("                        <ModifyPartition wcm:action=\"add\">");
                sb.AppendLine("                            <Order>3</Order>");
                sb.AppendLine("                            <PartitionID>3</PartitionID>");
                sb.AppendLine("                            <Label>Windows</Label>");
                sb.AppendLine("                            <Letter>C</Letter>");
                sb.AppendLine("                            <Format>NTFS</Format>");
                sb.AppendLine("                        </ModifyPartition>");
            }
            else
            {
                sb.AppendLine("                        <ModifyPartition wcm:action=\"add\">");
                sb.AppendLine("                            <Order>1</Order>");
                sb.AppendLine("                            <PartitionID>1</PartitionID>");
                sb.AppendLine("                            <Label>Windows</Label>");
                sb.AppendLine("                            <Letter>C</Letter>");
                sb.AppendLine("                            <Format>NTFS</Format>");
                sb.AppendLine("                            <Active>true</Active>");
                sb.AppendLine("                        </ModifyPartition>");
            }

            sb.AppendLine("                    </ModifyPartitions>");
            sb.AppendLine("                </Disk>");
            sb.AppendLine("            </DiskConfiguration>");
        }

        private static void GenerateSpecializeSettings(StringBuilder sb, UnattendConfig config)
        {
            sb.AppendLine("    <settings pass=\"specialize\">");

            sb.AppendLine("        <component name=\"Microsoft-Windows-Shell-Setup\" processorArchitecture=\"amd64\" publicKeyToken=\"31bf3856ad364e35\" language=\"neutral\" versionScope=\"nonSxS\">");

            if (!config.UseInteractiveComputerName)
            {
                sb.AppendLine($"            <ComputerName>{config.ComputerName}</ComputerName>");
            }

            sb.AppendLine($"            <TimeZone>{config.TimeZone}</TimeZone>");
            sb.AppendLine("        </component>");

            // Driver injection using DriverPaths method
            if (config.EnableDriverInjection && config.DriverInjectionMethod == "DriverPaths")
            {
                sb.AppendLine("        <component name=\"Microsoft-Windows-PnpCustomizationsNonWinPE\" processorArchitecture=\"amd64\" publicKeyToken=\"31bf3856ad364e35\" language=\"neutral\" versionScope=\"nonSxS\">");
                sb.AppendLine("            <DriverPaths>");
                sb.AppendLine("                <PathAndCredentials wcm:action=\"add\" wcm:keyValue=\"1\">");
                sb.AppendLine($"                    <Path>{config.DriverSourcePath}</Path>");
                sb.AppendLine("                </PathAndCredentials>");
                sb.AppendLine("            </DriverPaths>");
                sb.AppendLine("        </component>");
            }

            if (config.UseInteractiveComputerName || config.SetUACNeverNotify || config.EnableRDP)
            {
                sb.AppendLine("        <component name=\"Microsoft-Windows-Deployment\" processorArchitecture=\"amd64\" publicKeyToken=\"31bf3856ad364e35\" language=\"neutral\" versionScope=\"nonSxS\">");
                sb.AppendLine("            <RunSynchronous>");

                int order = 1;

                if (config.UseInteractiveComputerName)
                {
                    sb.AppendLine($"                <RunSynchronousCommand wcm:action=\"add\">");
                    sb.AppendLine($"                    <Order>{order++}</Order>");
                    sb.AppendLine("                    <Path>powershell -Command \"$name = Read-Host -Prompt 'Enter computer name'; if ($name) { Rename-Computer -NewName $name -Force }\"</Path>");
                    sb.AppendLine("                </RunSynchronousCommand>");
                }

                if (config.SetUACNeverNotify)
                {
                    sb.AppendLine($"                <RunSynchronousCommand wcm:action=\"add\">");
                    sb.AppendLine($"                    <Order>{order++}</Order>");
                    sb.AppendLine("                    <Path>reg add \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\" /v EnableLUA /t REG_DWORD /d 1 /f</Path>");
                    sb.AppendLine("                </RunSynchronousCommand>");

                    sb.AppendLine($"                <RunSynchronousCommand wcm:action=\"add\">");
                    sb.AppendLine($"                    <Order>{order++}</Order>");
                    sb.AppendLine("                    <Path>reg add \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\" /v ConsentPromptBehaviorAdmin /t REG_DWORD /d 0 /f</Path>");
                    sb.AppendLine("                </RunSynchronousCommand>");

                    sb.AppendLine($"                <RunSynchronousCommand wcm:action=\"add\">");
                    sb.AppendLine($"                    <Order>{order++}</Order>");
                    sb.AppendLine("                    <Path>reg add \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\" /v PromptOnSecureDesktop /t REG_DWORD /d 0 /f</Path>");
                    sb.AppendLine("                </RunSynchronousCommand>");
                }

                if (config.EnableRDP)
                {
                    sb.AppendLine($"                <RunSynchronousCommand wcm:action=\"add\">");
                    sb.AppendLine($"                    <Order>{order++}</Order>");
                    sb.AppendLine("                    <Path>reg add \"HKLM\\SYSTEM\\CurrentControlSet\\Control\\Terminal Server\" /v fDenyTSConnections /t REG_DWORD /d 0 /f</Path>");
                    sb.AppendLine("                </RunSynchronousCommand>");
                }

                sb.AppendLine("            </RunSynchronous>");
                sb.AppendLine("        </component>");
            }

            sb.AppendLine("    </settings>");
            sb.AppendLine();
        }

        private static void GenerateOobeSystemSettings(StringBuilder sb, UnattendConfig config)
        {
            sb.AppendLine("    <settings pass=\"oobeSystem\">");

            sb.AppendLine("        <component name=\"Microsoft-Windows-Shell-Setup\" processorArchitecture=\"amd64\" publicKeyToken=\"31bf3856ad364e35\" language=\"neutral\" versionScope=\"nonSxS\">");

            sb.AppendLine("            <OOBE>");
            sb.AppendLine("                <HideEULAPage>true</HideEULAPage>");
            sb.AppendLine("                <HideOEMRegistrationScreen>true</HideOEMRegistrationScreen>");
            sb.AppendLine("                <HideOnlineAccountScreens>true</HideOnlineAccountScreens>");
            sb.AppendLine("                <HideWirelessSetupInOOBE>true</HideWirelessSetupInOOBE>");
            sb.AppendLine("                <ProtectYourPC>3</ProtectYourPC>");
            sb.AppendLine("            </OOBE>");

            if (config.UserAccounts != null && config.UserAccounts.Count > 0)
            {
                sb.AppendLine("            <UserAccounts>");
                sb.AppendLine("                <LocalAccounts>");

                foreach (var account in config.UserAccounts)
                {
                    sb.AppendLine("                    <LocalAccount wcm:action=\"add\">");
                    sb.AppendLine($"                        <Name>{account.Username}</Name>");
                    sb.AppendLine($"                        <DisplayName>{account.Username}</DisplayName>");
                    sb.AppendLine($"                        <Group>{(account.IsAdmin ? "Administrators" : "Users")}</Group>");
                    sb.AppendLine("                        <Password>");
                    string passB64 = Convert.ToBase64String(Encoding.Unicode.GetBytes(account.Password + "Password"));
                    sb.AppendLine($"                            <Value>{passB64}</Value>");
                    sb.AppendLine("                            <PlainText>false</PlainText>");
                    sb.AppendLine("                        </Password>");
                    sb.AppendLine("                    </LocalAccount>");
                }

                sb.AppendLine("                </LocalAccounts>");
                sb.AppendLine("            </UserAccounts>");

                if (config.AutoLogin && config.UserAccounts.Count > 0)
                {
                    var firstAccount = config.UserAccounts[0];
                    string passB64 = Convert.ToBase64String(Encoding.Unicode.GetBytes(firstAccount.Password + "Password"));
                    sb.AppendLine("            <AutoLogon>");
                    sb.AppendLine("                <Enabled>true</Enabled>");
                    sb.AppendLine($"                <Username>{firstAccount.Username}</Username>");
                    sb.AppendLine("                <Password>");
                    sb.AppendLine($"                    <Value>{passB64}</Value>");
                    sb.AppendLine("                    <PlainText>false</PlainText>");
                    sb.AppendLine("                </Password>");
                    sb.AppendLine("            </AutoLogon>");
                }
            }

            bool needsFirstLogonCommands = config.RemoveBloatware || config.ShowFileExtensions ||
                config.SetNLADelayedStart || config.ConfigureHighPerformancePower || config.EnableBitLocker ||
                config.InstallSoftware ||
                (config.EnableDriverInjection && config.DriverInjectionMethod == "pnputil");

            if (needsFirstLogonCommands)
            {
                sb.AppendLine("            <FirstLogonCommands>");
                int order = 1;

                // Driver injection using pnputil method (runs first to ensure drivers are available)
                if (config.EnableDriverInjection && config.DriverInjectionMethod == "pnputil")
                {
                    GenerateDriverInstallCommands(sb, config, ref order);
                }

                if (config.InstallSoftware)
                {
                    GenerateSoftwareInstallCommands(sb, config, ref order);
                }

                if (config.ShowFileExtensions)
                {
                    sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
                    sb.AppendLine($"                    <Order>{order++}</Order>");
                    sb.AppendLine("                    <CommandLine>reg add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced\" /v HideFileExt /t REG_DWORD /d 0 /f</CommandLine>");
                    sb.AppendLine("                </SynchronousCommand>");
                }

                if (config.SetNLADelayedStart)
                {
                    sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
                    sb.AppendLine($"                    <Order>{order++}</Order>");
                    sb.AppendLine("                    <CommandLine>sc config NlaSvc start=delayed-auto</CommandLine>");
                    sb.AppendLine("                </SynchronousCommand>");
                }

                if (config.ConfigureHighPerformancePower)
                {
                    GeneratePowerSettings(sb, ref order);
                }

                if (config.RemoveBloatware)
                {
                    GenerateBloatwareRemoval(sb, ref order);
                }

                if (config.EnableBitLocker)
                {
                    GenerateBitLockerSettings(sb, config, ref order);
                }

                sb.AppendLine("            </FirstLogonCommands>");
            }

            sb.AppendLine("        </component>");
            sb.AppendLine("    </settings>");
        }

        private static void GeneratePowerSettings(StringBuilder sb, ref int order)
        {
            sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
            sb.AppendLine($"                    <Order>{order++}</Order>");
            sb.AppendLine("                    <CommandLine>powercfg /setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c</CommandLine>");
            sb.AppendLine("                    <Description>Set High Performance power plan</Description>");
            sb.AppendLine("                </SynchronousCommand>");

            sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
            sb.AppendLine($"                    <Order>{order++}</Order>");
            sb.AppendLine("                    <CommandLine>powercfg /change disk-timeout-ac 0</CommandLine>");
            sb.AppendLine("                </SynchronousCommand>");

            sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
            sb.AppendLine($"                    <Order>{order++}</Order>");
            sb.AppendLine("                    <CommandLine>powercfg /change standby-timeout-ac 0</CommandLine>");
            sb.AppendLine("                </SynchronousCommand>");

            sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
            sb.AppendLine($"                    <Order>{order++}</Order>");
            sb.AppendLine("                    <CommandLine>powercfg /change monitor-timeout-ac 45</CommandLine>");
            sb.AppendLine("                </SynchronousCommand>");
        }

        private static void GenerateBloatwareRemoval(StringBuilder sb, ref int order)
        {
            string[] bloatwareApps = new[]
            {
                "Microsoft.BingNews", "Microsoft.BingWeather", "Microsoft.GamingApp",
                "Microsoft.GetHelp", "Microsoft.Getstarted", "Microsoft.MicrosoftOfficeHub",
                "Microsoft.MicrosoftSolitaireCollection", "Microsoft.People", "Microsoft.Todos",
                "Microsoft.WindowsFeedbackHub", "Microsoft.XboxApp", "Microsoft.XboxGameOverlay",
                "Microsoft.XboxGamingOverlay", "Microsoft.XboxIdentityProvider",
                "Microsoft.XboxSpeechToTextOverlay", "Microsoft.ZuneMusic", "Microsoft.ZuneVideo",
                "MicrosoftTeams", "Clipchamp.Clipchamp", "*OneDrive*"
            };

            foreach (var app in bloatwareApps)
            {
                sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
                sb.AppendLine($"                    <Order>{order++}</Order>");
                sb.AppendLine($"                    <CommandLine>powershell -Command \"Get-AppxPackage -Name '{app}' | Remove-AppxPackage -ErrorAction SilentlyContinue\"</CommandLine>");
                sb.AppendLine("                </SynchronousCommand>");
            }
        }

        private static void GenerateBitLockerSettings(StringBuilder sb, UnattendConfig config, ref int order)
        {
            if (config.BitLockerRecoveryOption == "FilePath")
            {
                sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
                sb.AppendLine($"                    <Order>{order++}</Order>");
                sb.AppendLine($"                    <CommandLine>powershell -Command \"New-Item -ItemType Directory -Force -Path '{config.BitLockerRecoveryPath}'\"</CommandLine>");
                sb.AppendLine("                </SynchronousCommand>");
            }

            if (config.BitLockerProtectionMethod == "PasswordOnly")
            {
                sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
                sb.AppendLine($"                    <Order>{order++}</Order>");
                sb.AppendLine("                    <CommandLine>reg add \"HKLM\\SOFTWARE\\Policies\\Microsoft\\FVE\" /v EnableBDEWithNoTPM /t REG_DWORD /d 1 /f</CommandLine>");
                sb.AppendLine("                </SynchronousCommand>");
            }

            string bitlockerScript = config.BitLockerProtectionMethod switch
            {
                "TPMOnly" => $"Enable-BitLocker -MountPoint '{config.BitLockerDriveLetter}' -EncryptionMethod XtsAes256 -TpmProtector -SkipHardwareTest",
                "TPMAndPIN" when !string.IsNullOrEmpty(config.BitLockerPIN) =>
                    $"$pin = ConvertTo-SecureString '{config.BitLockerPIN}' -AsPlainText -Force; Enable-BitLocker -MountPoint '{config.BitLockerDriveLetter}' -EncryptionMethod XtsAes256 -TpmAndPinProtector -Pin $pin -SkipHardwareTest",
                "TPMAndPIN" =>
                    $"$pin = Read-Host -AsSecureString -Prompt 'Enter BitLocker PIN'; Enable-BitLocker -MountPoint '{config.BitLockerDriveLetter}' -EncryptionMethod XtsAes256 -TpmAndPinProtector -Pin $pin -SkipHardwareTest",
                _ => $"$pw = Read-Host -AsSecureString -Prompt 'Enter BitLocker Password'; Enable-BitLocker -MountPoint '{config.BitLockerDriveLetter}' -EncryptionMethod XtsAes256 -PasswordProtector -Password $pw -SkipHardwareTest"
            };

            sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
            sb.AppendLine($"                    <Order>{order++}</Order>");
            sb.AppendLine($"                    <CommandLine>powershell -ExecutionPolicy Bypass -Command \"{bitlockerScript}\"</CommandLine>");
            sb.AppendLine("                </SynchronousCommand>");
        }

        private static void GenerateDriverInstallCommands(StringBuilder sb, UnattendConfig config, ref int order)
        {
            // First, copy drivers from source (USB) to target system
            sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
            sb.AppendLine($"                    <Order>{order++}</Order>");
            sb.AppendLine($"                    <CommandLine>xcopy \"{config.DriverSourcePath}\" \"{config.DriverTargetPath}\" /E /I /H /Y</CommandLine>");
            sb.AppendLine("                    <Description>Copy drivers from installation media to local drive</Description>");
            sb.AppendLine("                </SynchronousCommand>");

            // Then install all drivers using pnputil
            sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
            sb.AppendLine($"                    <Order>{order++}</Order>");
            sb.AppendLine($"                    <CommandLine>pnputil /add-driver \"{config.DriverTargetPath}\\*.inf\" /subdirs /install</CommandLine>");
            sb.AppendLine("                    <Description>Install drivers using pnputil</Description>");
            sb.AppendLine("                </SynchronousCommand>");
        }

        private static void GenerateSoftwareInstallCommands(StringBuilder sb, UnattendConfig config, ref int order)
        {
            string script = $"$path = '{config.SoftwareSourcePath}'; " +
                "if (Test-Path $path) { " +
                "$files = Get-ChildItem -Path $path -File | Where-Object { $_.Extension -match '\\.(exe|msi|bat|cmd|ps1)$' }; " +
                "foreach ($file in $files) { " +
                "  Write-Host 'Installing ' $file.Name; " +
                "  if ($file.Extension -eq '.msi') { " +
                "    Start-Process msiexec.exe -ArgumentList '/i \"' + $file.FullName + '\" /qn /norestart' -Wait; " +
                "  } elseif ($file.Extension -eq '.ps1') { " +
                "    Start-Process powershell.exe -ArgumentList '-ExecutionPolicy Bypass -File \"' + $file.FullName + '\"' -Wait; " +
                "  } else { " +
                "    Start-Process $file.FullName -Wait; " +
                "  } " +
                "} " +
                "}";

            sb.AppendLine($"                <SynchronousCommand wcm:action=\"add\">");
            sb.AppendLine($"                    <Order>{order++}</Order>");
            sb.AppendLine($"                    <CommandLine>powershell -ExecutionPolicy Bypass -Command \"{script}\"</CommandLine>");
            sb.AppendLine("                    <Description>Auto-install software from USB</Description>");
            sb.AppendLine("                </SynchronousCommand>");
        }
    }
}
