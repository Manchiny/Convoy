using System;
using UnityEngine;

namespace Assets.Scripts.Saves
{
    public abstract class Saver
    {
        public abstract string Tag { get; }

        public abstract void RemoveAllData();

        public void Save(UserData data)
        {
            WriteData(data);
            Debug.Log(Tag + ": save progress.");
        }

        public void LoadUserData(Action<UserData> onComplete)
        { 
            Debug.Log(Tag + ": try load progress...");
            LoadData(OnLoaded);

            void OnLoaded(UserData data)
            {
                ValidateData(data);
                Debug.Log(Tag + ": progress loaded;");

                onComplete?.Invoke(data);
            }
        }

        protected abstract void WriteData(UserData data);
        protected abstract void LoadData(Action<UserData> onComplete);

        private void ValidateData(UserData data)
        {
            if (data.TankData == null)
                data.TankData = new UnitData();

            if (data.PlayerCharacterData == null)
                data.PlayerCharacterData = new UnitData();
        }
    }
}
