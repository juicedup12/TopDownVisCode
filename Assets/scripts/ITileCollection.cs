using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileCollection
{
    void RegisterTile(Vector3Int tilepos, float angle, int index);
}
