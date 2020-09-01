using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class AudioMediaPlayer : MonoBehaviour, IMediaPlayer
{
    [SerializeField] private AudioSource source;
    public UnityEvent OnAudioFinished;
    public UnityEvent OnProceed;

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
