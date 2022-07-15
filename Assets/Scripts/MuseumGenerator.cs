using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using UnityEditor;
using System.IO;

public class MuseumGenerator : MonoBehaviour
{
    public UIController UI;

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

    int[] skiplist = new int[16] {0,3,4,5,6,7,10,11,15,16,17,19,20,21,29,30 }; 



    // Start is called before the first frame update
    void Start()
    {
        objPath = "C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\figure-holding-the-hraschina-meteorite\\Karyatide_Hraschina_Saal4_EDIT_VW_lowres.obj";
        objPath_material = "C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\figure-holding-the-hraschina-meteorite\\Karyatide_Hraschina_Saal4_EDIT_VW_lowres.mtl";
        UI.progressMax = 30;
        foreach (string directory in System.IO.Directory.GetDirectories("C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\"))
        {
            UI.progressCurrent = counter;
            if (counter > 2)
                break;

            foreach (string file in System.IO.Directory.GetFiles(directory))
            {
                extension = file.Split(".")[1];

                if (extension == "obj")
                    objPath = file;
                if (extension == "mtl")
                    objPath_material = file;
            }

            loadObjectToScene(objPath, objPath_material);

            counter++;
            if (counter % 8 == 0)
            {
                row++;
            }
        }

    }




    protected void Update()
    {
        if(Input.GetKeyDown(KeyCode.M) && Input.GetKey(KeyCode.LeftControl))
        {
            for(int j = 0; j <20; j++)
            {
                //UI.progressMax++;
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

                    UI.progressCurrent++;

                    loadObjectToScene(objPath, objPath_material);

                    if (counter == i)
                    {
                        if (counter % 8 == 0)
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

        if (PlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            //UI.progressMax++;
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

                UI.progressCurrent++;

                loadObjectToScene(objPath, objPath_material);

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
    protected void loadObjectToScene(string objectPath, string materialPath)
    {
        foreach (int n in skiplist) // go over every number in the list
        {
            if (n == counter)
                return;
        }

        loadedObject = new OBJLoader().Load(objectPath, materialPath);

        loadedObject.transform.localScale = new Vector3(-0.01f, -0.01f, -0.01f);
        loadedObject.transform.position = new Vector3(-40.0f + counter % 8 * 10.0f, 8.75f, 10.0f + counter / 8 * 10.0f);
        loadedObject.transform.Rotate(new Vector3(90.0f, 180.0f, 0.0f));

        GameObject childGameObject;
        MeshRenderer childGameObjectRenderer;

        for (int child = 0; child < loadedObject.transform.childCount; child++)
        {
            childGameObject = loadedObject.transform.GetChild(child).gameObject;
            childGameObject.AddComponent<ExhibitObject>();
            childGameObjectRenderer = childGameObject.GetComponent<MeshRenderer>();
            childGameObjectRenderer.material.shader = Shader.Find("Standard");
            childGameObjectRenderer.material.SetFloat("_Glossiness", 0.0f);

            //tempCollider = childGameObject.AddComponent<BoxCollider>();
            //childGameObject.AddComponent<CapsuleCollider>();
            //childGameObject.AddComponent<Rigidbody>();
        }
    }

}
