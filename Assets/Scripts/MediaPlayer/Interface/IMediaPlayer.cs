using UnityEngine;
using System.Collections;

public interface IMediaPlayer
{
    void Play();
    void Repeat();
    void Proceed();
    void OnMediaFinished();
}
