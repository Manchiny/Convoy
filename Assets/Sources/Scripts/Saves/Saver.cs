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
            Debug.Log(Tag + ": load progress.");
            LoadData(onComplete);
        }

        protected abstract void WriteData(UserData data);
        protected abstract void LoadData(Action<UserData> onComplete);
    }
}
