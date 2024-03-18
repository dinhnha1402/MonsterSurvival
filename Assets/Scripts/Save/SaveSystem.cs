using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public void SaveUsername(string username)
    {
        PlayerPrefs.SetString("Username", username);
    }

    public void SaveHighscore(string highscore)
    {
        PlayerPrefs.SetString("Highscore", highscore);
    }

    public void GetUsername()
    {
        PlayerPrefs.GetString("Username");
    }

    public void GetHighscore()
    {
        PlayerPrefs.GetString("Highscore");
    }
}
