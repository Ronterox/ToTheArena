using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveScore(int highScore, int wave, int kills)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.scores";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData playerScore = new PlayerData(highScore, wave, kills);

        formatter.Serialize(stream, playerScore);

        stream.Close();
    }

    public static PlayerData LoadScore()
    {
        string path = Application.persistentDataPath + "/player.scores";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            return data;
        }

        Debug.LogWarning("Couldn't find any saved files");
        return null;
    }
}
