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
    BoxCollider tempCollider;
    List<GameObject> loadedObjects = new List<GameObject>();
    protected bool PlayerInRange;

    int counter = 0;
    int row = 0;



    // Start is called before the first frame update
    void Start()
    {
        objPath = "C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\figure-holding-the-hraschina-meteorite\\Karyatide_Hraschina_Saal4_EDIT_VW_lowres.obj";
        objPath_material = "C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\figure-holding-the-hraschina-meteorite\\Karyatide_Hraschina_Saal4_EDIT_VW_lowres.mtl";

        foreach (string directory in System.IO.Directory.GetDirectories("C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\"))
        {
            if (counter > 20)
                break;

            foreach (string file in System.IO.Directory.GetFiles(directory))
            {
                extension = file.Split(".")[1];

                if (extension == "obj")
                    objPath = file;
                if (extension == "mtl")
                    objPath_material = file;
            }

            loadedObject = new OBJLoader().Load(objPath, objPath_material);


            loadedObject.transform.localScale = new Vector3(-0.01f, -0.01f, -0.01f);
            loadedObject.transform.position = new Vector3(-40.0f + counter % 8 * 10.0f, 8.75f, 10.0f + counter/8 * 10.0f);
            loadedObject.transform.Rotate(new Vector3(90.0f, 180.0f, 0.0f));

            //loadedObject.GetComponentInChildren<MeshRenderer>().material.shader = Shader.Find("Standard");
            //loadedObject.GetComponentInChildren<MeshRenderer>().material.SetFloat("_Glossiness", 0.0f);

            GameObject childGameObject;
            MeshRenderer childGameObjectRenderer;
            for (int child = 0; child  < loadedObject.transform.childCount; child++)
            {
                childGameObject = loadedObject.transform.GetChild(child).gameObject;
                childGameObjectRenderer = childGameObject.GetComponent<MeshRenderer>();
                childGameObjectRenderer.material.shader = Shader.Find("Standard");
                childGameObjectRenderer.material.SetFloat("_Glossiness", 0.0f);

                tempCollider = childGameObject.AddComponent<BoxCollider>();
                //tempCollider.convex = true;
                childGameObject.AddComponent<Rigidbody>();
            }


            counter++;
            if (counter % 8 == 0)
            {
                row++;
            }
        }


        //if (loadedObject != null)
        //    Destroy(loadedObject);
        //loadedObject = new OBJLoader().Load(objPath, objPath_material);


        //loadedObject.transform.localScale = new Vector3(-0.01f, -0.01f, -0.01f);
        //loadedObject.transform.position = new Vector3(0.0f, 3.75f, 40.0f);
        //loadedObject.transform.Rotate(new Vector3(90.0f, 180.0f, 0.0f));

        //loadedObject.GetComponentInChildren<MeshRenderer>().material.shader = Shader.Find("Standard");
        //loadedObject.GetComponentInChildren<MeshRenderer>().material.SetFloat("_Glossiness", 0.0f);


        //Destroy(loadedObject.GetComponent<MeshRenderer>());
        //loadedObject.GetComponentInChildren<MeshRenderer>().material = "defaultMat"; 




    }




    protected void Update()
    {
        if (PlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("entering Loading new Object");
            int i = 0;
            foreach (string directory in System.IO.Directory.GetDirectories("C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\"))
            {
                if (i < counter)
                {
                    Debug.Log("inc i");
                    i++;
                    continue;
                }


                Debug.Log("Loading new Object");
                foreach (string file in System.IO.Directory.GetFiles(directory))
                {
                    extension = file.Split(".")[1];

                    if (extension == "obj")
                        objPath = file;
                    if (extension == "mtl")
                        objPath_material = file;
                }

                loadedObject = new OBJLoader().Load(objPath, objPath_material);


                loadedObject.transform.localScale = new Vector3(-0.01f, -0.01f, -0.01f);
                loadedObject.transform.position = new Vector3(-40.0f + counter%8 * 10.0f, 8.75f, 10.0f + counter/8 * 10.0f);
                loadedObject.transform.Rotate(new Vector3(90.0f, 180.0f, 0.0f));

                //loadedObject.GetComponentInChildren<MeshRenderer>().material.shader = Shader.Find("Standard");
                //loadedObject.GetComponentInChildren<MeshRenderer>().material.SetFloat("_Glossiness", 0.0f);

                GameObject childGameObject;
                MeshRenderer childGameObjectRenderer;
                for (int child = 0; child < loadedObject.transform.childCount; child++)
                {
                    childGameObject = loadedObject.transform.GetChild(child).gameObject;
                    childGameObjectRenderer = childGameObject.GetComponent<MeshRenderer>();
                    childGameObjectRenderer.material.shader = Shader.Find("Standard");
                    childGameObjectRenderer.material.SetFloat("_Glossiness", 0.0f);

                    tempCollider = childGameObject.AddComponent<BoxCollider>();
                    childGameObject.AddComponent<Rigidbody>();
                }



                if (counter == i)
                {   
                    if (counter%8==0)
                    {
                        row++;
                    }
                    Debug.Log("inc counter and break");
                    counter++;
                    break;
                }
            }
        }
    }

    protected void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
    }

    protected void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }


}
