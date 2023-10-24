using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Toggle aiToggle;
    
    // Start is called before the first frame update
    void Start()
    {
        int flag = PlayerPrefs.GetInt("AI", 1);
        if (flag == 0)
        {
            aiToggle.isOn = false;
        }
        else
        {
            aiToggle.isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void aiToggleChange(bool flag)
    {
        Debug.Log("aiToggleChange "+aiToggle.isOn);
        if (aiToggle.isOn)
        {
            PlayerPrefs.SetInt("AI", 1);    
        }
        else
        {
            PlayerPrefs.SetInt("AI", 0);
        }
        
    }
}
