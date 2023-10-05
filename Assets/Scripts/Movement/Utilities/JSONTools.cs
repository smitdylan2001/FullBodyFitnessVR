using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

namespace DevDunk.Movement.Utilities
{
    public static class JSONTools
    {
        public static void SaveIntoJson(BodyTrackerResult data)
        {
            string potion = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/PotionData.json", potion);
        }
        public static BodyTrackerResult ReadFromJson(string data)
        {
            return JsonUtility.FromJson<BodyTrackerResult>(data);
        }
        public static BodyTrackerResult ReadFromJsonPath(string path)
        {
            var data = System.IO.File.ReadAllText(path);
            return ReadFromJson(data);
        }
        public static BodyTrackerResult ReadFromJsonPersistentPath()
        {
            var path = Application.persistentDataPath + "/PotionData.json";
            var data = System.IO.File.ReadAllText(path);
            return ReadFromJson(data);
        }

        public static void V3SaveIntoJson(Vector3 data)
        {
            string potion = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/V3PotionData.json", potion);
        }
        public static Vector3 V3ReadFromJson(string data)
        {
            return JsonUtility.FromJson<Vector3>(data);
        }
        public static Vector3 V3ReadFromJsonPath(string path)
        {
            var data = System.IO.File.ReadAllText(path);
            return V3ReadFromJson(data);
        }
        public static Vector3 V3ReadFromJsonPersistentPath()
        {
            var path = Application.persistentDataPath + "/V3PotionData.json";
            var data = System.IO.File.ReadAllText(path);
            return V3ReadFromJson(data);
        }
    }
}