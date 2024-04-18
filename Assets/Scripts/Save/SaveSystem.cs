using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public bool SelectedLoadGame = false;

    public void SaveUsername(string username)
    {
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.Save(); // Đảm bảo rằng dữ liệu được lưu ngay lập tức
    }


    public void SaveHighscore(string highscore)
    {
        PlayerPrefs.SetString("Highscore", highscore);
        PlayerPrefs.Save();
    }

    public string GetUsername()
    {
        return PlayerPrefs.GetString("Username");
    }

    public void GetHighscore()
    {
        PlayerPrefs.GetString("Highscore");
    }
    public void SaveGame(string jsonGameSaveData)
    {
        PlayerPrefs.SetString("SaveGameInfo", jsonGameSaveData);
        PlayerPrefs.Save(); // Đảm bảo rằng dữ liệu được lưu ngay lập tức
        SelectedLoadGame = true;
    }

    // Lấy chuỗi JSON từ PlayerPrefs
    public string GetSavedGame()
    {
        return PlayerPrefs.GetString("SaveGameInfo", "{}");
    }
}
