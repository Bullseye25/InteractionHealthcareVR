using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Filler : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float speed;
    [SerializeField] private UnityEvent OnFilled;
    [SerializeField] private UnityEvent OnTriggerEntered;
    [SerializeField] private UnityEvent OnTriggerExited;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        isTriggered = true;
        OnTriggerEntered?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        image.fillAmount += Time.deltaTime * speed;
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
        OnTriggerExited?.Invoke();
    }

    private void Update()
    {
        if (!isTriggered && image.fillAmount > 0)
        {
            image.fillAmount -= Time.deltaTime * speed;
        }
        if (isTriggered && image.fillAmount >= 1)
        {
            OnFilled?.Invoke();
        }

    }
}
