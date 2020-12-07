﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Playables;
using Boo.Lang;
using System.Linq;
using UnityEngine.Timeline;

public class AudioMediaPlayer : MonoBehaviour, IMediaPlayer
{
    [SerializeField] private AudioSource source;
    private TransitionManager transition;
    public UnityEvent OnAudioFinished;
    public UnityEvent OnProceed;

    private void Start()
    {
        transition = TransitionManager.Instance;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        if (this.gameObject.name == "Patient.C.Audio.Source")
    //        {
    //            GetComponent<AudioSource>().Play();
    //        }
    //    }
    //}

    public void OnMediaFinished()
    {
        StartCoroutine(IsFinished());
    }

    private IEnumerator IsFinished()
    {
        yield return new WaitForSeconds(1.0f);

        if (source.isPlaying)
        {
            StartCoroutine(IsFinished());
        }
        else
        {
            OnAudioFinished?.Invoke();
        }
    }

    public void SetClip(AudioClip clip)
    {
        source.clip = clip;
    }

    public void Play()
    {
        if (!source.isPlaying)
        {
            source.Play();
        }
    }

    public void Proceed(bool value)
    {
        OnProceed.AddListener(() =>
        {
            transition.gameObject.SetActive(true);
            transition.FadeIn(value);
            OnProceed.RemoveAllListeners();
        });
    }

    public void Proceed(GameObject gameObject)
    {
        OnProceed.AddListener(() =>
        {
            gameObject.SetActive(true);
            OnProceed.RemoveAllListeners();
        });
    }

    public void Proceed()
    {
        OnProceed?.Invoke();
    }

    public void ClearProceed()
    {
        OnProceed.RemoveAllListeners();
    }

    public void ClearAudioFinished()
    {
        OnAudioFinished.RemoveAllListeners();
    }

    public void Repeat()
    {
        Play();
    }
}