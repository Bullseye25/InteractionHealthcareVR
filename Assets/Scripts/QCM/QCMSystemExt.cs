using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QCMSystemExt : QCMSystem
{
    #region Private Variables
    [SerializeField] private RectTransform maxParent, miniParent;
    [SerializeField] private Sprite mini, max;
    [SerializeField] private float moveSpeed;
    private RectTransform rect;
    private bool sizeMini = true;
    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        rect = GetComponent<RectTransform>() != null ? GetComponent<RectTransform>() : null;

        OnEnable();
    }

    public void OnEnable()
    {
        questionHolder.gameObject.SetActive(true);

        MaxMini(null);
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
            if (icon != null)
                icon.sprite = mini;

            transform.SetParent(maxParent);


            sizeMini = false;
        }
        else
        {
            if (icon != null)
                icon.sprite = max;

            transform.SetParent(miniParent);

            sizeMini = true;
        }

        rect.DOAnchorPos(Vector3.zero, moveSpeed);
        rect.DOLocalMove(Vector3.zero, moveSpeed);
        rect.DOLocalRotate(Vector3.zero, moveSpeed);
        rect.DOSizeDelta(Vector2.zero, moveSpeed);
    }

    #endregion

}