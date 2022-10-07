using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Tilemaps;
using UnityEditor;

#if (UNITY_EDITOR) 
[CustomGridBrush(true, false, false, "Custom line Brush")]
public class CustomLineBrush : GridBrush
{
    public bool lineStartActive = false;
    public Vector3Int lineStart = Vector3Int.zero;
    [SerializeField]
    int PaintTimes = 0;
    [SerializeField] TileDataHandlerSO TileDataSO;
    [SerializeField] int TileSize;
    [SerializeField] bool DoPaint;


    public override void Select(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
        Debug.Log("line start active : " + lineStartActive);
        if (lineStartActive)
        {
            Vector2Int startPos = new Vector2Int(lineStart.x, lineStart.y);
            Vector2Int endPos = new Vector2Int(position.x, position.y);
            Debug.Log("making line from " + startPos + " to " + endPos);
        }
        else
        {
            Debug.Log("setting start pos" + position);
            lineStart = position.position;
        }
    }

    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        Tilemap map = brushTarget.GetComponent<Tilemap>();
        if (map == null)
            return;
        if (lineStartActive)
        {
            Vector2Int startPos = new Vector2Int(lineStart.x, lineStart.y);
            Vector2Int endPos = new Vector2Int(position.x, position.y);
            if (startPos == endPos)
                RegisterTile(grid, brushTarget, position);
            else
            {
                //ChangeRotation(startPos, endPos);
                foreach (var point in GetPointsOnLine(startPos, endPos))
                {
                    Vector3Int paintPos = new Vector3Int(point.x, point.y, position.z);
                    Debug.Log("Line returned position: " + paintPos);

                    //base.Paint(grid, brushTarget, paintPos);
                    //PaintTimes++;
                    //ITileCollection TC = brushTarget.GetComponent<ITileCollection>();
                    //TC.RegisterTile(new Vector3Int(point.x, point.y, position.z), PaintTimes);
                    RegisterTile(grid, brushTarget, paintPos);

                }

            }
            lineStartActive = false;
        }
        else
        {
            lineStart = position;
            lineStartActive = true;
        }
    }
    [MenuItem("Assets/Create/Line Brush")]
    public static void CreateBrush()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Line Brush", "New Line Brush", "Asset", "Save Line Brush", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CustomLineBrush>(), path);
    }

    public void ChangeRotation(Vector2Int p1, Vector2Int p2)
    {
        Vector2Int dir = p2 - p1;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (Mathf.Abs(angle) < 30)
        {
            //change matrix
            cells[0].matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, 0), Vector3.one);
        }
        if (angle > 30 && angle < 135)
        {
            //change matrix
            cells[0].matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, 90), Vector3.one);
        }
        if (angle < -30 && angle > -135)
        {
            //change matrix
            cells[0].matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, -90), Vector3.one);
        }
        if (Mathf.Abs(angle) > 135)
        {
            //Debug.Log("facing left");
            cells[0].matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, 180), Vector3.one);
        }
    }

    void RegisterTile(GridLayout grid, GameObject brushTarget, Vector3Int Point)
    {
        if (DoPaint)
        {
            base.Paint(grid, brushTarget, Point);
        }
        PaintTimes++;
        //TC replaced with TileDataHandlerSO
        //ITileCollection TC = brushTarget.GetComponent<ITileCollection>();
        float Zangle = cells[0].matrix.rotation.eulerAngles.z;
        //TC.RegisterTile(new Vector3Int(Point.x, Point.y, Point.z), Zangle, PaintTimes);
        TileDataSO.SetTileData(new Vector3Int(Point.x, Point.y, Point.z), Zangle);
    }

    Vector2Int AddPoints(int i, Vector2Int p1)
    {
        
        return (p1 + new Vector2Int(i, 0));
        
        
    }

     bool FitsTile(int i)
    {
        if (i % TileSize == 0)
        {
            return true;
        }

            return false;
    }

    public  List<Vector2Int> GetPointsOnLine(Vector2Int p1, Vector2Int p2)
    {
        Vector2Int dir = p2 - p1;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        List<Vector2Int> points = new List<Vector2Int>();
        //Debug.Log("angle from " + p1 + " to " + " p2 " + p2 + " is " + angle);
        if (Mathf.Abs(angle) < 30)
        {
            for (int i = 0; i < dir.x; i++)
            {
                //points[i] = p1 + new Vector2Int(i, 0);
                if (FitsTile(i))
                {
                    points.Add(p1 + new Vector2Int(i, 0));
                    //Debug.Log("painting at " + (p1 + new Vector2Int(i, 0)));
                }
            }
            return points;
        }
        if (Mathf.Abs(angle) > 135)
        {

            for (int i = 0; i < -dir.x; i++)
            {
                if (FitsTile(i))
                {
                    points.Add(p1 - new Vector2Int(i, 0));
                    //Debug.Log("painting at " + (p1 - new Vector2Int(i, 0)));
                }
            }
            return points;
        }
        if (angle > 30 && angle < 135)
        {
            for (int i = 0; i < dir.y; i++)
            {
                if (FitsTile(i))
                {
                    points.Add( p1 + new Vector2Int(0, i));
                    //Debug.Log("painting at " + (p1 + new Vector2Int(0, i)));
                }
            }
            return points;
        }
        if (angle < -30 && angle > -135)
        {
            for (int i = 0; i < -dir.y; i++)
            {
                if (FitsTile(i))
                {
                    points.Add(p1 - new Vector2Int(0, i));
                    //Debug.Log("painting at " + (p1 - new Vector2Int(0, i)));
                }
            }
            return points;
        }

        points = new List<Vector2Int>() { Vector2Int.zero };
        Debug.Log("error painting error at 0");
        return points;
    }
}
[CustomEditor(typeof(CustomLineBrush))]
public class LineBrushEditor : GridBrushEditor
{
    private CustomLineBrush lineBrush { get { return target as CustomLineBrush; } }
    Tilemap tmap;
    public override void OnPaintSceneGUI(GridLayout grid, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing)
    {
        base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);
        if (lineBrush.lineStartActive && tool == GridBrushBase.Tool.Paint)
        {
            Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
            if (tmap == null && tilemap != null)
            {
                tmap = tilemap;
            }
            if(tilemap)
                 tilemap.ClearAllEditorPreviewTiles();
            // Draw preview tiles for tilemap
            Vector2Int startPos = new Vector2Int(lineBrush.lineStart.x, lineBrush.lineStart.y);
            Vector2Int endPos = new Vector2Int(position.x, position.y);
            lineBrush.ChangeRotation(startPos, endPos);
            if (startPos == endPos)
                PaintPreview(grid, brushTarget, position.min);
            else
            {
                foreach (var point in lineBrush.GetPointsOnLine(startPos, endPos))
                {
                    Vector3Int paintPos = new Vector3Int(point.x, point.y, position.z);
                    PaintPreview(grid, brushTarget, paintPos);
                }
            }
            if (Event.current.type == EventType.Repaint)
            {
                var min = lineBrush.lineStart;
                var max = lineBrush.lineStart + position.size;
                // Draws a box on the picked starting position
                GL.PushMatrix();
                GL.MultMatrix(GUI.matrix);
                GL.Begin(GL.LINES);
                Handles.color = Color.blue;
                Handles.DrawLine(new Vector3(min.x, min.y, min.z), new Vector3(max.x, min.y, min.z));
                Handles.DrawLine(new Vector3(max.x, min.y, min.z), new Vector3(max.x, max.y, min.z));
                Handles.DrawLine(new Vector3(max.x, max.y, min.z), new Vector3(min.x, max.y, min.z));
                Handles.DrawLine(new Vector3(min.x, max.y, min.z), new Vector3(min.x, min.y, min.z));
                GL.End();
                GL.PopMatrix();
            }
        }
    }

    public override void OnToolActivated(GridBrushBase.Tool tool)
    {
        //base.OnToolActivated(tool);
        lineBrush.lineStartActive = false;
        //base.ClearPreview();
        Debug.Log(tool + " activated");
        //ClearPreview();
        if(tmap)
        tmap.ClearAllEditorPreviewTiles();
    }

    public override void OnToolDeactivated(GridBrushBase.Tool tool)
    {
        //base.OnToolDeactivated(tool);
        Debug.Log(tool + " deactivated");
        lineBrush.lineStartActive = false;
        //base.ClearPreview();
        //ClearPreview();
        if(tmap)
        tmap.ClearAllEditorPreviewTiles();
    }
}

#endif