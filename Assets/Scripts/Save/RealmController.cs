using System.Collections.Generic;
using System.Threading.Tasks;
using Realms;
using Realms.Sync;
using UnityEngine;
using Realms.Logging;
using Logger = Realms.Logging.Logger;
using Realms.Sync.ErrorHandling;
using Realms.Sync.Exceptions;
using System.Linq;
using System;
using Assets.Scripts.Save;
using System.Security.Cryptography;
using System.Text;

public class RealmController : MonoBehaviour
{
    private Realm realm;
    private readonly string myRealmAppId = "application-0-wqtsj";
    private readonly string apiKey = "AyXs9i2NiviS1uN5xTvKePQAhtECJFYxAMgTCx7TtP19snL1dbUiIwGogIzIh9YD";

    public RealmController()
    {
        InitAsync();
    }

    private async void InitAsync()
    {
        var app = App.Create(new AppConfiguration(myRealmAppId)
        {
            MetadataPersistenceMode = MetadataPersistenceMode.NotEncrypted
        });

        User user = await Get_userAsync(app);
        if (user == null) return; // Thêm kiểm tra nếu không thể đăng nhập

        var config = new FlexibleSyncConfiguration(user);

        realm = await Realm.GetInstanceAsync(config);

        // Cần cấu hình subscriptions cho Flexible Sync nếu cần
        // Ví dụ:
        // realm.Subscriptions.Update(() =>
        // {
        //     realm.Subscriptions.Add(realm.All<UserAccount>());
        // });

        // Lưu ý: Đảm bảo đã đăng ký model trong MongoDB Atlas
    }

    private async Task<User> Get_userAsync(App app)
    {
        User user = app.CurrentUser;
        if (user == null && !string.IsNullOrEmpty(apiKey))
        {
            user = await app.LogInAsync(Credentials.ApiKey(apiKey));
        }
        return user;
    }

    public void AddHighscore(string playerName, int score)
    {
        Highscore currentHighscore = null;
        if (realm == null)
        {
            Debug.Log("Realm not ready");
            return;
        }
        try
        {
            currentHighscore = realm.All<Highscore>().Where(highscore => highscore.Player == playerName).First();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        realm.Write(() =>
        {
            if (currentHighscore == null)
            {
                realm.Add(new Highscore()
                {
                    Player = playerName,
                    Score = score
                });
            }
            else
            {
                if (currentHighscore.Score < score)
                {
                    currentHighscore.Score = score;
                }
            }
        });
    }

    private string HashPassword(string password)
    {
        // Sử dụng SHA256 hoặc một phương pháp mã hóa mạnh mẽ hơn
        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    public async Task<bool> IsValidLoginAsync(string username, string password)
    {
        if (realm == null)
        {
            Debug.Log("Realm not ready");
            return false;
        }

        // Mã hóa password trước khi so sánh, giả sử sử dụng phương pháp mã hóa giống khi lưu password
        var hashedPassword = HashPassword(password); // Bạn cần cung cấp phương thức HashPassword

        try
        {
            var userAccount = realm.All<UserAccount>().FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);
            return userAccount != null;
        }
        catch (Exception e)
        {
            Debug.Log($"Error during login: {e.Message}");
            return false;
        }
    }

    public async Task<bool> RegisterUserAsync(string username, string password)
    {
        if (realm == null)
        {
            Debug.Log("Realm not ready");
            return false;
        }

        var userExists = realm.All<UserAccount>().Any(u => u.Username == username);
        if (userExists)
        {
            Debug.Log($"User {username} already exists.");
            return false; // Người dùng đã tồn tại
        }

        // Mã hóa mật khẩu trước khi lưu
        var hashedPassword = HashPassword(password);

        try
        {
            realm.Write(() =>
            {
                var newUser = new UserAccount
                {
                    Username = username,
                    Password = hashedPassword
                };
                realm.Add(newUser);
            });
            return true; // Tạo mới người dùng thành công
        }
        catch (Exception e)
        {
            Debug.Log($"Error during user registration: {e.Message}");
            return false; // Đã xảy ra lỗi khi tạo mới người dùng
        }
    }

}

