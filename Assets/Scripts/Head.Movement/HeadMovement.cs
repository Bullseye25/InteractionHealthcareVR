using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform target;

    void OnAnimatorIK(int layerIndex)
    {
        //Debug.LogWarning("OnAnimatorIK");

        anim.SetLookAtPosition(target.position);

        anim.SetLookAtWeight(1.0f);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}
