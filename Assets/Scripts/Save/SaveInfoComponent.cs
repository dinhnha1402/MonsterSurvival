using Amazon.Runtime.Documents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInfoComponent : MonoBehaviour
{
    public string saveInfo;
    public SaveSystem saveSystem;
    public MongoDBConnector mongoDBController;

    public void Setup(MongoDBConnector controller)
    {
        mongoDBController = controller;
    }

    public void LoadSelectionClicked()
    {
        saveSystem.SaveGame(saveInfo);
        
    }

}
