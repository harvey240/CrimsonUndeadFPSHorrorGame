using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("TestLevel1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectHUDConfiguration(int hudInt)
    {
        // hudInt = hudInt + 1;
        // HUDManager.instance.SetHUDVersion(hudInt);
        switch(hudInt)
        {
            case 0:
                HUDManager.hudType = HUDManager.HUDType.Numerical;
                Debug.Log("Numerical");
                break;
            case 1:
                HUDManager.hudType = HUDManager.HUDType.Discrete;
                Debug.Log("Discrete");
                break;                
            case 2:
                HUDManager.hudType = HUDManager.HUDType.Bar;
                Debug.Log("Bar");
                break;
            case 3:
                HUDManager.hudType = HUDManager.HUDType.Hue;
                Debug.Log("Hue");
                break;
            case 4:
                HUDManager.hudType = HUDManager.HUDType.None;
                Debug.Log("None");
                break;
        }
    }
}
