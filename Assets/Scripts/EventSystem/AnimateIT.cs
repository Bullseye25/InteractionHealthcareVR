using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateIT : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private string parameterName;

    public void SetParameterName(string _parameterName)
    {
        parameterName = _parameterName;
    }

    public void SetAnimation(bool value)
    {
        animator.SetBool(parameterName, value);
    }

    public void SetAnimation(int value)
    {
        animator.SetInteger(parameterName, value);
    }
}
