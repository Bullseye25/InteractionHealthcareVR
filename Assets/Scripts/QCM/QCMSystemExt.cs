using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QCMSystemExt : QCMSystem
{
    #region Private Variables
    [Header("Minimize & Maximize")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private RectTransform maxParent, miniParent;
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
        currentQuestion = 0;
        MaxMini();
    }

    #endregion

    #region Helping Functions

    /// <summary>
    /// Allow player to minimize and maximize the quiz window
    /// </summary>
    public void MaxMini()
    {
        if (rect == null)
            return;

        if (sizeMini)
        {
            transform.SetParent(maxParent);
            sizeMini = false;
        }
        else
        {
            transform.SetParent(null);
            sizeMini = true;
        }

        rect.DOAnchorPos(Vector3.zero, moveSpeed);
        rect.DOLocalMove(Vector3.zero, moveSpeed);
        rect.DOLocalRotate(Vector3.zero, moveSpeed);
        rect.DOSizeDelta(Vector2.zero, moveSpeed);
    }

    public void SetMaxSize(RectTransform _maxParent)
    {
        maxParent = _maxParent;
    }

    public void SetMiniSize(RectTransform _miniParent)
    {
        miniParent = _miniParent;
    }

    #endregion

}