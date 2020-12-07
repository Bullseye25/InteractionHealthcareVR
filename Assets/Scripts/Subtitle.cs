using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Subtitle : MonoBehaviour
{
    private TextMeshPro mytmp;
    void Start()
    {
        mytmp = GetComponent<TextMeshPro>();
    }

    public void DoSubtitle(String input)
    {
        mytmp.text = input;
    }
}
