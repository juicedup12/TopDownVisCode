using System.Collections;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;
using topdown;
using System;




public class Slicer : MonoBehaviour
{

    public GameObject MeshToCut;
    [SerializeField] Transform HullParent;
    [SerializeField] ParticlePool particlePool;
    public Vector2 MouseInputPos;
    Vector2 AimPos;
    bool IsClicked;
    public PlayerInput playerinput;
    float MouseAngle;
    bool ispaused;
    bool canSlice = true;
    public CaptureScreen cap;
    Animator sliceanimator;
    SlicedDissolveController dissolver;
    SpriteRenderer SliceAimRenderer;
    public Vector3 angularChangeInDegrees;
    [SerializeField] Transform SliceAimTransform;
    public int MaxSliceAmount;
    int currentSlices;
    float timer = 0;
    public float PauseTime;
    public Material DisMat;
    delegate IEnumerator Resume(IEnumerator enumerator);
    [SerializeField] float HullRotationStrength;
    Resume onResume;
    [SerializeField] Material ParticleMat;
    [HideInInspector]
    public player _player;
    public float TestAngle;

    public RenderTexture RendTex;
    public GameObject CapCam;

    BoxCollider boxcol;
    ISliceTextureRetriever textureRetriever;
    Vector2 PlayerMoveDir;
    Transform NextDoor;


    private void Awake()
    {
        //dissolver = gameObject.GetComponent<SlicedDissolveController>();
        ////DisMat = dissolver.dissolvemat;
        sliceanimator = GetComponentInChildren<Animator>();
        SliceAimRenderer = GetComponentInChildren<SpriteRenderer>();
        dissolver = gameObject.GetComponent<SlicedDissolveController>();
        //ISliceTextureRetriever textureRetriever = GetComponent<ISliceTextureRetriever>();
        //DisMat.mainTexture = textureRetriever.RetrieveTex;
        //textureRetriever.OnCapture = () => SliceAimRenderer.enabled = true;
        //SliceInitialize();
        textureRetriever = GetComponent<ISliceTextureRetriever>();

        textureRetriever.OnCapture = () => SliceAimRenderer.enabled = true;
    }


    //need to change this to coroutine
    public void SliceInitialize()
    {
        enabled = true;
        MeshToCut.SetActive(true);

        DisMat = MeshToCut.GetComponent<MeshRenderer>().sharedMaterial;
        DisMat.mainTexture = textureRetriever.RetrieveTex;
        SliceAimRenderer.enabled = true;
        //this can be handled by the script that calls slice
        //gameObject.SetActive(true);
        DisMat.SetFloat("blend_opacity", 1);
        DisMat.SetFloat("Noise_Strength", 1);
        timer = 0;
        currentSlices = 0;
        print("pausing time");
        Time.timeScale = 0;
        ispaused = true;
        canSlice = true;
        dissolver.BlendAmount = 0;


        //cap.grab = true;



        DisMat.SetFloat("blend_opacity", 0);
        DisMat.SetFloat("Noise_Strength", 1);
        //dissolver.dissolvemat = MeshToCut.GetComponent<MeshRenderer>().material;

        //get rid of this
        //NextDoor = nextDoor;
        //PlayerMoveDir = dir;
        
        
        //CapCam.SetActive(true);
    }


    private void OnEnable()
    {
        //StartCoroutine(EnableEffects());
    }

    //disables render texture cam
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

        //Vector2 mousedir =
        //if(_player.Attacking)
        //{ MouseClick(); }

        Vector3 relative =  transform.position - Camera.main.ScreenToWorldPoint(new Vector3(MouseInputPos.x, MouseInputPos.y, 2.77f));
        //print("screen to world point: " + Camera.main.ScreenToWorldPoint(MouseInputPos));
        //print("relative is " + relative);
        //relative.Normalize();
        MouseAngle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        //Debug.Log("mouse dir is " + relative.normalized);
        //print("mouseangle is " + MouseAngle);
        SliceAimTransform.rotation = Quaternion.AngleAxis(MouseAngle, transform.forward *-1 );
        
    }


    public void MousePos(InputAction.CallbackContext context)
    {
        MouseInputPos = context.ReadValue<Vector2>();
        
    }

    public void MouseClick ()
    {
        print("can slice is " + canSlice);
        print("timer is " + timer);
            if(canSlice && timer < Time.unscaledTime)
            DoSlice();
        
        
    }

    public void OnControlSchemeChanged()
    {
        print("controlls changed");
    }



    void DoSlice()
    {
        print("do slice called");
       
        //timer's value is based on the time when the action was called
        if (timer < Time.unscaledTime)
        {
            currentSlices++;
            //timer will be set to .2 seconds after current time while game is paused
            timer = Time.unscaledTime + .2f;
            sliceanimator.SetTrigger("slice");
            //print("slice successful " + currentSlices);
            
        }
        else
        {
            print("can't slice, timer is at " + timer + " current time is " + Time.unscaledTime);
            return;
        }


        //Collider[] cols = Physics.OverlapBox(transform.position, Vector3.one * 5 ,Quaternion.identity);

        //print("overlap box at " + transform.position );
        //print("mesh is at " + MeshToCut.transform.position);
        //print("cols amount is " + cols.Length);

        //if (cols.Length == 0)
        //{
        //    print("didn't colide with anything");
        //    currentSlices--;
        //    return;
        //}

        //foreach(Collider col in cols)
        //{
        //    print("slicing " + col.gameObject);
        //    SliceObject(col.gameObject);
           
        //}

        //if no slices have happened
        //slice the screenshot mesh
        //else
        //create dissolving hulls on all objects in children of hullparent
        //if hull created successful
        //destroy that game object

        if(currentSlices == 1)
        {
            print("created hull: " + CreateDissolvingHulls(MeshToCut));
        }
        else
        {
            foreach (Transform transform in HullParent)
            {
                if (CreateDissolvingHulls(transform.gameObject))
                {
                    Destroy(transform.gameObject);
                }
            }
        }

        dissolver.DarkenMaterial(MeshToCut.GetComponent<MeshRenderer>().sharedMaterial);
        if (currentSlices == MaxSliceAmount - 1)
        {
            //add code to immediately start dissolve
            print("max slices reached");
            finishSlicing();
            canSlice = false;
        }

    }



    void SliceObject(GameObject obj)
    {
        if (CreateDissolvingHulls(obj))
        {
            print("hulls created");
            if (obj.CompareTag("mesh"))
            {
                obj.SetActive(false);
            }
            else
            {
                //obj.gameObject.SetActive(false);
                Destroy(obj);
            }
    
        }
        print("no hulls created");
    }


    //player functions will be called in timeline
    void finishSlicing()
    {
        print("resuming time");
        Time.timeScale = 1;
        canSlice = false;
        dissolver.dissolvemat = MeshToCut.GetComponent<MeshRenderer>().material;
        //destroy mesh when dissolve finishes
        dissolver.StartDissolve(MeshToCut.GetComponent<MeshRenderer>().sharedMaterial, () => { DestroyHulls();});
        SliceAimRenderer.enabled = false;
        MeshToCut.SetActive(false);
        enabled = false;
        //_player.WalkInDir(NextDoor.transform.position, PlayerMoveDir);
        //StartCoroutine(movecam());
        foreach (Transform child in HullParent)
        {
            if(child.gameObject.activeSelf)
            {
                if (!particlePool) return;
                particlePool.RetrieveParticle(child);
                //ParticleSystem particle = child.gameObject.AddComponent<ParticleSystem>();
                
                //ParticleSystemRenderer particleSystemRenderer = particle.GetComponent<ParticleSystemRenderer>();
                //particleSystemRenderer.material = ParticleMat;
                //var sh = particle.shape;
                //sh.shapeType = ParticleSystemShapeType.MeshRenderer;
                //sh.meshRenderer = child.GetComponent<MeshRenderer>();
                //sh.useMeshColors = false;
                //ParticleSystem.MainModule main = particle.main;
                //main.simulationSpace = ParticleSystemSimulationSpace.World;
                //particle.Stop();
                //ParticleSystem.EmissionModule emission = particle.emission;
                //emission.rateOverTime = 40;
                //main.startRotation3D = true;
                //main.startLifetime = .5f;
                //main.duration = 1;
                //main.startDelay = 1;
                //main.startSpeed = 0;
                //particle.Play();
            }
            else
            {
                Destroy(child.gameObject);
            }
        }
        //GetComponent<bullet>().enabled = true;
        
    }

    void DestroyHulls()
    {
        print("destroying hulls");
        foreach (Transform child in HullParent)
        {
            Destroy(child.gameObject);

        }
    }
        

    void AddParticles()
    {

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
        print("attempting to create hulls for " + obj);
        EzySlice.Plane newplane = new EzySlice.Plane(obj.transform.position, Quaternion.AngleAxis(MouseAngle, Vector3.forward) * Vector3.up);

        //GameObject[] hulls = SlicerExtensions.SliceInstantiate(obj, newplane);
        GameObject[] hulls = SlicerExtensions.SliceInstantiate(obj, obj.transform.position, Quaternion.AngleAxis(MouseAngle, Vector3.forward) * Vector3.up);

        if (hulls != null)
        {
            foreach (GameObject go in hulls)
            {
                go.transform.position = HullParent.position;
                go.transform.rotation = obj.transform.rotation;
                go.transform.parent = HullParent;
                Rigidbody rb = go.AddComponent<BoxCollider>().gameObject.AddComponent<Rigidbody>();
                //dissolver.GetDissolveMat(rb.gameObject.GetComponent<MeshRenderer>());
                
                rb.gameObject.GetComponent<MeshRenderer>().sortingOrder = 11;


                Vector3 centerdirection = Camera.main.transform.position - rb.transform.position;
                //rb1.AddForce(centerdirection);
                //rb1.AddTorque(centerdirection);
                rb.AddExplosionForce(15, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 5), 0), 1, 0, ForceMode.Impulse);
                //print("half screen is " + Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 5), 0));
                rb.useGravity = false;
                rb.AddTorque(centerdirection * HullRotationStrength);
                //rb.angularVelocity = UnityEngine.Random.insideUnitSphere * HullRotationStrength;
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
        _player.SetPosAndWalkDir(NextDoor.transform.position, PlayerMoveDir);

        yield return new WaitForSecondsRealtime(PauseTime);
        print("resuming time");
        //Time.timeScale = 1;
        //canSlice = false;
        //dissolver.dissolvemat = MeshToCut.GetComponent<MeshRenderer>().sharedMaterial; ;
        //dissolver.StartDissolve(DisMat);


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


public interface ISliceTextureRetriever
{
    Texture2D RetrieveTex { get; }
    Action OnCapture
    {
        set;
    }
}
