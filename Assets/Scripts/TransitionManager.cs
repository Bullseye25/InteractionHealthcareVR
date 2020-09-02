using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TransitionManager : MonoBehaviour
{
    private Fader transition;
    [SerializeField] private MeshRenderer vrView;
    [SerializeField] private float fadeInTime, fadeOutTime, fadeInDelay, fadeOutDelay;
    [SerializeField] private TextMeshProUGUI[] infoBars;

    private GameObject tempObjHolder;

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

        PrepareInfoBar();

        gameObject.SetActive(false);

    }

    private void PrepareInfoBar()
    {
        infoBars = new TextMeshProUGUI[vrView.transform.childCount];

        for (int i = 0; i < infoBars.Length; i++)
        {
            infoBars[i] = vrView.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }
    }

    public void InfoHolder(string information)
    {
        foreach(var infoHolder in infoBars)
        {
            infoHolder.text = information;
        }
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
        foreach (var infoHolder in infoBars)
        {
            infoHolder.gameObject.SetActive(value);
        }
    }

    private IEnumerator DisableInfo()
    {
        yield return new WaitForSeconds(fadeOutDelay);

/*        VoidAreInfo(false);*/

        FadeOut();
    }

    public void FadeOut()
    {
        transition.FadeOut(fadeOutTime, fadeOutDelay, () => { OnFadeOut?.Invoke(); });
    }

    public void SelfDisable()
    {
        gameObject.SetActive(false);
    }

    private void UIActivator(bool value)
    {
        foreach(var ui in GameObject.FindGameObjectsWithTag("VRUIScreen"))
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

    public void OnFadeInOBJ(bool value)
    {
        OnFadeIn.AddListener(() => 
        {
            tempObjHolder.SetActive(value);
            tempObjHolder = null;
            ClearFadeInListeners();
        });

        FadeIn();
    }

    public void OnFadeOutOBJ(bool value)
    {
        OnFadeOut.AddListener(() =>
        {
            tempObjHolder.SetActive(value);
            tempObjHolder = null;
            ClearFadeOutListeners();
        });

        FadeOut();
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
