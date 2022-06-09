using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitObject : MonoBehaviour
{
    public bool isFlat = false;
    public bool isLong = false;
    public bool isSmall = false;
    public Vector3 sortedObjectSize;
    public Vector3 farthestPoint_;
    public Vector3 closestPoint_;
    public Vector3 firstPoint_;
    public Vector3 center_;
    public Vector3 center2_;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 objectSize = GetComponent<MeshFilter>().mesh.bounds.size;
        Debug.Log("My size is: " + objectSize);

        sortedObjectSize = SortVec3(objectSize).normalized;

        Debug.Log("my sorted size is: " + sortedObjectSize);

        if (sortedObjectSize.x * 15 < sortedObjectSize.y * sortedObjectSize.z)
        {
            Debug.LogWarning("and I'm flat");
            isFlat = true;
        }
        else
            Debug.LogWarning("and I'm not flat");
        //center_ = transform.TransformPoint(GetComponent<MeshFilter>().mesh.bounds.center);
        //center2_ = transform.TransformPoint(new Vector3(0,0,0));
        //(closestPoint_, farthestPoint_) = GetSignificantVec3s(GetComponent<MeshFilter>().mesh.vertices, center_);
        //closestPoint_ = transform.TransformPoint(closestPoint_);
        //farthestPoint_ = transform.TransformPoint(farthestPoint_);
        //firstPoint_ = transform.TransformPoint(GetComponent<MeshFilter>().mesh.vertices[0]);
        //Debug.LogWarning((closestPoint_, farthestPoint_));
        //GetFaceSizes(GetComponent<MeshFilter>().mesh);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDrawGizmos()
    {

        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(center_, farthestPoint_);
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(center_, closestPoint_);
        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(center_, firstPoint_);
    }

    private (Vector3, Vector3) GetSignificantVec3s(Vector3[] vertices, Vector3 center)
    {
        Vector3 closestPoint = vertices[0], farthestPoint = vertices[0];
        float distance = (center - vertices[0]).magnitude;
        float closestDistance = Mathf.Infinity, farthestDistance = distance;
        
        foreach (Vector3 point in vertices)
        {
            distance = (center- point).magnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
            if (distance > farthestDistance)
            {
                farthestDistance = distance;
                farthestPoint = point;
            }
        }


        return (closestPoint, farthestPoint);
    }

    private double[] GetFaceSizes(Mesh mesh)
    {
        double[] faceSizes = new double[mesh.triangles.Length/3];
        double area;
        for (int i = 0; i < mesh.triangles.Length + 3; i += 3)
        {
            area = Vector3.Cross(mesh.vertices[mesh.triangles[i]] - mesh.vertices[mesh.triangles[i + 1]], mesh.vertices[mesh.triangles[i]] - mesh.vertices[mesh.triangles[i + 2]]).magnitude * 0.5;
            faceSizes[i / 3] = area;
        }

        return faceSizes;
    }

    private Vector3 SortVec3(Vector3 toSort)
    {
        Vector3 sorted = toSort;
        if (sorted.x > sorted.y)
        {
            sorted.x = toSort.y;
            sorted.y = toSort.x;
        }

        if (sorted.y > sorted.z)
        {
            sorted.z = sorted.y;
            sorted.y = toSort.z;
        }

        if (sorted.x > sorted.y)
        {
            float temp = sorted.x;
            sorted.x = sorted.y;
            sorted.y = temp;
        }

        return sorted;
    }
}
