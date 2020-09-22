using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    private bool pause = false;
    [SerializeField] private GameObject appMenu;

    private void OnApplicationPause(bool _pause)
    {
        if (pause == true && _pause == false)
        {
            pause = false;
            appMenu.SetActive(true);
        }

        if (_pause == true)
            pause = true;
    }

    public void AppReset()
    {
        SceneManager.LoadScene(0);
    }
}
