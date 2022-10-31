using System;
using System.Collections.Generic;

namespace Assets.Scripts.Localization
{
    [Serializable]
    public class LocalizationsData
    {
        public float Version;
        public List<LocalizationKey> LocalizationKeys;
    }
}
