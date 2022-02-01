using System.Collections;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;
using topdown;
using System;




public class Slicer : MonoBehaviour
{

    public GameObject MeshToCut;
    public Vector2 MouseInputPos;
    Vector2 AimPos;
    bool IsClicked;
    public PlayerInput playerinput;
    
    bool ispaused;
    bool canSlice = true;
    public CaptureScreen cap;
    Animator sliceanimator;
    SlicedDissolveController dissolver;
    SpriteRenderer SliceAimRenderer;
    public Vector3 angularChangeInDegrees;
    public int MaxSliceAmount;
    int currentSlices;
    float timer = 0;
    public float PauseTime;
    public Material DisMat;
    delegate IEnumerator Resume(IEnumerator enumerator);
    Resume onResume;
    
    [HideInInspector]
    public player _player;

    public RenderTexture RendTex;
    public GameObject CapCam;

    BoxCollider boxcol;

    Vector2 PlayerMoveDir;
    Transform NextDoor;


    private void Awake()
    {
        dissolver = gameObject.GetComponent<SlicedDissolveController>();
        sliceanimator = GetComponentInChildren<Animator>();
        SliceAimRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void SliceInitialize(Transform nextDoor, Vector2 dir)
    {
        
        gameObject.SetActive(true);
        timer = 0;
        currentSlices = 0;
        print("pausing time");
        Time.timeScale = 0;
        ispaused = true;
        

        
        //cap.grab = true;



        DisMat.SetFloat("blend_opacity", 0);
        DisMat.SetFloat("Noise_Strength", 1);
        dissolver.dissolvemat = MeshToCut.GetComponent<MeshRenderer>().material;


        NextDoor = nextDoor;
        PlayerMoveDir = dir;
        
        
        CapCam.SetActive(true);
    }


    private void OnEnable()
    {
        StartCoroutine(EnableEffects());
    }

    // Start is called before the first frame update
    IEnumerator EnableEffects()
    {
        yield return new WaitForEndOfFrame();

        sliceanimator.gameObject.SetActive(true);
        SliceAimRenderer.enabled = true;
        MeshToCut.SetActive(true);
        MeshToCut.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", RendTex);
        DisMat.SetTexture("_MainTex", RendTex);
        CapCam.SetActive(false);

        yield return null;
    }



    // Update is called once per frame
    void Update()
    {

        //Vector2 mousedir = _player.GetAngleToMouse();
        if(_player.Attacking)
        { MouseClick(); }
        //Debug.Log("mouse dir is " + mousedir.normalized);
        float angle = _player.GetAngleToMouse();
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
    }


    public void MousePos(InputAction.CallbackContext context)
    {
        MouseInputPos = context.ReadValue<Vector2>();
        
    }

    public void MouseClick ()
    {
        
            if(canSlice && timer < Time.unscaledTime)
            DoSlice();
        
        
    }

    public void OnControlSchemeChanged()
    {
        print("controlls changed");
    }



    public void DoSlice()
    {
        print("do slice called");
        if (currentSlices == MaxSliceAmount - 1)
        {
            //add code to immediately start dissolve
            print("max slices reached");
            finishSlicing();
            canSlice = false; 
        }

        if (timer < Time.unscaledTime)
        {
            currentSlices++;
            timer = Time.unscaledTime + .2f;
            sliceanimator.SetTrigger("slice");
            print("slice successful " + currentSlices);
            
        }
        else
        {
            print("can't slice, timer is at " + timer + " current time is " + Time.unscaledTime);
            return;
        }

        
        
        Collider[] cols = Physics.OverlapBox(transform.position, Vector3.one ,Quaternion.identity);

        //print("overlap box at " + transform.position + "and bounds is " + boxcol.bounds);
        //print("mesh is at " + MeshToCut.transform.position);
        //print("cols amount is " + cols.Length);

        if (cols == null)
        {
            print("didn't colide with anything");
            currentSlices--;
            return;
        }

        foreach(Collider col in cols)
        {
            //print("slicing " + col.gameObject);
            SliceObject(col.gameObject);
           
        }
        dissolver.DarkenMaterial();
    }



    void SliceObject(GameObject obj)
    {

        if (CreateDissolvingHulls(obj))
        { 
            if (obj.CompareTag("mesh"))
            {
                obj.SetActive(false);
            }
            else
            {
                obj.gameObject.SetActive(false);
            }
    
        }
    }


    void finishSlicing()
    {
        print("resuming time");
        Time.timeScale = 1;
        canSlice = false;
        dissolver.dissolvemat = MeshToCut.GetComponent<MeshRenderer>().material; ;
        dissolver.StartDissolve();

        _player.WalkInDir(NextDoor.transform.position, PlayerMoveDir);
        StartCoroutine(movecam());
        

        
    }

    IEnumerator movecam()
    {
        yield return new WaitForSeconds(1);
        CameraManager.instance.TransferCam(NextDoor.parent.gameObject);
        gameObject.SetActive(false);
        yield return null;
    }

    void finishSlicing(Action onFinish)
    {
        finishSlicing();
        onFinish();
    }

    bool CreateDissolvingHulls(GameObject obj)
    {
        //print("attempting to create hulls for " + obj);
        EzySlice.Plane newplane = new EzySlice.Plane(transform.forward, transform.right, transform.forward * -1) ;
        
        GameObject[] hulls = SlicerExtensions.SliceInstantiate(obj, newplane);

        if (hulls != null)
        {
            foreach (GameObject go in hulls)
            {
                go.transform.position = transform.position;
                Rigidbody rb = go.AddComponent<BoxCollider>().gameObject.AddComponent<Rigidbody>();
                dissolver.GetDissolveMat(rb.gameObject.GetComponent<MeshRenderer>());
                
                rb.gameObject.GetComponent<MeshRenderer>().material = MeshToCut.GetComponent<MeshRenderer>().material;


                Vector3 centerdirection = Camera.main.transform.position - rb.transform.position;
                //rb1.AddForce(centerdirection);
                //rb1.AddTorque(centerdirection);
                rb.AddExplosionForce(1, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 5), 0), 1, 0, ForceMode.Impulse);
                //print("half screen is " + Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 5), 0));
                rb.useGravity = false;
                rb.AddTorque(centerdirection);
            }
            return true;
        }
        return false;
    }


    //first sets screen mesh active then
    //resumes time scale after 2 seconds
    IEnumerator ResumeTimeCountdown()
    {
        yield return new WaitForEndOfFrame();
        MeshToCut.GetComponent<MeshRenderer>().material = cap.ColorChangemat;
        MeshToCut.SetActive(true);
        _player.WalkInDir(NextDoor.transform.position, PlayerMoveDir);

        yield return new WaitForSecondsRealtime(PauseTime);
        print("resuming time");
        //Time.timeScale = 1;
        //canSlice = false;
        //dissolver.dissolvemat = MeshToCut.GetComponent<MeshRenderer>().sharedMaterial; ;
        dissolver.StartDissolve();


        gameObject.SetActive(false);

        yield return null;
    }





#if UNITY_EDITOR
    /**
	 * This is for Visual debugging purposes in the editor 
	 */
    public void OnDrawGizmos()
    {
        EzySlice.Plane cuttingPlane = new EzySlice.Plane();

        // the plane will be set to the same coordinates as the object that this
        // script is attached to
        // NOTE -> Debug Gizmo drawing only works if we pass the transform
        cuttingPlane.Compute(transform);

        // draw gizmos for the plane
        // NOTE -> Debug Gizmo drawing is ONLY available in editor mode. Do NOT try
        // to run this in the final build or you'll get crashes (most likey)
        cuttingPlane.OnDebugDraw();
    }

#endif

}
