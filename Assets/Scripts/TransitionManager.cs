using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransitionManager : MonoBehaviour
{
    private Fader transition;
    [SerializeField] private Image vrView;
    [SerializeField] private float fInTime, fOutTime, fInDelay, fOutDelay;

    private void Start()
    {
        transition = Fader.Instance;
    }

    public void FadeIn()
    {
        transition.FadeInVR(vrView, fInTime, fInDelay);
    }

    /// <summary>
    /// This will display the information in between the FadeIn and FadeOut
    /// </summary>
    /// <param name="infoText"></param>
    public void VoidAreInfo(string infoText)
    {
        var info = vrView.GetComponentInChildren<TextMeshProUGUI>();

        info.text = infoText;
    }

    public void FadeOut()
    {
        transition.FadeOutVR(vrView, fOutTime, fOutDelay);
    }
}
