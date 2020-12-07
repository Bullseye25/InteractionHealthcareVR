using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

public class PopUpBehaviour : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Transform target;
    [SerializeField] private TMPro.TextMeshProUGUI info;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetInfoText(string _info)
    {
        info.text = _info;
    }

    public void SetPositionAndRotation(RectTransform _rect)
    {
        rect.anchoredPosition3D = _rect.anchoredPosition;
        rect.rotation = _rect.rotation;
        rect.sizeDelta = _rect.sizeDelta;
    }

    private void Update()
    {
        if (target == null)
            return;

        var lookAtTarget = target.position;
        
        transform.LookAt(lookAtTarget, Vector3.up);
    }

}
