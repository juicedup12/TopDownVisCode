using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//collection of interfaces that transition classes
//use to get data from other classes
public interface ITileTransitionDataProvider
{
    //TileContainer.TileTransform[] GetTilesToTransform { get; }
    //TileGroupTransitionType GetTileTransitionType { get; }
    TileDataHandlerSO GetTileDataHandler { get; }
}

public interface ISpawnTransitionDataProvider
{
    GameObject[] GetSpawnObjects { get; }
}

public interface ISubroomTransitionDataProvider
{
    AnimatorTimeController[] GetTimeControllers { get; }
}
