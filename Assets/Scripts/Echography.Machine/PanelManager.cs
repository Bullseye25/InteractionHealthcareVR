using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] items;

    private int currentIndex = 0;

    public void Next()
    {
        Debug.Log("Length: " + items.Length);
        Debug.Log("currentIndex: " + currentIndex);
        if ((currentIndex + 1) >= items.Length)
            return;
        Debug.Log(" NEXT ");
        items[currentIndex].SetActive(false);
        currentIndex++;
        items[currentIndex].SetActive(true);
    }

    public void Back()
    {
        Debug.Log("Length: " + items.Length);
        Debug.Log("currentIndex: " + currentIndex);

        if ((currentIndex - 1) < 0)
            return;

        Debug.Log(" BACK ");

        items[currentIndex].SetActive(false);

        if (currentIndex != 0)
            currentIndex--;

        items[currentIndex].SetActive(true);
    }
}
