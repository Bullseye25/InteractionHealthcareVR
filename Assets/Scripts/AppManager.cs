using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    private bool pause = false;

    private void OnApplicationPause(bool _pause)
    {
        if(pause == true && _pause == false)
        {
            pause = false;
            SceneManager.LoadScene(0);
        }

        if (_pause == true)
            pause = true;
    }
}
