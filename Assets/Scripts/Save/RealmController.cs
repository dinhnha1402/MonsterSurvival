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

public class RealmController
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
        var app = App.Create(myRealmAppId);
        User user = await Get_userAsync(app);
        PartitionSyncConfiguration config = GetConfig(user);
        realm = await Realm.GetInstanceAsync(config);
    }

    private async Task<User> Get_userAsync(App app)
    {
        User user = app.CurrentUser;
        if (user == null)
        {
            user = await app.LogInAsync(Credentials.ApiKey(apiKey));
        }
        return user;
    }

    private PartitionSyncConfiguration GetConfig(User user)
    {
        PartitionSyncConfiguration config = new("MonsterSurvival", user);

        config.ClientResetHandler = new DiscardUnsyncedChangesHandler()
        {
            ManualResetFallback = (ClientResetException clientResetException) => clientResetException.InitiateClientReset()
        };
        return config;
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

