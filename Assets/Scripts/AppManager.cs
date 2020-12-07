using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    private bool pause = false;
    [SerializeField] private GameObject resetMenu;

    private void OnApplicationPause(bool _pause)
    {
        if (pause && !_pause)
        {
            pause = false;

            // quickFix
            if (!GameObject.Find("Tutorial.Manager"))
                resetMenu.SetActive(true);
        }

        if (_pause)
        {
            pause = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnApplicationPause(true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnApplicationPause(false);
        }
    }

    public void AppReset()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        GameManager.Instance.EnableTutorial = false;
    }
}
