using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Highlighter : MonoBehaviour
{
	private Image img;
	private Color baseColor;

	[SerializeField]
	private Color highlightColor;

	public void Awake()
	{
		img = GetComponent<Image>();
		baseColor = img.color;
	}

	public void SetHighlightColor()
	{
		img.color = highlightColor;
	}

	public void ResetColor()
	{
		img.color = baseColor;
	}


	public void OnDisable()
	{
		ResetColor();
	}

}
