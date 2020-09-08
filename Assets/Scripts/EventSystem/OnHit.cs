using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnHit : MonoBehaviour
{
    [SerializeField] private string colliderTag;
    public UnityEvent Implement;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == colliderTag)
        {
            Implement?.Invoke();
        }
    }
}
