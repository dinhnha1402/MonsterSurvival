using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsMenuManager : MonoBehaviour
{

    public TMP_Dropdown graphicsDropdown;


    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
