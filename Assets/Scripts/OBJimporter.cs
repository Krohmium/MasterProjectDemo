using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using UnityEditor;

public class OBJimporter : MonoBehaviour
{


    string objPath = string.Empty;
    string objPath_material = string.Empty;
    string error = string.Empty;
    GameObject loadedObject;
    GameObject loadedObject3;
    Object loadedObject2;





    // Start is called before the first frame update
    void Start()
    {
        objPath = "C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\figure-holding-the-hraschina-meteorite\\Karyatide_Hraschina_Saal4_EDIT_VW_lowres.obj";
        objPath_material = "C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\figure-holding-the-hraschina-meteorite\\Karyatide_Hraschina_Saal4_EDIT_VW_lowres.mtl";




        if (loadedObject != null)
            Destroy(loadedObject);
        loadedObject = new OBJLoader().Load(objPath, objPath_material);


        loadedObject.transform.localScale = new Vector3(-0.01f, -0.01f, -0.01f);
        loadedObject.transform.position = new Vector3(0.0f, 3.75f, 40.0f);
        loadedObject.transform.Rotate(new Vector3(90.0f, 180.0f, 0.0f));

        loadedObject.GetComponentInChildren<MeshRenderer>().material.shader = Shader.Find("Standard");
        loadedObject.GetComponentInChildren<MeshRenderer>().material.SetFloat("_Glossiness", 0.0f);


        //Destroy(loadedObject.GetComponent<MeshRenderer>());
        //loadedObject.GetComponentInChildren<MeshRenderer>().material = "defaultMat"; 




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
