using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepController : MonoBehaviour
{
    private StepsManager stepsManager;

    private void Start()
    {
        stepsManager = StepsManager.Instance;
    }

    void Update()
    {
        var pressed = WaveVR_Controller.Input(WaveVR_Controller.EDeviceType.Dominant).GetPressDown(wvr.WVR_InputId.WVR_InputId_Alias1_Trigger);

        if (pressed)
        {
            stepsManager.NextStep();
        }

        var reset = WaveVR_Controller.Input(WaveVR_Controller.EDeviceType.Dominant).GetPressDown(wvr.WVR_InputId.WVR_InputId_Alias1_Touchpad);

        if (reset)
        {
            SceneManager.LoadScene(0);
        }
    }
}
