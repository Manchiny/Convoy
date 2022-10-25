using Agava.YandexGames;
using System;
using UnityEngine;

namespace Assets.Scripts.Saves
{
    public class YandexSaver : Saver
    {
        public override string Tag => "[YandexSaver]";

        public override void RemoveAllData()
        {    
            Save(new UserData());
        }

        protected override void LoadData(Action<UserData> onComplete)
        {
            PlayerAccount.GetPlayerData(OnSuccess, OnError);

            void OnSuccess(string data)
            {
                string dataString = data;
                UserData userData = JsonUtility.FromJson<UserData>(dataString);

                onComplete?.Invoke(userData);
            }

            void OnError(string error)
            {
                Debug.LogError(Tag + " get player data error: " + error);
                UserData userData = new UserData();

                onComplete?.Invoke(userData);
            }
        }

        protected override void WriteData(UserData data)
        {
            string dataToJson = JsonUtility.ToJson(data);
            PlayerAccount.SetPlayerData(dataToJson);
        }
    }
}
