using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//moves tiles and places sprites into a sprite mask
//could probably change this to just control specific tiles through timeline instead of a coroutine
namespace topdown {
    public class SpawnOnTile : MonoBehaviour, IAnimationInterpolate
    {

        [SerializeField]
        Tilemap map;
        [SerializeField]
        GameObject TatamiSpriteMask;
        [SerializeField]
        float TransitionTime, DelayBetweenSpawn;
        float CurrentTimeDelay;
        private Transform interpolateTarget;
        public GameObject InterpolateTarget { set { interpolateTarget = value.transform;  SpawnObj();} }
        Vector3Int TilePos;
        [SerializeField] Vector3 ObjectSpawnOffset;
        Vector3 objectStartingPos;



        void SpawnObj()
        {
            Vector3 pos = interpolateTarget.position;
            TilePos = map.WorldToCell(pos);
            if (map.GetTile(TilePos) == null) 
            { 
                print("no tile at " + TilePos + " from pos " + pos + " can't spawn");
                return;
            }
            print(gameObject + " spawning on tile " + TilePos);
            //StartCoroutine(TileRoutine(TilePos, TransitionTime));
            //spawn a mask
            GameObject newMask = Instantiate(TatamiSpriteMask);
            newMask.transform.position = TilePos ;
            //move Object in order to lerp it back up later
            interpolateTarget.position = TilePos + ObjectSpawnOffset;
            objectStartingPos = interpolateTarget.position;
            //StartCoroutine(SpawnRoutine(ObjectsToSpawn[index], 1));

        }


        void TileLerp(float value)
        {
            print(gameObject + " is interpolating with a value of " + value);
            float ypos = Mathf.Lerp(0, 1, value);
            Matrix4x4 mat = Matrix4x4.Translate(new Vector3(0, -SpikeValue(ypos), 0));
            map.SetTransformMatrix(TilePos, mat);
        }


        float SpikeValue(float value)
        {
            if (value <= .5f)
                return EaseOut(value / .5f);

            return EaseOut( flip(value) / .5f);
            
        }

        float EaseOut(float value)
        {
            return flip(Square(flip(value)));
        }

        float Square(float value)
        {
            return value * value;
        }

        float flip(float value)
        {
            return 1 - value;
        }

        //moves tile over time
        void TileRoutine(float SetTime)
        {
            Matrix4x4 Matpos = map.GetTransformMatrix(TilePos);
            float YStartPos = Matpos[1, 3];
            print("y start pos is " + YStartPos);
            float YEndPos = Matpos[1, 3] - 1;
            float timer = 0;
            print("moving tile " + TilePos);
            while (timer < SetTime)
            {
                float Ypos = Mathf.Lerp(YStartPos, YEndPos, timer / SetTime);
                Matrix4x4 mat = Matrix4x4.TRS(new Vector3(0, Ypos, 0), Matpos.rotation, Vector3.one);
                //print("current y pos is " + Ypos);
                map.SetTransformMatrix(TilePos, mat);
                timer += Time.deltaTime;
            }
            print("tile routine done");
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

            }
            map.SetTransformMatrix(TilePos, Matpos);

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

        //still need to add better ease
        void ObjectLerp(float value)
        {
            value = (value - .4f) / .6f;
            float ypos = Mathf.Lerp(0, 1, value);
            interpolateTarget.position = new Vector3(interpolateTarget.position.x, objectStartingPos.y + ypos);
        }

        //haven't tested yet
        //may need to use the functions from easing.net
        //or recopy but set duration variable to 1
        float ElasticEaseOut(float t, float b, float c)
        {
            if (t  == 1)
                return b + c;

            float p = .3f;
            float s = p / 4;

            return (c * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * s) * (2 * Mathf.PI) / p) + c + b);
        }



        public void Interpolate(float value)
        {
            TileLerp(value);
            ObjectLerp(value);
        }
    }
}
