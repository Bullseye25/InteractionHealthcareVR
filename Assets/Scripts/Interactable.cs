using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private const string 
        HIGHLIGHTER_PROPERTY_OUTLINE_RANGE = "_Outline",
        HIGHLIGHTER_PROPERTY_OUTLINE_COLOR = "_OutlineColor";

    [SerializeField] private MeshRenderer mRenderer;
    [SerializeField] private Material highlighterRef;
    private Material highlighter;
    [SerializeField]
    private Color highlightingColor;
    [Range(0, 0.1f)]
    [SerializeField] private float maxOutlineSize;
    [SerializeField] private float highlightDelay, moveDuration;
    private HandManager primaryHand;
 
    private void Start()
    {
        ErrorHandler();

        StartCoroutine(GetPrimaryHand());

        PrepareHighlighter();
    }

    public void Highlight(bool onPointerEnter)
    {
        highlighter.DOFloat(onPointerEnter == true ? 0 : maxOutlineSize, HIGHLIGHTER_PROPERTY_OUTLINE_RANGE, highlightDelay);

        if (onPointerEnter == true)
            StartCoroutine(CheckTrigger(onPointerEnter));
        else
            StopCoroutine(CheckTrigger(onPointerEnter));
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

        highlighter.SetColor(HIGHLIGHTER_PROPERTY_OUTLINE_COLOR, highlightingColor);
            
        var materials = new Material[mRenderer.materials.Length + 1];

        materials[0] = highlighter;

        for (int i = 1; i < materials.Length; i++)
        {
            materials[i] = mRenderer.materials[i - 1];
        }

        mRenderer.materials = materials;
    }

    private IEnumerator GetPrimaryHand()
    {
        yield return new WaitForSeconds(0.5f);

        var hands = FindObjectsOfType<HandManager>();

        foreach (var hand in hands)
        {
            if(hand.IsPrimary())
            {
                primaryHand = hand;
                break;
            }
        }
    }

    private IEnumerator CheckTrigger(bool onPointerEnter)
    {
        yield return new WaitForSeconds(0);

        if (onPointerEnter == true && primaryHand.IsTrigger() == true)
        {
            transform.DOMove(primaryHand.transform.position, moveDuration);
        }

        Debug.Log("In Routine");

        if (onPointerEnter == true)
            StartCoroutine(CheckTrigger(onPointerEnter));
        else
            StopCoroutine(CheckTrigger(onPointerEnter));
    }
}