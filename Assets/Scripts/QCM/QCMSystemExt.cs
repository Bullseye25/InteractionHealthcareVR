using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QCMSystemExt : QCMSystem
{
    #region Private Variables
    [SerializeField] private RectTransform maxParent, miniParent;
    [SerializeField] private Sprite mini, max;
    private RectTransform rect;
    private bool sizeMini = true;
    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        rect = GetComponent<RectTransform>() != null ? GetComponent<RectTransform>() : null;
    }

    #endregion

    #region Helping Functions

    /// <summary>
    /// Allow player to minimize and maximize the quiz window
    /// </summary>
    public void MaxMini(Image icon)
    {
        if (rect == null)
            return;

        if (sizeMini)
        {
            icon.sprite = mini;
            transform.SetParent(maxParent);

            sizeMini = false;
        }
        else
        {
            icon.sprite = max;
            transform.SetParent(miniParent);

            sizeMini = true;
        }

        rect.anchoredPosition = Vector3.zero;
        rect.localPosition = Vector3.zero;
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rect.sizeDelta = Vector3.zero;
    }

    #endregion

}