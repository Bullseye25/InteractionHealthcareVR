using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wvr;
using WVR_Log;

public class HandManager : MonoBehaviour
{
    #region Private Variables
    private const string
        LOG_TAG = "ControllerManagerTest",
        INTERACTABLE = "Interactable",
        DICTAPHONE = "Dictaphone",
        DOSSIER_PLANCHE = "DossierPlanche",
        SMARTPHONE = "Smartphone",
        T_SMARTPHONE = "Tutorial.Smartphone",
        ECHOGRAPHY_DEVICE = "Ultrasound.Device";

    [SerializeField] private wvr.WVR_InputId resetMenuActivator;
    [SerializeField] private WaveVR_Controller.EDeviceType eDevice;
    [SerializeField] private List<Interactable> interactablesCollection = new List<Interactable>();
    [SerializeField] private GameObject tutorialManager, resetMenu;
    [SerializeField] private SkinnedMeshRenderer handMesh;
    //[SerializeField] private WaveVR_Beam _beam = null;
    //[SerializeField] private WaveVR_ControllerPointer _pointer = null;

    private WaveVR_RenderModel.ControllerHand hand;
    private WVR_DeviceType deviceType;
    #endregion

    #region Unity Callbacks

    IEnumerator Start() // using Enumerator because the controllers respose with some delay
    {
        yield return new WaitForSeconds(0.75f);

        #region Get the controllers when using Casque 

        hand = eDevice == WaveVR_Controller.EDeviceType.Dominant ? WaveVR_RenderModel.ControllerHand.Controller_Dominant : WaveVR_RenderModel.ControllerHand.Controller_NonDominant;
        deviceType = eDevice == WaveVR_Controller.EDeviceType.Dominant ? WVR_DeviceType.WVR_DeviceType_Controller_Right : WVR_DeviceType.WVR_DeviceType_Controller_Left;

        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
                {
                    var renderModels = FindObjectsOfType<UniversalControllerActions>();

                    foreach (var rm in renderModels)
                        if (rm.device == deviceType)
                        {
                            ControllerMeshManager(rm.transform);
                            break;
                        }

                    break;
                }

            default:
                {
                    var _renderModels = FindObjectsOfType<WaveVR_RenderModel>();

                    foreach (var rm in _renderModels)
                        if (rm.WhichHand == hand)
                        {
                            ControllerMeshManager(rm.transform);
                        }

                    break;
                }
        }

        #endregion
    }

    public void OnEnable()
    {
        StartCoroutine(Start());
    }

    private void Update()
    {
        #region Keep the list of Interactables
        var _interactables = FindObjectsOfType<Interactable>();

        foreach (var _interactable in _interactables)
        {
            if (!interactablesCollection.Contains(_interactable))
            {
                interactablesCollection.Add(_interactable);
            }
        }
        #endregion

        Grab();

        ResetMenuManager();

        //Release();

    }



    private void OnTriggerEnter(Collider value)
    {
        Debug.LogWarning("value.tag: " + value.tag);
        Debug.LogWarning("transform.childCount: " + transform.childCount);

        if (value.tag == INTERACTABLE                                      // Make sure the object is interactiable 
            && transform.childCount == 2                                   // The Hand already holds the skin mesh and the bones as the child, so make sure the hand do not have more then 2 children 
            && value.transform.parent.GetComponent<HandManager>() == null) // make sure the interactable object is not already in other hand
        {
            value.transform.SetParent(this.transform);                     // make the selected object as the child of this hand

            value.transform.localPosition = Vector3.zero;                  // reset the position of the selected object

            #region Rotate The Selected Object Accordingly
            if (value.name == DICTAPHONE || value.name == DOSSIER_PLANCHE)
            {
                value.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (value.name == SMARTPHONE || value.name == T_SMARTPHONE)
            {
                value.transform.localRotation = Quaternion.Euler(-45, 180, 0);
            }
            else if (value.name == ECHOGRAPHY_DEVICE)
            {
                value.transform.localRotation = Quaternion.Euler(45f, 0, 0);
            }
            #endregion

            ControllerAppearance(false);                                   // make controller disappear 

            value.GetComponent<Interactable>().OnGrab?.Invoke();
        }
    }

    #endregion

    #region Helping Functions

    /// <summary>
    /// This will make sure to Disable controler mesh at the start of the application
    /// </summary>
    /// <param name="rm"></param>
    private void ControllerMeshManager(Transform rm)
    {
        transform.SetParent(rm);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        var meshes = new List<MeshRenderer>();

        foreach (Transform child in rm)
        {
            if (child.GetComponent<MeshRenderer>() != null && !meshes.Contains(child.GetComponent<MeshRenderer>()))
            {
                meshes.Add(child.GetComponent<MeshRenderer>());

                if (child.transform.childCount >= 0)
                {
                    foreach (Transform subChild in child)
                    {
                        if (subChild.GetComponent<MeshRenderer>() != null && !meshes.Contains(subChild.GetComponent<MeshRenderer>()))
                        {
                            meshes.Add(subChild.GetComponent<MeshRenderer>());
                        }
                    }
                }
            }
        }

        foreach (var mesh in meshes)
        {
            if (mesh.GetComponent<HandManager>() == null && !mesh.gameObject.CompareTag(INTERACTABLE) && !mesh.gameObject.name.Contains("Pointer"))
            {
                Debug.LogWarning($"Hand Manager is disabling {mesh.gameObject.name} Tag: {mesh.gameObject.tag}");
                mesh.enabled = false;
            }
        }
    }

    public void SetDeviceIndex(WaveVR_Controller.EDeviceType device)
    {
        Log.i(LOG_TAG, "SetDeviceIndex, _index = " + device);
        //this.eDevice = device;
    }

    /// <summary>
    /// This function will perform the Grab Behaviour
    /// </summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for Grab
    private void Grab()
    {
        if (eDevice != WaveVR_Controller.EDeviceType.Dominant)
            return;

        bool grabGesture;

        if (Application.platform == RuntimePlatform.WindowsEditor)
            grabGesture = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1);
        else
            grabGesture = WaveVR_Controller.Input(eDevice).GetPressDown(wvr.WVR_InputId.WVR_InputId_Alias1_Trigger);

        if (grabGesture == true)
        {
            if (CanGrab())
            {
                GetInteractable();
            }
            else
            {
                ///NOTE: if you want to do something while certain object is in hand.. Apply a method here
            }
        }
    }


    private void Release()
    {
        var releaseGesture = WaveVR_Controller.Input(eDevice).GetPressDown(wvr.WVR_InputId.WVR_InputId_Alias1_Touchpad);

        //var releaseGesture = Input.GetKeyDown(KeyCode.X);

        if (releaseGesture == true && transform.childCount != 2)
        {
            var child = transform.GetChild(0);
            if (child.GetComponent<Interactable>() != null)
            {
                child.GetComponent<Interactable>().SetInHand(false);
                child.SetParent(null);
                child.position = transform.position;
                child.rotation = Quaternion.Euler(0, 0, 0);
                child.localScale = new Vector3(1, 1, 1);
            }
            ControllerAppearance(true);
        }
    }

    private void ResetMenuManager()
    {
        if (eDevice != WaveVR_Controller.EDeviceType.Dominant)
            return;

        bool resetMenuEnabler = WaveVR_Controller.Input(eDevice).GetPressDown(resetMenuActivator);
        if (resetMenuEnabler &&
         // enable menu only outside of tutorial
         !GameObject.Find("Tutorial.Manager"))
        {
            resetMenu.SetActive(true);
        }

    }

    /// <summary>
    /// Will move the selected Object, and will make the child of the hand
    /// </summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for GetInteractable
    private void GetInteractable()
    {
        foreach (var interactable in interactablesCollection)
        {
            if (interactable.IsPointerOnInteractable())
            {
                interactable.SetInHand(true);
                interactable.MoveTo(transform.position, 0.05f);
                interactable.SetInHand(false);

                // the Page Was not responding so, This will force page to respond quickly as needed
                if (interactable.gameObject.name == "Page")
                    interactable.OnGrab?.Invoke();

                break;
            }
        }
    }

    /// <summary>
    /// IMPORTANT NOTE: You can only test this method in Casque
    /// Will be used to enable and disable the 3D-hands' appearance
    /// </summary>
    /// <param name="value"></param>
    public void ControllerAppearance(bool value)
    {
        if (transform.parent.name == "Hands")
        {
            Debug.LogWarning(transform.name + " Controller Not In Use");
            handMesh.enabled = false;
            return;
        }

        Debug.LogWarning("ControllerAppearance: " + value);

        #region Enable or Disable the hands mesh as according to the value of the parameter

        handMesh.enabled = value;

        #endregion

        #region Enable or Disable the Beam as according to the value of the parameter

        var beams = FindObjectsOfType<WaveVR_Beam>();

        foreach (var beam in beams)
        {
            if (beam.device == WaveVR_Controller.EDeviceType.Dominant) // Will only enable the beam of the Dominant controller
            {
                beam.ShowBeam = value;
                beam.enabled = value;
            }

            Debug.LogWarning("*** Beam children: " + beam.transform.childCount);
        }
        #endregion

        #region Enable or Disable the Pointer as according to the value of the parameter

        var pointers = FindObjectsOfType<WaveVR_ControllerPointer>();

        foreach (var pointer in pointers)
        {
            if (pointer.device == WaveVR_Controller.EDeviceType.Dominant)// Will only enable the pointer of the Dominant controller
            {
                Debug.LogWarning($"{(value ? "enabling" : "disabling")} the pointer");
                pointer.ShowPointer = value;
                pointer.enabled = value;
            }

            Debug.LogWarning("*** Pointer children: " + pointer.transform.childCount);
        }
        #endregion
    }

    public bool CanGrab()
    {
        return transform.childCount == 2;
    }

    public WaveVR_Controller.EDeviceType GetController()
    {
        return eDevice;
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