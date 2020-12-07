using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Linq;

public class TransitionManager : MonoBehaviour
{
    private Fader transition;
    [SerializeField] private MeshRenderer vrView;
    [SerializeField] private float fadeInTime, fadeOutTime, fadeInDelay, fadeOutDelay;
    [SerializeField] private TextMeshProUGUI infoBars;
    private GameObject tempObjHolder;
    private List<GameObject> objToDisableOnFade = new List<GameObject>();

    public UnityEvent OnFadeIn;
    public UnityEvent OnFadeOut;

    public static TransitionManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        transition = Fader.Instance;

        objToDisableOnFade = GameObject.FindGameObjectsWithTag("VRUIScreen").ToList();

        gameObject.SetActive(false);
    }

    public void InfoHolder(string information)
    {
        infoBars.text = information;   
    }

    public void FadeIn()
    {
        UIActivator(false);

        transition.FadeIn(fadeInTime, fadeInDelay, () => { OnFadeIn?.Invoke(); });

        StartCoroutine(DisableInfo());
    }

    /// <summary>
    /// This will display the information in between the FadeIn and FadeOut
    /// </summary>
    /// <param name="infoText"></param>
    private void VoidAreInfo(bool value)
    {    
        infoBars.gameObject.SetActive(value);
    }

    private IEnumerator DisableInfo()
    {
        yield return new WaitForSeconds(fadeOutDelay);

/*        VoidAreInfo(false);*/

        FadeOut();
    }

    public void FadeOut()
    {
        transition.FadeOut(fadeOutTime, fadeOutDelay, () => 
        { 
            OnFadeOut?.Invoke();
            UIActivator(true);
        });
    }

    public void SelfDisable()
    {
        gameObject.SetActive(false);
    }

    private void UIActivator(bool value)
    {
        foreach(var ui in objToDisableOnFade)
        {
            ui.SetActive(value);
        }
    }

    public void ClearFadeInListeners()
    {
        OnFadeIn.RemoveAllListeners();
    }

    public void ClearFadeOutListeners()
    {
        OnFadeOut.RemoveAllListeners();
    }

    public void SetGameObject(GameObject obj)
    {
        tempObjHolder = obj;
    }

    public GameObject GetGameObject()
    {
        return tempObjHolder;
    }

    public void FadeIn(bool value)
    {
        OnFadeIn.AddListener(() => 
        {
            tempObjHolder.SetActive(value);
            tempObjHolder = null;
            ClearFadeInListeners();
        });

        FadeIn();
    }

    public void FadeOut(bool value)
    {
        OnFadeOut.AddListener(() =>
        {
            tempObjHolder.SetActive(value);
            tempObjHolder = null;
            ClearFadeOutListeners();
        });
    }
    /*    private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InfoHolder("testing");
                FadeIn();
            }
        }*/
}
