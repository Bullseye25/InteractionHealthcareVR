using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class Fader : MonoBehaviour
{
    public static Fader Instance;
    //private MeshRenderer meshRenderer;

    private void Awake()
    {
        Instance = this;
        //meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Fade to Black
    /// </summary>
    /// <param name="_time">How much time to fade in</param>
    /// <param name="_delay">How much time before fade in</param>
    /// <param name="_callback_OnFaded">What to do after fade</param>
    //public void FadeIn(float _time, float _delay = 0, Action _callback_OnFaded = null)
    //{
    //    meshRenderer.material.DOFade(1f, _time).SetEase(Ease.Linear).SetDelay(_delay).OnComplete( () =>
    //    {
    //        _callback_OnFaded?.Invoke();
    //    });
    //}

    public void FadeInVR(Image vrView, float _time, float _delay = 0, Action _callback_OnFaded = null)
    {
        vrView.material.DOFade(1f, _time).SetEase(Ease.Linear).SetDelay(_delay).OnComplete(() =>
        {
            _callback_OnFaded?.Invoke();
        });
    }

    /// <summary>
    /// Fade to transparent
    /// </summary>
    /// <param name="_time">How much time to fade out</param>
    /// <param name="_delay">How much time before fade out</param>
    /// <param name="_callback_OnFaded">What to do after fade</param>
    //public void FadeOut(float _time, float _delay = 0, Action _callback_OnFaded = null)
    //{
    //    meshRenderer.material.DOFade(0f, _time).SetEase(Ease.Linear).SetDelay(_delay).OnComplete(() =>
    //    {
    //        _callback_OnFaded?.Invoke();
    //    });
    //}

    public void FadeOutVR(Image vrView, float _time, float _delay = 0, Action _callback_OnFaded = null)
    {
        vrView.material.DOFade(0f, _time).SetEase(Ease.Linear).SetDelay(_delay).OnComplete(() =>
        {
            _callback_OnFaded?.Invoke();
        });
    }

    //[ContextMenu("DEBUG_FadeIn")]
    //private void DEBUG_FadeIn()
    //{
    //    FadeIn(1f);
    //}
}
