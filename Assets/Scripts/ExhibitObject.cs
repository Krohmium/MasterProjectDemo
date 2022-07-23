using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMeshSimplifier;

public class ExhibitObject : MonoBehaviour
{
    public float reducedQuality = 0.1f;
    public float meshVolume = 0;
    public double[] colliderVolume = new double[3];
    public double verticeAmount = 0;
    private CapsuleCollider capColl;
    private int minCollider;

    private bool closeUpActive;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private Vector3 lastMousePosition;

    public float x, y, z;

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        CalculateMeshVolume(mesh);

        Vector3 boundssize = GetComponent<Renderer>().localBounds.size;
        x = boundssize.x;
        y = boundssize.y;
        z = boundssize.z;

        float[] xyz = new float[3] { boundssize.x, boundssize.y, boundssize.z };
        CapsuleCollider[] colids = new CapsuleCollider[3];

        for (int i = 0; i < 3; i++)
        {
            colids[i] = this.gameObject.AddComponent<CapsuleCollider>();
            colids[i].direction = i;
            colids[i].height = xyz[i];
            if (xyz[(i + 1) % 3] > xyz[(i + 2) % 3])
                colids[i].radius = xyz[(i + 1) % 3] / 2;
            else
                colids[i].radius = xyz[(i + 2) % 3] / 2;

            colliderVolume[i] = CalculateCapsuleVolume(colids[i]);
        }

        minCollider = colliderVolume.ToList().IndexOf(colliderVolume.Min());
        Destroy(colids[(minCollider + 1) % 3]);
        Destroy(colids[(minCollider + 2) % 3]);

        capColl = colids[minCollider];


        if (colliderVolume[minCollider] > 26158062)
        {
            Destroy(this.gameObject);
        }
        else if(!this.gameObject.transform.name.EndsWith("(Clone)"))
        {
            GameObject parent_ = this.gameObject.transform.parent.gameObject;
            GameObject podest_ = GameObject.Find("template/podest");
            GameObject exhibitPodest_ = GameObject.Instantiate(podest_);
            

            exhibitPodest_.transform.SetParent(parent_.transform, true);
            exhibitPodest_.transform.position = parent_.transform.position;

            if (capColl.direction == 0)
            {
                exhibitPodest_.transform.position += new Vector3(capColl.center.x / 100, 0, capColl.center.y / 100);
                exhibitPodest_.transform.position += new Vector3(0, 0.30f, 0f);

                this.gameObject.transform.position += new Vector3(0, -capColl.center.z / 100 + capColl.radius / 100f + 1.15f, 0f);

                exhibitPodest_.transform.localScale = new Vector3(-capColl.height - 20, -15f, -capColl.radius * 2 - 20);
            }

            if (capColl.direction == 1)
            {
                exhibitPodest_.transform.position += new Vector3(capColl.center.x / 100, 0, capColl.center.y / 100);
                exhibitPodest_.transform.position += new Vector3(0, 0.30f, 0f);

                this.gameObject.transform.position += new Vector3(0, -capColl.center.z / 100 + capColl.radius / 100f + 1.15f, 0f);

                exhibitPodest_.transform.localScale = new Vector3(-capColl.radius * 2 - 20, -15f, -capColl.height - 20);
            }

            if (capColl.direction == 2)
            {
                exhibitPodest_.transform.position += new Vector3(capColl.center.x / 100, 0, capColl.center.y / 100);
                exhibitPodest_.transform.position += new Vector3(0, 0.30f, 0f);

                this.gameObject.transform.position += new Vector3(0, -capColl.center.z / 100 + capColl.height / 200f + 1.15f, 0f);

                exhibitPodest_.transform.localScale = new Vector3(-capColl.radius * 2 - 20, -15f, -capColl.radius * 2 - 20);
            }
        }

    }

    // Start is called before the first frame update
    public void StartMeUp()
    {



    }

    // Update is called once per frame
    void Update()
    {
        if (closeUpActive)
        {

            float mouseX = Input.GetAxis("Mouse X") * 2.0f;
            float mouseY = Input.GetAxis("Mouse Y") * 2.0f;

            if (Input.GetMouseButton(0))
            {
                this.gameObject.transform.localPosition += Input.mousePosition-lastMousePosition;

            }
            else
            { 
                yRotation -= mouseX;
                xRotation += mouseY;

                this.gameObject.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
            }
            lastMousePosition = Input.mousePosition;
        }
    }

    public void SetForCloseup()
    {
        Vector3 boundssize = GetComponent<Renderer>().localBounds.size;
        x = boundssize.x;
        y = boundssize.y;
        z = boundssize.z;

        float[] xyz = new float[3] { boundssize.x, boundssize.y, boundssize.z };

        float scale = - Screen.height / xyz.Max(); // objects need to be inverted

        this.transform.localScale = new Vector3(scale, scale, scale);
        closeUpActive = true;
    }

    private void SimplifyMeshFilter(MeshFilter meshFilter)
    {
        Mesh sourceMesh = meshFilter.sharedMesh;
        if (sourceMesh == null) // verify that the mesh filter actually has a mesh
            return;

        // Create our mesh simplifier and setup our entire mesh in it
        var meshSimplifier = new MeshSimplifier();
        meshSimplifier.Initialize(sourceMesh);

        // This is where the magic happens, lets simplify!
        meshSimplifier.SimplifyMesh(reducedQuality);

        // Create our final mesh and apply it back to our mesh filter
        meshFilter.sharedMesh = meshSimplifier.ToMesh();
    }

    private double CalculateCapsuleVolume(CapsuleCollider collider)
    {
        return Math.Abs(Math.PI * collider.radius * collider.radius * ((4/3) * collider.radius + collider.height - collider.radius*2));
    }

    private void CalculateMeshVolume(Mesh mesh)
    {
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        verticeAmount = mesh.vertices.Length;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        meshVolume = Mathf.Abs(volume);
    }

    private float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var v321 = p3.x * p2.y * p1.z;
        var v231 = p2.x * p3.y * p1.z;
        var v312 = p3.x * p1.y * p2.z;
        var v132 = p1.x * p3.y * p2.z;
        var v213 = p2.x * p1.y * p3.z;
        var v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }
}
