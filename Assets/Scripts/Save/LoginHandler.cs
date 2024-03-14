using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginHandler : MonoBehaviour
{
    public TMP_Text usernameInputField;
    public TMP_Text passwordInputField;
    public MongoDBConnector MongoController; // Giả sử bạn có thể truy cập RealmController từ đây

    public void OnLoginButtonClicked()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        MongoController.CheckLogin(username, password);

    }
}

