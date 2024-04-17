using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLoadGame : MonoBehaviour
{
    public Transform container; // Assign this in the inspector to your container GameObject

    public void OnButtonPress()
    {
        foreach (Transform child in container)
        {
            SaveSystem saveSystem = child.GetComponent<SaveSystem>();
            if (saveSystem != null && saveSystem.SelectedLoadGame)
            {
                Debug.Log("Selected game found in: " + child.name);
                // Perform additional operations as needed
            }
        }
    }
}
