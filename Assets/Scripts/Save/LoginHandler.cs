using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private MongoDBConnector mongoController;

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
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

