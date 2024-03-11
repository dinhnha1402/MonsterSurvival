using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginHandler : MonoBehaviour
{
    public TMP_Text usernameInputField;
    public TMP_Text passwordInputField;
    public RealmController realmController; // Giả sử bạn có thể truy cập RealmController từ đây

    public async void OnLoginButtonClicked()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        // Gọi hàm kiểm tra đăng nhập từ RealmController
        bool isValidUser = await realmController.IsValidLoginAsync(username, password);

        if (isValidUser)
        {
            Debug.Log("Đăng nhập thành công.");
            // Xử lý đăng nhập thành công (chuyển scene, hiển thị thông tin người dùng, v.v.)
        }
        else
        {
            Debug.Log("Đăng nhập thất bại.");
            // Xử lý đăng nhập thất bại (hiển thị thông báo lỗi, v.v.)
        }
    }
}

