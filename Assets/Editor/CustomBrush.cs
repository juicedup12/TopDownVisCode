using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Tilemaps;
using UnityEditor;
using UnityEngine.Tilemaps;

#if (UNITY_EDITOR) 
[CustomGridBrush(false, false, false, "Paint Count")]
public class CustomBrush : GridBrush
{
    public int PaintTimes = 0;
    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        //PaintTimes++;
        //base.Paint(gridLayout, brushTarget, position);
        //TileRotateController trc = brushTarget.GetComponent<TileRotateController>();
        //trc.RegisterTile(position, PaintTimes);
    }
    [MenuItem("Assets/Create/Custom Brush")]
    public static void CreateBrush()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save custom Brush", "New custom Brush", "Asset", "Save custom Brush", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CustomBrush>(), path);
    }

    public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
        if (brushTarget == null)
            return;

        Tilemap map = brushTarget.GetComponent<Tilemap>();
        if (map == null)
            return;
        Vector3Int[] locations = new Vector3Int[position.size.x * position.size.y];
        //Debug.Log("creating an array, size of " + locations.Length);
        TileBase[] tiles = new TileBase[position.size.x * position.size.y];
        foreach (Vector3Int location in position.allPositionsWithin)
        {
            Vector3Int local = location - position.min;
            Debug.Log("max is " + position.max);
            Debug.Log("min is " + position.min);
            Debug.Log("max minus min is " + (position.max - position.min));
            Debug.Log("min minus max is " + (position.min - position.max));
            //check if size x is is less than one and size y is bigger than one
            if (position.size.x == 1 && position.size.y > 1)
            {
                Debug.Log("box fill length wise");
                //set tiles if they are on even Y axis 
                if (local.y % 2 == 0)
                {
                    map.SetTile(location, cells[0].tile);

                    Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 90 * Mathf.Sign(local.y))), Vector3.one);
                    map.SetTransformMatrix(location, mat);
                    PaintTimes++;
                    TileRotateController trc = brushTarget.GetComponent<TileRotateController>();
                    trc.RegisterTile(location, PaintTimes);
                }
            }
            if(position.size.x > 1 || position.x < 1 && position.size.y == 1)
            {
                Debug.Log("box fill width wise");
                if (local.x % 2 == 0)
                {
                    map.SetTile(location, cells[0].tile);
                    //set rotation by size x's sign

                    Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Sign(position.size.x) + 1)), Vector3.one);
                    //map.SetTransformMatrix(location, mat);
                    PaintTimes++;
                    TileRotateController trc = brushTarget.GetComponent<TileRotateController>();
                    trc.RegisterTile(location, PaintTimes);
                }
            }
        }

    }


}

[CustomEditor(typeof(CustomBrush))]
public class CustomBrushCountEditor : GridBrushEditor
{

}
#endif