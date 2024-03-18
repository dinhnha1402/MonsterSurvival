using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    public MongoDBConnector MongoController; // Giả sử bạn có thể truy cập RealmController từ đây

    public void OnLoginButtonClicked()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        MongoController.CheckLogin(username, password);
        //MongoController.AddOrUpdateUserScore(username, 100);

    }

    public void OnRegisterButtonClicked()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        MongoController.RegisterUser(username, password);

    }
}

