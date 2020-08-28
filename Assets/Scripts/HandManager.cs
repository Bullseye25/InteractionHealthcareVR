using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wvr;
using WVR_Log;

public class HandManager : MonoBehaviour
{

    private static string LOG_TAG = "ControllerManagerTest";
    [SerializeField] private WaveVR_Controller.EDeviceType eDevice = WaveVR_Controller.EDeviceType.Dominant;
    private WVR_PoseState_t pose;

/*    WaveVR_Beam _beam = null;
    WaveVR_ControllerPointer _pointer = null;*/

    public void SetDeviceIndex(WaveVR_Controller.EDeviceType device)
    {
        Log.i(LOG_TAG, "SetDeviceIndex, _index = " + device);
        this.eDevice = device;
    }

    void Update()
    {
/*        var grab = WaveVR_Controller.Input(WaveVR_Controller.EDeviceType.Dominant).GetPressDown(wvr.WVR_InputId.WVR_InputId_Alias1_Trigger);
*/
        WVR_DeviceType _type = WaveVR_Controller.Input(this.eDevice).DeviceType;

        Interop.WVR_GetPoseState(
            _type,
            WVR_PoseOriginModel.WVR_PoseOriginModel_OriginOnHead,
            500,
            ref pose);

        transform.localPosition = new WaveVR_Utils.RigidTransform(pose.PoseMatrix).pos;
        transform.localRotation = new WaveVR_Utils.RigidTransform(pose.PoseMatrix).rot;

/*        if(_pointer != null)
        {
            _pointer.OnPointerEnter
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "grab")
        {
            other.transform.SetParent(this.transform);
        }
    }

/*    private GameObject dominantController = null, nonDominantController = null;
    void OnEnable()
    {
        WaveVR_Utils.Event.Listen(WaveVR_Utils.Event.CONTROLLER_MODEL_LOADED, OnControllerLoaded);
    }

    void OnControllerLoaded(params object[] args)
    {
        WaveVR_Controller.EDeviceType _type = (WaveVR_Controller.EDeviceType)args[0];
        if (_type == WaveVR_Controller.EDeviceType.Dominant)
        {
            this.dominantController = (GameObject)args[1];
            listControllerObjects(this.dominantController);
        }
        if (_type == WaveVR_Controller.EDeviceType.NonDominant)
        {
            this.nonDominantController = (GameObject)args[1];
            listControllerObjects(this.nonDominantController);
        }
    }

    void OnDisable()
    {
        WaveVR_Utils.Event.Remove(WaveVR_Utils.Event.CONTROLLER_MODEL_LOADED, OnControllerLoaded);
    }

    private void listControllerObjects(GameObject ctrlr)
    {
        if (ctrlr == null)
            return;

        // Get all children.
        GameObject[] _objects = new GameObject[ctrlr.transform.childCount];
        for (int i = 0; i < ctrlr.transform.childCount; i++)
            _objects[i] = ctrlr.transform.GetChild(i).gameObject;

        // Find beam.
        for (int i = 0; i < _objects.Length; i++)
        {
            _beam = _objects[i].GetComponentInChildren<WaveVR_Beam>();
            if (_beam != null)
                break;
        }
        if (_beam != null)
            Debug.Log("Find beam: " + _beam.name);

        // Find pointer.
        for (int i = 0; i < _objects.Length; i++)
        {
            _pointer = _objects[i].GetComponentInChildren<WaveVR_ControllerPointer>();
            if (_pointer != null)
                break;
        }
        if (_pointer != null)
            Debug.Log("Find pointer: " + _pointer.name);
    }*/
}