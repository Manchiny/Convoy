using System;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
    public class WindowsHolder
    {
        public static readonly Dictionary<Type, string> Windows = new Dictionary<Type, string>
        {
            [typeof(UpgradeTankWindow)] = "Windows/UpgradeTankWindow",
            [typeof(LevelCompleteWindow)] = "Windows/LevelCompleteWindow",
            [typeof(StartLevelWindow)] = "Windows/StartLevelWindow",
            [typeof(UpgradePlayerWindow)] = "Windows/UpgradePlayerWindow",
            [typeof(ShopWindow)] = "Windows/ShopWindow",
        };
    }
}

