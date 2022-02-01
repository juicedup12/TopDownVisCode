using UnityEngine;

public class ScreenMesh : MonoBehaviour
{
    RectTransform rect;
    Canvas canvas;
    Material mat;
    float Opacity;
    public float OpacityIncrement;
    public bool startincrement;
    float maxOpacity = 1;
    // Start is called before the first frame update
    void Start()
    {
        Opacity = 0;
        Mesh mesh = new Mesh();
        mat = GetComponent<MeshRenderer>().material;
        canvas = GetComponentInParent<Canvas>();
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        float height = Camera.main.orthographicSize ;
        float width = height * Camera.main.aspect;

        vertices[0] = new Vector3(-width, height, 0);
        vertices[1] = new Vector3(width, height, 0);
        vertices[2] = new Vector3(width, -height, 0);
        vertices[3] = new Vector3(-width, -height, 0);




        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(0, 1);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 0;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        GetComponent<MeshFilter>().mesh = mesh;
        Debug.DrawLine(vertices[0], vertices[1], Color.yellow, 10);
        Debug.DrawLine(vertices[0], vertices[2], Color.green, 10);
        Debug.DrawLine(vertices[0], vertices[3], Color.blue, 10);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Opacity != maxOpacity && startincrement)
        //{
        //    mat.SetFloat("_Opacity", Opacity);
        //    Opacity += OpacityIncrement;
        //}


    }
}
