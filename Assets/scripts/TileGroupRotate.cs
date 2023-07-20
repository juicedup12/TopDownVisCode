
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileGroupRotate : TileGroupTransition
{
    public override void Awake()
    {
        base.Awake();
    }

    //called when a new room is activated
    public override void NewTiles(TileContainer.TileTransform[] tiles)
    {
        base.NewTiles(tiles);
        SetTilesStartRot();
    }

    /*change conditions to check the tile transform angle
    public  void LerpObjects(float duration, float time)
    {
        //moving to awake
        //base.LerpObjects();

        if (tmap == null)
        {
            print("getting tmap reference");
            tmap = GetComponent<Tilemap>();
        }
        for (int i = 0; i < TileLength; i++)
        {
            //Quaternion tileRot = tmap.GetTransformMatrix(tiles[i].TilePos).rotation;
            //Vector3 TileEuler = tileRot.eulerAngles;
            //float angle = Quaternion.Angle(Quaternion.Euler(Vector3.zero), Quaternion.Euler(TileEuler));
            //float angle = 0;
            //Vector3 axis = Vector3.zero;
            //tileRot.ToAngleAxis(out angle,out axis);
            //print("angle is " + Mathf.Round(angle) + "/n" + "axis is " + axis);
            float angle = tiles[i].TileZAngle;
            //print("current angle is " + angle);
            //float startTime = i * (duration / TileLength);
            //print("tile index " + i + " at " + tiles[i].ToString() + "rotation is " + tileRot.eulerAngles);
            //print("duration is " + duration);
            //print("tile length is " + TileLength);
            //detremine x or y rotation 
            //if it's lower or greater

            float durationsplit = duration / TileLength;
            float startTime = i * durationsplit;
            float localTime = time - startTime;
            float timePercentage = localTime / durationsplit;

            if(time >= startTime + durationsplit)
            {
                RotateTile(i, 1);
                continue;
            }

            //print("start time is " + startTime + " duration split is " + durationsplit);
            if (time > startTime && time < startTime + durationsplit)
            {
                print("rotate lerp working on " + i);
                RotateTile(i, timePercentage);
                //Quaternion StartAngle;
                //if (angle == 0)
                //{
                //    StartAngle = Quaternion.Euler(new Vector3(0, -90, 0));
                //    //Quaternion Rot = Quaternion.Lerp(StartAngle, Quaternion.identity, time / duration);
                //    //Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, Rot, Vector3.one);
                //    //tmap.SetTransformMatrix(tiles.GetTileTransforms[i].TilePos, m);
                //    RotateTile(i, timePercentage, StartAngle, Quaternion.identity);
                //}


                //if (angle == 90)
                //{
                //    StartAngle = Quaternion.Euler(new Vector3(90, 0, 90));
                //    Quaternion EndAngle = Quaternion.Euler(new Vector3(0, 0, 90));
                //    RotateTile(i, timePercentage, StartAngle, EndAngle);
                //}

                //if (angle == 180)
                //{
                //    StartAngle = Quaternion.Euler(0, 90, 180);
                //    Quaternion EndAngle = Quaternion.Euler(new Vector3(0, 0, 180));
                //    RotateTile(i, timePercentage, StartAngle, EndAngle);
                //}

                //if (angle == 270)
                //{
                //    StartAngle = Quaternion.Euler(90, 0, 270);
                //    Quaternion Endangle = Quaternion.Euler(new Vector3(0, 0, 270));
                //    RotateTile(i, timePercentage, StartAngle, Endangle);
                //}

                //Quaternion RightRotation = Quaternion.Euler(new Vector3(0, 90, 0));
                //print("right rotation angle is " + Quaternion.Angle(tileRot, RightRotation));
                ////facing right
                //if (Quaternion.Angle(tileRot, RightRotation) <= 90)
                //{
                //    print("facing right");
                //    RotateTile(i, duration / TileLength, time, RightRotation, Quaternion.identity);
                //    continue;
                //}

                //Quaternion LeftRotation = Quaternion.Euler(new Vector3(0, -90, -180));
                //print("left rotation angle is " + Quaternion.Angle(tileRot, LeftRotation));
                /////facing left
                //if (Quaternion.Angle(tileRot, LeftRotation) <= 90)
                //{
                //    print("facing left");
                //    Quaternion EndAngle = Quaternion.Euler(new Vector3(0, 0, -180));
                //    RotateTile(i, duration / TileLength, time, LeftRotation, EndAngle);
                //    continue;
                //}



                ////if (Mathf.Round(angle) == .5)
                ////{
                ////    print("facing up");
                //Quaternion UpRotation = Quaternion.Euler(new Vector3(90, 0, 90));
                ////try getting the current angle from starting angle
                ////if it's 90 or less then we know what the starting angle is
                //print("up rotation angle is " + Quaternion.Angle(tileRot, UpRotation));

                //if ( Quaternion.Angle(tileRot, UpRotation) <= 90)
                //{
                //    print("angle is facing up");
                //    Quaternion EndAngle = Quaternion.Euler(new Vector3(0, 0, 90));
                //    RotateTile(i, duration / TileLength, time, UpRotation, EndAngle);
                //    continue;
                //}


                //Quaternion DownRotation = Quaternion.Euler(new Vector3(270, 0, 270));
                //print("down rotation angle is " + Quaternion.Angle(tileRot, DownRotation));
                //if (Quaternion.Angle(tileRot, DownRotation) <= 90)
                //{
                //    print("facing down");
                //    Quaternion EndAngle = Quaternion.Euler(new Vector3(0, 0, 270));
                //    RotateTile(i, duration / TileLength, time, DownRotation, EndAngle);
                //}

                //facing up
                //if (angle >0)
                //Quaternion StartAngle = Quaternion.Euler(new Vector3(90, 90, 0));
                //Quaternion EndAngle = Quaternion.Euler(new Vector3(0, 0, 90));
                //RotateTile(i, duration / TileLength, time, StartAngle, EndAngle);
                //Quaternion StartAngle = Quaternion.Euler(new Vector3(90, 0, 90));
                //Quaternion EndAngle = Quaternion.Euler(new Vector3(0, 0, 90));
                //RotateTile(i, duration / TileLength, time, StartAngle, EndAngle);
                //}
                //if (angle <0)
                //{
                //    print("tile not facing right");
                //    //Quaternion StartAngle = Quaternion.Euler(new Vector3(90, 90, 0));
                //    //Quaternion EndAngle = Quaternion.Euler(new Vector3(0, 0, 90));
                //    //RotateTile(i, duration / TileLength, time, StartAngle, EndAngle);
                //    Quaternion StartAngle = Quaternion.Euler(new Vector3(90, 0, -90));
                //    Quaternion EndAngle = Quaternion.Euler(new Vector3(0, 0, -90));
                //    RotateTile(i, duration / TileLength, time, StartAngle, EndAngle);
                //}

            }
        }
    }
    */

    public  void Lerp(int TileIndex, float TimePercentage)
    {

        //float startTime = TileIndex * DurationSplit;
        //float localTime = time - startTime;
        //lerp start from 90 to 0
        //print("local time is " + localTime);
        //print("time for each rotation is " + DurationSplit);
        Quaternion StartAngle = Quaternion.identity, EndAngle = Quaternion.identity;
        switch (tiles[TileIndex].TileZAngle)
        {
            case 0:
                StartAngle = Quaternion.Euler(new Vector3(0, -90, 0));
                EndAngle = Quaternion.identity;
                break;
            case 90:
                StartAngle = Quaternion.Euler(new Vector3(90, 0, 90));
                EndAngle = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case 180:
                StartAngle = Quaternion.Euler(0, 90, 180);
                EndAngle = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
            case 270:
                StartAngle = Quaternion.Euler(90, 0, 270);
                EndAngle = Quaternion.Euler(new Vector3(0, 0, 270));
                break;
            default:
                break;
        }

        print("start rotation is " + StartAngle.eulerAngles);
        print("end rotation is " + EndAngle.eulerAngles);
        Quaternion rotation = Quaternion.Lerp(StartAngle, EndAngle, TimePercentage);
        //print("lerp time for tile " + TileIndex + " is " + localTime / DurationSplit);
        Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        tmap.SetTransformMatrix(tiles[TileIndex].TilePos, m);

    }

    public static float GetSignedAngle(Quaternion A, Quaternion B, Vector3 axis)
    {
        float angle;
        Vector3 angleAxis = Vector3.zero;
        (B * Quaternion.Inverse(A)).ToAngleAxis(out angle, out angleAxis);
        if (Vector3.Angle(axis, angleAxis) > 90f)
        {
            angle = -angle;
        }
        return Mathf.DeltaAngle(0f, angle);
    }



    public void SetTilesStartRot()
    {
        if (tiles == null) return;
        if (tmap == null)
        {
            print("getting tmap reference");
            tmap = GetComponent<Tilemap>();
        }
        foreach (TileContainer.TileTransform tilepos in tiles)
        {
            tmap.SetTransformMatrix(tilepos.TilePos, SetStartRotation(tilepos.TilePos, tilepos.TileZAngle));
        }
    }

    

    Matrix4x4 SetStartRotation(Vector3Int Tilepos, float Zangle)
    {
        //go through all elements and rotate
        Quaternion rotation;
        //print("rotation for tile at pos: " + Tilepos + " is " + EulerRotation);
        if (Zangle ==0)
        {
            print("tile " + Tilepos + " facing right");
            rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        }
        if (Zangle == 90)
        {
            //print("tile " + Tilepos + " facing up");
            rotation = Quaternion.Euler(90, 0, Zangle);
            return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        }
        if (Zangle == 270)
        {
            //print("tile " + Tilepos + " facing down");
            rotation = Quaternion.Euler(90, 0, Zangle);
            return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);

        }
        if (Zangle == 180)
        {
            //print("tile " + Tilepos + " facing left");
            rotation = Quaternion.Euler(new Vector3(0, 90, Zangle));
            return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        }

        return Matrix4x4.identity;
    }

    public override void LerpOjects(int TileIndex, float TimePercentage)
    {
        //float startTime = TileIndex * DurationSplit;
        //float localTime = time - startTime;
        //lerp start from 90 to 0
        //print("local time is " + localTime);
        //print("time for each rotation is " + DurationSplit);
        Quaternion StartAngle = Quaternion.identity, EndAngle = Quaternion.identity;
        switch (tiles[TileIndex].TileZAngle)
        {
            case 0:
                StartAngle = Quaternion.Euler(new Vector3(0, -90, 0));
                EndAngle = Quaternion.identity;
                break;
            case 90:
                StartAngle = Quaternion.Euler(new Vector3(90, 0, 90));
                EndAngle = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case 180:
                StartAngle = Quaternion.Euler(0, 90, 180);
                EndAngle = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
            case 270:
                StartAngle = Quaternion.Euler(90, 0, 270);
                EndAngle = Quaternion.Euler(new Vector3(0, 0, 270));
                break;
            default:
                break;
        }

        print("start rotation is " + StartAngle.eulerAngles);
        print("end rotation is " + EndAngle.eulerAngles);
        Quaternion rotation = Quaternion.Lerp(StartAngle, EndAngle, TimePercentage);
        //print("lerp time for tile " + TileIndex + " is " + localTime / DurationSplit);
        Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        tmap.SetTransformMatrix(tiles[TileIndex].TilePos, m);

    }
}
