using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Localization
{
    [CreateAssetMenu]
    public class LocalizationDatabase : ScriptableObject
    {
        [SerializeField] private float _version = 0;
        [SerializeField] private List<LocalizationKey> _localizationKeys;

        private List<LocalizationKey> _actualLicalizationKeys;

        public IReadOnlyList<LocalizationKey> LocalizationKeys => _actualLicalizationKeys;
        public float Version => _version;

        public void InitData(LocalizationsData data)
        {
            if (data == null || data.LocalizationKeys.Count == 0)
            {
                _actualLicalizationKeys = _localizationKeys;
                Debug.Log("Localization keys loaded defualt;");
            }
            else
            {
                _actualLicalizationKeys = data.LocalizationKeys.ToList();
                Debug.Log($"Localization keys updated by server; Count: {_actualLicalizationKeys.Count}; Version: {data.Version}");
            }
        }

#if UNITY_EDITOR
        public IReadOnlyList<LocalizationKey> GetKeysData() => _localizationKeys;
#endif
    }

    [Serializable]
    public class LocalizationKey
    {
        [SerializeField] private string _key;
        [SerializeField] private List<LocalizationValue> _values;

        public string Key => _key;
        public IReadOnlyList<LocalizationValue> Values => _values;
    }

    [Serializable]
    public class LocalizationValue
    {
        [SerializeField] private SystemLanguage _language;
        [SerializeField] private string _value;

        public SystemLanguage Language => _language;
        public string Value => _value;
    }
}

