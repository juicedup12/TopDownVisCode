using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new 3d object", menuName = "StackObject", order = 2)]
public class stackobject : ScriptableObject
{
    public List<Sprite> stack;
}
