namespace Windows_Key_Builder.Web.Models
{
    public class UserAccount
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsAdmin { get; set; } = true;
        public bool IsEnabled { get; set; } = false;
    }

    public class UnattendConfig
    {
        // Basic Settings
        public string Language { get; set; } = "en-US";
        public string Keyboard { get; set; } = "0409:00000409";
        public string TimeZone { get; set; } = "Eastern Standard Time";
        public string ComputerName { get; set; } = "*";
        public bool UseInteractiveComputerName { get; set; }

        // User Accounts
        public List<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();
        public bool AutoLogin { get; set; }

        // Disk Partitioning
        public string PartitionStyle { get; set; } = "GPT";
        public bool AutoPartition { get; set; } = true;
        public int EfiPartitionSize { get; set; } = 300;
        public int RecoveryPartitionSize { get; set; } = 1000;

        // Windows Edition
        public string Edition { get; set; } = "Pro";
        public string ProductKey { get; set; } = "";
        public bool UseKMSKey { get; set; }

        // System Tweaks
        public bool BypassTPM { get; set; }
        public bool SetUACNeverNotify { get; set; }
        public bool ShowFileExtensions { get; set; } = true;
        public bool EnableRDP { get; set; }
        public bool RemoveBloatware { get; set; }
        public bool SetNLADelayedStart { get; set; } = true;

        // Software Installation
        public bool InstallSoftware { get; set; }
        public string SoftwareSourcePath { get; set; } = "D:\\Software";

        // Power Settings
        public bool ConfigureHighPerformancePower { get; set; }

        // BitLocker Configuration
        public bool EnableBitLocker { get; set; }
        public string BitLockerProtectionMethod { get; set; } = "TPMOnly";
        public string BitLockerRecoveryOption { get; set; } = "FilePath";
        public string BitLockerRecoveryPath { get; set; } = "C:\\BitLockerRecovery";
        public string BitLockerDriveLetter { get; set; } = "C:";
        public string BitLockerPIN { get; set; } = "";

        // Driver Injection
        public bool EnableDriverInjection { get; set; }
        public string DriverInjectionMethod { get; set; } = "DriverPaths";
        public string DriverSourcePath { get; set; } = "D:\\Drivers";
        public string DriverTargetPath { get; set; } = "C:\\Drivers";
    }
}
