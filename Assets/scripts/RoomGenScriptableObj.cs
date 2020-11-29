using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomGenScriptableObject", order = 1)]
public class RoomGenScriptableObj : ScriptableObject
{
    public GameObject FloorTile;
    public GameObject Carpet;
    [SerializeField]
    public string InsideOfRoomSortLyaer;
    public Ease RotEase, RotEaseHorizontal, tileEase, enemyScaleEase, enemyLowerEase, enemyHopEase ;
    public GameObject HallwayLight;
}
