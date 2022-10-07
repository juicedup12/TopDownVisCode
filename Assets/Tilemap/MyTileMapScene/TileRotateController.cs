using System.Collections;
using System.Collections.Generic;
using topdown;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileRotateController : MonoBehaviour, IRoomTransitioner
{
    [SerializeField]
    Vector3Int[] TilePositions;
    Tilemap tmap;
    [SerializeField]
    float DelayOffset = .2f, RotateTime = 2;
    iStageBuild builder;

    //gets called by preset level creator
    public void DoRoomTransition(iStageBuild LevelBuilder)
    {
        builder = LevelBuilder;
        tmap = GetComponent<Tilemap>();
        //start rotate co routine based on foreach loop
        if (TilePositions != null)
            for (int i = 0; i < TilePositions.Length; i++)
            {
                if (TilePositions[i] != null)
                {
                    print(i + " tile position " + i + " is at " + TilePositions[i]);
                    //RotateTile(i);
                    StartCoroutine(RotateRoutine(i, i * DelayOffset));
                }
            }
        if (TilePositions.Length < 1)
            print("no array");
    }

    //without callback
    public void DoRoomTransition()
    {
        tmap = GetComponent<Tilemap>();
        //start rotate co routine based on foreach loop
        if (TilePositions != null)
            for (int i = 0; i < TilePositions.Length; i++)
            {
                if (TilePositions[i] != null)
                {
                    //print(i + " tile position " + i + " is at " + TilePositions[i]);
                    //RotateTile(i);
                    StartCoroutine(RotateRoutine(i, i * DelayOffset));
                }
            }
        if (TilePositions.Length < 1)
            print("no array");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RegisterTile(Vector3Int tilepos, int Index)
    {
        if (TilePositions != null)
        {
            print("there's already an array");
            //if there's an array make it bigger
            print("making new array with size of " + Index);
            Vector3Int[] tempPos = new Vector3Int[Index];
            TilePositions.CopyTo(tempPos, 0);
            tempPos[Index - 1] = tilepos;
            TilePositions = tempPos;
            return;
        }

        print("registering new array with tile at " + tilepos + " at index " + Index);
        TilePositions[0] = tilepos;
    }

    Matrix4x4 RotateTile(int Index, Matrix4x4 startMat ,float time)
    {
        //do a certain rotation depending on the current orientation of the tile at index
        Vector3 EulerRotation = startMat.rotation.eulerAngles;
        print("rotation for tile at pos: " + TilePositions[Index] + " is " + startMat.rotation.eulerAngles);
        Quaternion rotation = new Quaternion();
        if (EulerRotation == Vector3.zero)
        {
            //print("tile " + TilePositions[Index] + " facing right");
            rotation = Quaternion.Lerp(Quaternion.Euler(0, -90f, 0), startMat.rotation, time / RotateTime);
            return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        }
        if(EulerRotation.z == 90)
        {
            //print("tile " + TilePositions[Index] + " facing up");
            rotation = Quaternion.Lerp(startMat.rotation, startMat.rotation * Quaternion.Euler(0, -90, 0), time / RotateTime);
            return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        }
        if (EulerRotation.z == 270)
        {
            //print("tile " + TilePositions[Index] + " facing down");
            rotation = Quaternion.Lerp(startMat.rotation, startMat.rotation * Quaternion.Euler(0, 90, 0), time / RotateTime);
            return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        }
        rotation = Quaternion.identity;
        return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
    }

    Matrix4x4 SetStartRotation(Vector3Int Tilepos)
    {
        //go through all elements and rotate
        Quaternion rotation;
        Vector3 EulerRotation = tmap.GetTransformMatrix(Tilepos).rotation.eulerAngles;
        //print("rotation for tile at pos: " + Tilepos + " is " + EulerRotation);
        if (EulerRotation == Vector3.zero)
        {
            //print("tile " + Tilepos + " facing right");
            rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        }
        if (EulerRotation.z == 90)
        {
            //print("tile " + Tilepos + " facing up");
            rotation = Quaternion.Euler(90, 0, EulerRotation.z);
            return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        }
        if (EulerRotation.z == 270)
        {
            //print("tile " + Tilepos + " facing down");
            rotation = Quaternion.Euler(90, 0, EulerRotation.z);
            return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);

        }
        if (EulerRotation.z == 180)
        {
            //print("tile " + Tilepos + " facing left");
            rotation = Quaternion.Euler(new Vector3(0, 90, 180));
            return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        }

        return Matrix4x4.identity;
    }

    IEnumerator RotateRoutine(int Index, float delay)
    {
        //rotate before delay
        Matrix4x4 EndMatr = tmap.GetTransformMatrix(TilePositions[Index]);
        Matrix4x4 StartMatr = SetStartRotation(TilePositions[Index]);
        tmap.SetTransformMatrix(TilePositions[Index], StartMatr);
        yield return new WaitForSeconds(delay);
        //print("starting rotate routine at index " + Index + " out of " + TilePositions.Length);
        float time = 0;

        while (time < RotateTime)
        {
            Quaternion rotation = Quaternion.Lerp(StartMatr.rotation, EndMatr.rotation, time/RotateTime);
            //Matrix4x4 rotation = RotateTile(Index, SetEndMatr, time);
            //print("rotation routine percent is " + rotation.rotation.eulerAngles);
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
            tmap.SetTransformMatrix(TilePositions[Index], matrix);
            time += Time.deltaTime;
            yield return null;
        }
        if(Index == TilePositions.Length -1)
        {
            //print("did final tile");
            if(builder != null)
            builder.SetupLevels();
        }

        //Matrix4x4 matrixEnd = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90f, 90f, 90f), Vector3.one);
        tmap.SetTransformMatrix(TilePositions[Index], EndMatr);
    }

    public Vector2 getspawn()
    {
        throw new System.NotImplementedException();
    }
}
