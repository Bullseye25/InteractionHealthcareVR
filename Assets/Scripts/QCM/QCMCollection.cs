using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QCMCollection", menuName = "QCM/QCMCollection")]
public class QCMCollection : ScriptableObject
{
    public QuestionEntity[] questions;
}
