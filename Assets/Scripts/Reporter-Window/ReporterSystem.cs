using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReporterSystem : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI title, discription;
    //[SerializeField] private Image windowIcon;
    private AudioMediaPlayer audioPlayer;
    
    //public void SetTitle(string _title)
    //{
    //    //title.text = _title;
    //}

    //public void SetDiscription(string _discription)
    //{
    //    //discription.text = _discription;
    //}

    //public void SetIcon(Sprite _iconImage)
    //{
    //    //windowIcon.sprite = _iconImage;
    //}

    public void SetAudioPlayer(AudioMediaPlayer _audioPlayer)
    {
        audioPlayer = _audioPlayer;
    }

    public void SetRead(AudioClip clip)
    {
        if (audioPlayer == null)
            return;

        audioPlayer.SetClip(clip);
    }

    public void Read()
    {
        if (audioPlayer == null)
            return;

        audioPlayer.Play();
    }

    public void Proceed()
    {
        if (audioPlayer == null)
            return;

        audioPlayer.Proceed();
    }

}
