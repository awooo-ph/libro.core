using System;
using System.Runtime.CompilerServices;

namespace Libro
{
    [Serializable]
    public class Config:ConfigBase<Config>
    {

        private Config() { }

        private bool _encryptDatabase;

        internal static bool EncryptDatabase
        {
            get { return Instance._encryptDatabase; }
            set { Instance._encryptDatabase = value; }
        }

        private static void ConfigChanged([CallerMemberName] string config = null)
        {
            Messenger.Default.Broadcast(Messages.ConfigChanged,config);
        }

        [NonSerialized]
        private static Config _instance = null;

        public static Config Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new Config();
                Load(ref _instance);
                return _instance;
            }
        }

        private bool _useUsername;

        public static bool UseUsername
        {
            get { return Instance._useUsername; }
            set
            {
                if (value == Instance._useUsername) return;
                Instance._useUsername = value;
                ConfigChanged();
            }
        }
        
        private bool _UseDarkTheme;

        public static bool DarkTheme
        {
            get { return Instance._UseDarkTheme; }
            set
            {
                Instance._UseDarkTheme = value;
                ConfigChanged();
            }
        }

        private string _PrimarySwatch;

        public static string PrimarySwatch
        {
            get { return Instance._PrimarySwatch; }
            set
            {
                Instance._PrimarySwatch = value;
                ConfigChanged();
            }
        }

        private string _AccentSwatch;

        public static string AccentSwatch
        {
            get { return Instance._AccentSwatch; }
            set
            {
                Instance._AccentSwatch = value;
                ConfigChanged();
            }
        }

        private string _takeoutDuration;
        public static string TakoutDuration
        {
            get { return Instance._takeoutDuration; }
            set
            {
                Instance._takeoutDuration = value;
                ConfigChanged();
            }
        }

        private double _penalty;

        public static double Penalty
        {
            get { return Instance._penalty; }
            set
            {
                Instance._penalty = value;
                ConfigChanged();
            }
        }

        public static string PenaltyString
        {
            get { return Instance._penalty.ToString(); }
            set
            {
                Instance._penalty = double.Parse("0"+value);
                ConfigChanged();
            }
        }

        private int _takeoutLimit;

        public static int TakeoutLimit
        {
            get { return Instance._takeoutLimit; }
            set
            {
                Instance._takeoutLimit = value;
                ConfigChanged();
            }
        }

        public static string TakeoutLimitString
        {
            get { return Instance._takeoutLimit.ToString(); }
            set
            {
                Instance._takeoutLimit = int.Parse("0"+value);
                ConfigChanged();
            }
        }

        public static void Save()
        {
            Save(Instance);
        }
        
     
    }
}
