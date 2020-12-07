using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseActive : MonoBehaviour
{
	public void InverseActiveGameobject()
	{
		gameObject.SetActive(!gameObject.activeSelf);
	}
}
