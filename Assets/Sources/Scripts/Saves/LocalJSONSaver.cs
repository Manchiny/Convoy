using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Saves
{
    public class LocalJSONSaver : Saver
    {
        private readonly string SavePath = Path.Combine(Application.persistentDataPath, "Save.json");
        public override string Tag => "[LocalSaver]";

        private bool _saving;

        public override void RemoveAllData()
        {
            Save(new UserData());
        }

        protected override void LoadData(Action<UserData> onComplete)
        {
            UserData data = new UserData();
            data.NeedSound = true;

            if (File.Exists(SavePath))
                data = JsonUtility.FromJson<UserData>(File.ReadAllText(SavePath));

            onComplete?.Invoke(data);
        }

        protected override void WriteData(UserData data)
        {
            if (_saving)
                return;

            string toJson = JsonUtility.ToJson(data);

            File.CreateText(SavePath).Dispose();

            using (TextWriter writer = new StreamWriter(SavePath, false))
            {
                writer.WriteLine(toJson);
                    writer.Close();
            }
        }
    }
}

