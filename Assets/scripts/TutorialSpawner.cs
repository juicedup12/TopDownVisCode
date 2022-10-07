using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//moves tiles and places sprites into a sprite mask
public class TutorialSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] ObjectsToSpawn;
    [SerializeField]
    Tilemap map;
    [SerializeField]
    GameObject Mask;
    [SerializeField]
    float TransitionTime, DelayBetweenSpawn;
    float CurrentTimeDelay;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(TileRoutine(SpawnObj(0), 5));
        SpawnObj(0);
        SpawnObj(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObj(int index)
    {
        CurrentTimeDelay = DelayBetweenSpawn * index;
        Vector3 pos = ObjectsToSpawn[index].transform.position;
        Vector3Int TilePos = map.WorldToCell(pos);
        if (map.GetTile(TilePos) == null) print("no tile at " + TilePos);
        StartCoroutine(TileRoutine(TilePos, TransitionTime));
        //spawn a mask
        GameObject newMask = Instantiate(Mask);
        newMask.transform.position = TilePos;
        //move Object
        ObjectsToSpawn[index].transform.position = pos + Vector3.down;
        StartCoroutine(SpawnRoutine(ObjectsToSpawn[index], 1));

    }

    //moves tile over time
    IEnumerator TileRoutine(Vector3Int TilePos, float SetTime)
    {
        yield return new WaitForSeconds(CurrentTimeDelay);
        Matrix4x4 Matpos = map.GetTransformMatrix(TilePos);
        float YStartPos = Matpos[1, 3];
        print("y start pos is " + YStartPos);
        float YEndPos = Matpos[1, 3] - 1;
        float timer = 0;
        print("moving tile " + TilePos);
        while (timer < SetTime)
        {
            float Ypos = Mathf.Lerp(YStartPos, YEndPos, timer/SetTime);
            Matrix4x4 mat = Matrix4x4.TRS(new Vector3(0, Ypos, 0), Matpos.rotation, Vector3.one);
            //print("current y pos is " + Ypos);
            map.SetTransformMatrix(TilePos, mat);
            timer += Time.deltaTime;

            yield return null;
        }
        print("tile routine done");
        yield return new WaitForSeconds(1 + CurrentTimeDelay);
        //move tile back
        print("moving tile back");
        timer = 0;
        while (timer < SetTime)
        {
            float Ypos = Mathf.Lerp(YEndPos, YStartPos, timer / SetTime);
            Matrix4x4 mat = Matrix4x4.TRS(new Vector3(0, Ypos, 0), Matpos.rotation, Vector3.one);
            //print("current y pos is " + Ypos);
            map.SetTransformMatrix(TilePos, mat);
            timer += Time.deltaTime;

            yield return null;
        }
        map.SetTransformMatrix(TilePos, Matpos);
        yield return null;

    }

    //move object back to starting pos
    IEnumerator SpawnRoutine(GameObject Obj, float SetTime)
    {
        yield return new WaitForSeconds(TransitionTime + CurrentTimeDelay);
        Obj.SetActive(true);
        print("spawn routine started");

        Vector3 ObjPos = Obj.transform.position;

        print("spawn y pos is " + ObjPos.y + " end pos is  " + ObjPos.y + 1);
        float timer = 0;
        while (timer < SetTime)
        {
            float Ypos = Mathf.Lerp(ObjPos.y, ObjPos.y + 1, timer / SetTime);
            Obj.transform.position = new Vector3(ObjPos.x, Ypos);
            timer += Time.deltaTime;
            yield return null;
        }
        print("spawn routine finished");
    }
}
