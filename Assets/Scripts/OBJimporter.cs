using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using UnityEditor;
using System.IO;

public class OBJimporter : MonoBehaviour
{

    string objPath = string.Empty;
    string objPath_material = string.Empty;
    string extension = string.Empty;

    string error = string.Empty;
    GameObject loadedObject;
    List<GameObject> loadedObjects = new List<GameObject>();





    // Start is called before the first frame update
    void Start()
    {
        int counter = 0;
        objPath = "C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\figure-holding-the-hraschina-meteorite\\Karyatide_Hraschina_Saal4_EDIT_VW_lowres.obj";
        objPath_material = "C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\figure-holding-the-hraschina-meteorite\\Karyatide_Hraschina_Saal4_EDIT_VW_lowres.mtl";

        foreach (string directory in System.IO.Directory.GetDirectories("C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\"))
        {
            counter++;
            foreach(string file in System.IO.Directory.GetFiles(directory))
            {
                extension = file.Split(".")[1];

                if (extension == "obj")
                    objPath = file;
                if (extension == "mtl")
                    objPath_material = file;
            }

            loadedObject = new OBJLoader().Load(objPath, objPath_material);


            loadedObject.transform.localScale = new Vector3(-0.01f, -0.01f, -0.01f);
            loadedObject.transform.position = new Vector3(-30.0f + counter * 10.0f, 8.75f, 20.0f);
            loadedObject.transform.Rotate(new Vector3(90.0f, 180.0f, 0.0f));

            loadedObject.GetComponentInChildren<MeshRenderer>().material.shader = Shader.Find("Standard");
            loadedObject.GetComponentInChildren<MeshRenderer>().material.SetFloat("_Glossiness", 0.0f);

            if (counter > 5)
                break;
        }


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
