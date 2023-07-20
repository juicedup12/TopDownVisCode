using System.Collections;
using UnityEngine;

public abstract class GroupTransition : MonoBehaviour
{

    public abstract void IterateGroup(float duration, float time);
}