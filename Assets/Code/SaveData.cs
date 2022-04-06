using System;
using System.IO;
using UnityEngine;

namespace cards
{
    public static class SaveData
    {
        private static readonly string _fileName = "SaveData.dat";

        private static string _path;

        static SaveData()
        {
            _path = Application.persistentDataPath + "/" + _fileName;
        }

        public static void Save(SaveDataItem item)
        {
            var str = JsonUtility.ToJson(item);
            File.WriteAllText(_path, str);

        }

        public static SaveDataItem Load()
        {
            try
            {
                var str = File.ReadAllText(_path);
                return JsonUtility.FromJson<SaveDataItem>(str);
            }
            catch
            {
                return new SaveDataItem();
            }
        }
    }

    [Serializable]
    public struct SaveDataItem
    {
        public int levelRecord;
        public int lastRecord;
        public SaveDataItem(int levels = 0, int last = 0)
        {
            levelRecord = levels;
            lastRecord = last;
        }
    }
}
