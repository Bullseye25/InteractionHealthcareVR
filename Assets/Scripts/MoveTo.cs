using UnityEngine;
using DG.Tweening;

public class MoveTo : MonoBehaviour
{
    [Header("References")]

    [Tooltip("The object that will be moved")]
    [SerializeField] private Transform target;

    [Tooltip("The Transform where the object will be moved to")]
    [SerializeField] private Transform endTarget;


    [Header("Settings")]

    [Tooltip("Does the target take the endTarget rotation")]
    [SerializeField] private bool rotation;

    [Tooltip("How many seconds does the target takes to move to endTarget")]
    [SerializeField] private float duration;

    [Tooltip("Which effect to apply to the movment")]
    [SerializeField] private Ease effect = Ease.OutQuad;

    [ContextMenu("Go")]
    public void Go()
    {
        target.DOMove(endTarget.position, duration).SetEase(effect);
        if (rotation)
            target.DORotate(endTarget.rotation.eulerAngles, duration).SetEase(effect);
    }
}
