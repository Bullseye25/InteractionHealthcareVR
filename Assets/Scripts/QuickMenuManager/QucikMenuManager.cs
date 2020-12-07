﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using wvr;

public class QucikMenuManager : MonoBehaviour
{
    private const string
        OPTION = "Button",
        TO_ENABLE_HANDS = "Left.Enable.Hands",
        TO_ENABLE_CONTROLLERS = "Right.Enable.Controllers";
    
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Color highlightColor, normalColor;
    private bool onClick = false, onButton = false;
    private Option option;
    private HandManager[] hands;

    private void Start()
    {
        hands = FindObjectsOfType<HandManager>();
        Reset();
    }

    private void OnEnable()
    {
        Reset();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == OPTION)
        {
            Debug.LogWarning("Collision: "+ collision.transform.parent.name);

            option = collision.transform.parent.GetComponent<Option>();

            // this value becomes true when the curser is on certain button
            onButton = true;

            // enable highlight Color
            collision.gameObject.transform.parent.GetComponent<Image>().color = highlightColor;         
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        onButton = false;
        collision.gameObject.transform.parent.GetComponent<Image>().color = normalColor;
    }

    private void Update()
    {
        // when user selects an option
        onClick = Application.platform == RuntimePlatform.WindowsEditor
            ? Input.GetKeyDown(KeyCode.Space)
            : WaveVR_Controller.Input(WaveVR_Controller.EDeviceType.Dominant).GetPressDown(wvr.WVR_InputId.WVR_InputId_Alias1_Touchpad);

        // check if user have selected an option
        if (onButton == true && onClick == true)
        {
            Reset();

            switch (option.name)
            {
                case TO_ENABLE_CONTROLLERS:
                case TO_ENABLE_HANDS:
                    if (IsHandAvailableToGrab()) // make sure the hands are empty
                        option.OnSelection?.Invoke();
                    break;
                default:
                    option.OnSelection?.Invoke();
                    break;
            }
        }
    }

    /// <summary>
    /// Resets the color of the buttons to default color
    /// </summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for Reset
    public void Reset()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Image>().color = normalColor;
        }
    }

    /// <summary>
    /// Determines whether both hands are availble to grab.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is hand available to grab]; otherwise, <c>false</c>.
    /// </returns>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for IsHandAvailableToGrab
    private bool IsHandAvailableToGrab()
    {
        foreach(var hand in hands)
        {
            if (!hand.CanGrab())
                return false;
        }

        return true;
    }
}