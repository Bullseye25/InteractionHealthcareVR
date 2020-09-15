using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPointReset : MonoBehaviour
{
    public void ResetLocalPositionAndRotation(Transform target)
    {
        target.localPosition = Vector3.zero;
        target.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
