using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMeshSimplifier;

public class ExhibitObject : MonoBehaviour
{
    public float reducedQuality = 0.1f;
    public float meshVolume = 0;
    public double colliderVolume = 0;
    public double verticeAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        CalculateMeshVolume(mesh);

        CapsuleCollider capColl = GetComponent<CapsuleCollider>();
        if (capColl.radius *2 > capColl.height)
        {
            float temp = capColl.radius;
            capColl.radius = capColl.height / 2;
            capColl.height = temp * 2;
            capColl.direction = 2;
        }
        CalculateCapsuleVolume(capColl);

        //if (meshVolume > 3.363327e+07)
        if (colliderVolume > 26158062)
        {
            Destroy(this.gameObject);
            //GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            GameObject podest_ = GameObject.Find("template/podest");
            GameObject exhibitPodest_ = GameObject.Instantiate(podest_);
            GameObject parent_ = this.gameObject.transform.parent.gameObject;
            parent_.transform.position = new Vector3(parent_.transform.position.x, 0, parent_.transform.position.z + 20f);

            exhibitPodest_.transform.SetParent(parent_.transform, true);
            exhibitPodest_.transform.position = parent_.transform.position;

            if (capColl.direction == 2)
            {
                exhibitPodest_.transform.position += new Vector3(capColl.center.x / 100, 0, capColl.center.y / 100);
                exhibitPodest_.transform.position += new Vector3(0, 0.30f, 0f);

                this.gameObject.transform.position += new Vector3(0, -capColl.center.z / 100 + capColl.height / 200f + 1.15f, 0f);

                if (capColl.radius * 2 > 80)
                    exhibitPodest_.transform.localScale = new Vector3(-capColl.radius * 2-20, -15f, -capColl.radius * 2-20);

            }

            //GetComponent<Renderer>().material.color = Color.red;
        }

        SimplifyMeshFilter(GetComponent<MeshFilter>());

    }

    // Update is called once per frame
    void Update()
    {
        
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


    private void CalculateCapsuleVolume(CapsuleCollider collider)
    {
        colliderVolume = Math.PI * collider.radius * collider.radius * ((4/3) * collider.radius + collider.height - collider.radius*2);
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
