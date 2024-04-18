using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MongoDBConnector;

public class LoginHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private MongoDBConnector mongoController;
    [SerializeField] private Transform container;

    public void OnLoginButtonClicked()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        mongoController.CheckLogin(username, password);


        //MongoController.AddOrUpdateUserScore(username, 100);

    }

    public void OnRegisterButtonClicked()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        mongoController.RegisterUser(username, password);

    }

    public void OnNewGameButtonClicked()
    {
        SaveSystem saveSystem = new SaveSystem();

        string username = saveSystem.GetUsername();

        SaveInfo data = new SaveInfo { username = username };

        string jsonGameSaveData = JsonUtility.ToJson(data);

        saveSystem.SaveGame(jsonGameSaveData);

        //mongoController.SaveGameInfo(data);

        SceneManager.LoadScene("Main");
    }

    public void OnLoadGameButtonClick()
    {
        if (container == null)
        {
            Debug.LogError("Container not assigned in the Inspector on " + gameObject.name);
            return;  // Exit the function if container is not assigned
        }
        // Iterate through each child transform in the container
        foreach (Transform child in container)
        {
            // Attempt to get the SaveSystem component
            SaveSystem saveSystem = child.GetComponent<SaveSystem>();
            if (saveSystem != null && saveSystem.SelectedLoadGame)
            {
                // Load the scene if SelectedLoadGame is true
                SceneManager.LoadScene("Main");

                // Break the loop after finding the first selected game
                break;
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

