using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private bool inactiveHighlighter;
    [SerializeField] private float maxOutlineSize;
    [SerializeField] private Color onHitColor, defaultColor;
    [SerializeField] private Outliner outline;

    [SerializeField] private bool pointerEnter = false, inHand = false;

    #endregion

    #region Public Variables

    public UnityEvent OnGrab;
    
    #endregion

    #region Unity Callbacks

    private void Start()
    {
        PrepareOutline();

        Highlight(false);
    }

    #endregion

    #region Event Functions
    
    /// <summary>
    /// Will highlight & unhighlight the interactable object accordingly
    /// </summary>
    /// <param name="onPointerEnter"></param>
    public void Highlight(bool onPointerEnter)
    {
        if (inHand == true)
            return;

        pointerEnter = onPointerEnter;

        if (inactiveHighlighter == true)
            return;

        //outline.OutlineWidth = (onPointerEnter == true ? 0 : maxOutlineSize);

        outline.OutlineColor = (onPointerEnter == true ? onHitColor : defaultColor);
    }

    #endregion

    #region Helping Functions

    /// <summary>
    /// This method will be used to check if controller's pointer is on the interactable
    /// </summary>
    /// <returns></returns>
    public bool IsPointerOnInteractable()
    {
        return pointerEnter;
    }

    /// <summary>
    /// Will move the interactable object towards the controller's position
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="moveSpeed"></param>
    public void MoveTo(Vector3 destination, float moveSpeed)
    {
        transform.DOMove(destination, moveSpeed);
    }

    /// <summary>
    /// This method will be used to detect whether or not the interactable object is in hand
    /// </summary>
    /// <param name="value"></param>
    public void SetInHand(bool value)
    {
        pointerEnter = value;
        inHand = value;
    }

    private void PrepareOutline()
    {
        if (outline == null)
        {
            Debug.LogError("GameObject: " + this.gameObject.name + " Does Not Hold Outliner Script");
            this.gameObject.SetActive(false);
        }
        else
        {
            outline.OutlineColor = defaultColor;
            outline.OutlineWidth = maxOutlineSize;
        }
    }

    #endregion
}