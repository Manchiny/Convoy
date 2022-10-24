using System;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
    public class WindowsHolder
    {
        public static readonly Dictionary<Type, string> Windows = new Dictionary<Type, string>
        {
            [typeof(UpgradeTankWindow)] = "Windows/UpgradeTankWindow",
            //[typeof(LevelCompleteWindow)] = "Windows/LevelCompleteWindow",
            //[typeof(SettingsWindow)] = "Windows/SettingsWindow",
            //[typeof(LeaderboardWindow)] = "Windows/LeaderboardWindow"
        };
    }
}

