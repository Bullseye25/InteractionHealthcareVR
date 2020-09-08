using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    #region Private Variables

    private const string 
        HIGHLIGHTER_PROPERTY_OUTLINE_RANGE = "_Outline",
        HIGHLIGHTER_PROPERTY_OUTLINE_COLOR = "_OutlineColor";

    [SerializeField] private MeshRenderer mRenderer;
    [SerializeField] private Texture texture;

    [SerializeField] private Material highlighterRef;
    private Material highlighter;

    [SerializeField] private Color highlightingColor;

    [Range(0, 0.1f)]
    [SerializeField] private float maxOutlineSize;
    
    [SerializeField] private float highlightDelay, moveDuration;
    
    [SerializeField] private bool pointerEnter = false, inHand = false;

    #endregion

    #region Public Variables

    public UnityEvent OnGrab;
    
    #endregion

    #region Unity Callbacks

    private void Start()
    {
        ErrorHandler();

        PrepareHighlighter();

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
        highlighter.DOFloat(onPointerEnter == true ? 0 : maxOutlineSize, HIGHLIGHTER_PROPERTY_OUTLINE_RANGE, highlightDelay);
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

    private void ErrorHandler()
    {
        if (mRenderer == null || highlighterRef == null)
        {
            Debug.LogError("Interactable (" + gameObject.name + ") : MeshRenderer or Material Not Found!");
            gameObject.SetActive(false);
        }
    }

    private void PrepareHighlighter()
    {
        highlighter = new Material(highlighterRef)
        {
            name = "HIGHLIGHTER"
        };

        if(texture != null)
            highlighter.mainTexture = texture;

        highlighter.SetColor(HIGHLIGHTER_PROPERTY_OUTLINE_COLOR, highlightingColor);

        var materials = new Material[mRenderer.materials.Length + 1];

        materials[0] = highlighter;

        for (int i = 1; i < materials.Length; i++)
        {
            materials[i] = mRenderer.materials[i - 1];
        }

        mRenderer.materials = materials;
    }

    #endregion
}