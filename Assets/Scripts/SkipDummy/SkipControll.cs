using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkipControll : MonoBehaviour
{
    public UnityEvent skip;
    // Update is called once per frame
    void Update()
    {
        var gesture = WaveVR_Controller.Input(WaveVR_Controller.EDeviceType.Dominant).GetPressDown(wvr.WVR_InputId.WVR_InputId_Alias1_Touchpad);

        if(gesture == true)
        {
            skip?.Invoke();
        }
    }
}
