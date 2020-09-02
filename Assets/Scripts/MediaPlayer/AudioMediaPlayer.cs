using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Playables;

public class AudioMediaPlayer : MonoBehaviour, IMediaPlayer
{
    [SerializeField] private AudioSource source;
    public UnityEvent OnAudioFinished;

    public UnityEvent OnProceed;

    private TransitionManager transition;
    private GameObject tempObjHolder;

    private void Start()
    {
        transition = TransitionManager.Instance;
    }

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

    public void SetGameObject(GameObject obj)
    {
        tempObjHolder = obj;
    }

    public GameObject GetGameObject()
    {
        return tempObjHolder;
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

    public void Proceed(int componentIndex)
    {
        OnProceed.AddListener(() =>
        {
            if (componentIndex == (int)UnityComponents.BOX_COLLIDER)
                tempObjHolder.GetComponent<BoxCollider>().enabled = true;
            else if (componentIndex == (int)UnityComponents.PLAYER_DIRECTOR)
                tempObjHolder.GetComponent<PlayableDirector>().enabled = true;

            tempObjHolder = null;
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

public enum UnityComponents
{
    BOX_COLLIDER,
    PLAYER_DIRECTOR,
};