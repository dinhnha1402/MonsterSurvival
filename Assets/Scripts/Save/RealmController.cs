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
}

