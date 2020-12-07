using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using DG.Tweening;

public class CustomeTimeline : MonoBehaviour
{

    [SerializeField] private TimelineEntity[] audioSources;
    [SerializeField] private AudioClipEntity[] clips;
    [SerializeField] private SignalReceiver[] signals;

    private int counter = -1;
    private bool isTimelineActive = false, waiting = false;
    private float pauseBetweenSpeech = 0.55f;

    private void Update()
    {
        if (isTimelineActive == true)
        {
            if (IsAudioInSession() == false)
            {
                counter++;
                if(counter < clips.Length)
                {
                    waiting = true;
                    StartCoroutine(PlaySource());
                }
                else
                {
                    foreach(var signal in signals)
                        signal.GetReactionAtIndex(0)?.Invoke();
                }
            }
        }
    }

    private IEnumerator PlaySource()
    {
        #region Disable Subtitles At Start
        if (counter == 0)
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].subtitleMain = audioSources[i].subtitleHolder.transform.parent.parent.gameObject;
                TimelineEntity _source = audioSources[i];
    
                if(audioSources.Length != 1)
                    _source.subtitleMain.SetActive(false);
            }
        }
        #endregion

        yield return new WaitForSeconds(pauseBetweenSpeech);

        waiting = false;
        UnityEngine.Debug.LogWarning("Playing Clip: " + counter);
        var clip = clips[counter];
        var source = GetSource(clip.id);
        source.clip = clip.clip;
        source.Play();

        foreach(var audioSource in audioSources)
        {
            if (audioSource.subtitleHolder != null)
            {
                switch (audioSource.id == clip.id)
                {
                    case true:
                        audioSource.subtitleMain.SetActive(true);

                        audioSource.subtitleHolder.text = "...";
                        audioSource.subtitleHolder.DOPlayForward();
                        var subtitleAppearingSpeed = clip.clip.length * 0.7f; // subtitles will appear 30% faster than speech
                        audioSource.subtitleHolder.DOText(clip.subtitle, subtitleAppearingSpeed, true, ScrambleMode.None).SetEase(Ease.Linear);
                        break;
                    default:
                        audioSource.subtitleMain.SetActive(false);
                        break;
                }
            }
        }
    }

    public void SetTimeLine(bool value)
    {
        isTimelineActive = value;
    }

    private AudioSource GetSource(int id)
    {
        foreach (var source in audioSources)
        {
            if (source.id == id)
                return source.source;
        }

        return new AudioSource();
    }

    private bool IsAudioInSession()
    {
        if (waiting == true)
            return true;

        foreach (var source in audioSources)
        {
            if (source.source.isPlaying)
                return true;
        }

        return false;
    }

}

[System.Serializable]
public class TimelineEntity
{
    public AudioSource source;
    public TextMeshProUGUI subtitleHolder;
    public int id;
    internal GameObject subtitleMain;

}

[System.Serializable]
public class AudioClipEntity
{
    public AudioClip clip;
    [TextArea]
	public string subtitle;
    public int id;
}
