using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clients pass properties to transition manager to be assigned to group transitions
public interface ITileTransitionClient 
{
    TileContainer.TileTransform[] GetTilesToTransform { get; }
    TileGroupTransitionType GetTileTransitionType { get; }
}

public enum TileGroupTransitionType
{
    Rotate, Scale, Transform, Random
}
