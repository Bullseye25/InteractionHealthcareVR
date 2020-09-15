using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wvr;
using WVR_Log;
using DG.Tweening;
using wvr.TypeExtensions;
using System.Linq;

public class HandManager : MonoBehaviour
{
    #region Private Variables
    private const string 
        LOG_TAG = "ControllerManagerTest", 
        INTERACTABLE_TAG = "Interactable";

    [SerializeField] private WaveVR_Controller.EDeviceType eDevice; // = WaveVR_Controller.EDeviceType.Dominant;

    private WVR_PoseState_t pose;

    [SerializeField] private Interactable[] interactablesCollection;
    private WaveVR_RenderModel.ControllerHand hand;

    //[SerializeField] private WaveVR_Beam _beam = null;
    //[SerializeField] private WaveVR_ControllerPointer _pointer = null;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        hand = eDevice == WaveVR_Controller.EDeviceType.Dominant ? WaveVR_RenderModel.ControllerHand.Controller_Dominant : WaveVR_RenderModel.ControllerHand.Controller_NonDominant;
    }

    public void OnEnable()
    {
        interactablesCollection = FindObjectsOfType<Interactable>();
    }

    private void Update()
    {
        Grab();

        Release();

        PositionTracker();
        
        /*
                 if(_pointer != null)
                {
                    _pointer.OnPointerEnter
                }
        */
    }

    private void OnTriggerEnter(Collider value)
    {
        if (value.tag == INTERACTABLE_TAG && transform.childCount == 0)
        {
            value.transform.SetParent(this.transform);
            value.transform.localPosition = Vector3.zero;
            value.transform.localRotation = Quaternion.Euler(0, 0, 0);
            ControllerAppearance(false);
            value.GetComponent<Interactable>().OnGrab?.Invoke();
        }
    }

    #endregion

    #region Helping Functions

    public void SetDeviceIndex(WaveVR_Controller.EDeviceType device)
    {
        Log.i(LOG_TAG, "SetDeviceIndex, _index = " + device);
        this.eDevice = device;
    }

    private void Grab()
    {
        if (eDevice != WaveVR_Controller.EDeviceType.Dominant)
            return;

        var grabGesture = WaveVR_Controller.Input(WaveVR_Controller.EDeviceType.Dominant).GetPressDown(wvr.WVR_InputId.WVR_InputId_Alias1_Trigger);

        //var grabGesture = Input.GetKeyDown(KeyCode.Space);

        if (grabGesture == true)
        {
            if (transform.childCount == 0)
            {
                GetInteractable();
            }
            else
            {
                //TODO: method that would play ther recorder if the recorder in the hand of player
            }
        }
    }

    private void Release()
    {
        var releaseGesture = WaveVR_Controller.Input(WaveVR_Controller.EDeviceType.Dominant).GetPressDown(wvr.WVR_InputId.WVR_InputId_Alias1_Touchpad);

        //var releaseGesture = Input.GetKeyDown(KeyCode.X);

        if (releaseGesture == true && transform.childCount != 0) //transform.GetChild(0).gameObject != null)
        {
            var child = transform.GetChild(0);
            child.GetComponent<Interactable>().SetInHand(false);
            child.SetParent(null);
            child.position = transform.position;
            child.rotation = Quaternion.Euler(0, 0, 0);
            child.localScale = new Vector3(1, 1, 1);
            ControllerAppearance(true);
        }
    }

    private void PositionTracker()
    {
        var type = WaveVR_Controller.Input(this.eDevice).DeviceType;

        Interop.WVR_GetPoseState(
            type,
            WVR_PoseOriginModel.WVR_PoseOriginModel_OriginOnHead,
            500,
            ref pose);

        transform.localPosition = new WaveVR_Utils.RigidTransform(pose.PoseMatrix).pos;
        transform.localRotation = new WaveVR_Utils.RigidTransform(pose.PoseMatrix).rot;
    }

    private void GetInteractable()
    {
        foreach(var interactable in interactablesCollection)
        {
            if (interactable.IsPointerOnInteractable())
            {
                interactable.SetInHand(true);
                interactable.MoveTo(transform.position, 0.05f);
                interactable.SetInHand(false);
                break;
            }
        }
    }

    /// <summary>
    /// Will be used to enable and disable the controllers' appearance
    /// </summary>
    /// <param name="value"></param>
    public void ControllerAppearance(bool value)
    {
        var renderModels = FindObjectsOfType<WaveVR_RenderModel>();

        foreach (var rm in renderModels)
            if (rm.WhichHand == hand)
            {
                var meshes = rm.GetComponentsInChildren<MeshRenderer>();
                Debug.LogWarning("*** model children: " + rm.transform.childCount);
                foreach (var mesh in meshes)
                {
                    mesh.enabled = value;
                }

                //var parent = rm.transform.parent;

                var beams = FindObjectsOfType<WaveVR_Beam>();

                foreach(var beam in beams)
                {
                    if(beam.device == WaveVR_Controller.EDeviceType.Dominant)
                    {
                        beam.ShowBeam = value;
                        beam.enabled = value;
                    }

                    Debug.LogWarning("*** Beam children: " + beam.transform.childCount);
                }

                var pointers = FindObjectsOfType<WaveVR_ControllerPointer>();

                foreach (var pointer in pointers)
                {
                    if (pointer.device == WaveVR_Controller.EDeviceType.Dominant)
                    {
                        pointer.ShowPointer = value;
                        pointer.enabled = value;
                    }

                    Debug.LogWarning("*** Pointer children: " + pointer.transform.childCount);
                }


                break;
            }
    }

    //private GameObject dominantController = null, nonDominantController = null;

    //void OnEnable()
    //{
    //    WaveVR_Utils.Event.Listen(WaveVR_Utils.Event.CONTROLLER_MODEL_LOADED, OnControllerLoaded);
    //}

    //void OnControllerLoaded(params object[] args)
    //{
    //    WaveVR_Controller.EDeviceType _type = (WaveVR_Controller.EDeviceType)args[0];
    //    if (_type == WaveVR_Controller.EDeviceType.Dominant)
    //    {
    //        this.dominantController = (GameObject)args[1];
    //        listControllerObjects(this.dominantController);
    //    }
    //    if (_type == WaveVR_Controller.EDeviceType.NonDominant)
    //    {
    //        this.nonDominantController = (GameObject)args[1];
    //        listControllerObjects(this.nonDominantController);
    //    }
    //}

    //void OnDisable()
    //{
    //    WaveVR_Utils.Event.Remove(WaveVR_Utils.Event.CONTROLLER_MODEL_LOADED, OnControllerLoaded);
    //}

    //private void listControllerObjects(GameObject ctrlr)
    //{
    //    if (ctrlr == null)
    //        return;

    //    // Get all children.
    //    GameObject[] _objects = new GameObject[ctrlr.transform.childCount];
    //    for (int i = 0; i < ctrlr.transform.childCount; i++)
    //        _objects[i] = ctrlr.transform.GetChild(i).gameObject;

    //    // Find beam.
    //    for (int i = 0; i < _objects.Length; i++)
    //    {
    //        _beam = _objects[i].GetComponentInChildren<WaveVR_Beam>();
    //        if (_beam != null)
    //            break;
    //    }
    //    if (_beam != null)
    //        Debug.Log("Find beam: " + _beam.name);

    //    // Find pointer.
    //    for (int i = 0; i < _objects.Length; i++)
    //    {
    //        _pointer = _objects[i].GetComponentInChildren<WaveVR_ControllerPointer>();
    //        if (_pointer != null)
    //            break;
    //    }
    //    if (_pointer != null)
    //        Debug.Log("Find pointer: " + _pointer.name);
    //}

    #endregion

}