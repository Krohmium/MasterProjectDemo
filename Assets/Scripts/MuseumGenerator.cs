using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using UnityEditor;
using System.IO;
using System.Xml.Linq;

public class MuseumGenerator : MonoBehaviour
{
    public UIController UI;

    string objPath = string.Empty;
    string objPath_material = string.Empty;
    string objPath_xml = string.Empty;
    string extension = string.Empty;

    string error = string.Empty;
    GameObject loadedObject;
    BoxCollider tempCollider;
    List<GameObject> loadedObjects = new List<GameObject>();
    protected bool PlayerInRange;

    [SerializeField] InspectController inspectController;

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

            objPath = string.Empty;
            objPath_material = string.Empty;
            objPath_xml = string.Empty;

            foreach (string file in System.IO.Directory.GetFiles(directory))
            {
                extension = file.Split(".")[1];

                if (extension == "obj")
                    objPath = file;
                if (extension == "mtl")
                    objPath_material = file;
                if (extension == "xml")
                    objPath_xml = file;
            }

            loadObjectToScene(objPath, objPath_material, objPath_xml);

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

                    objPath = string.Empty;
                    objPath_material = string.Empty;
                    objPath_xml = string.Empty;

                    Debug.Log("Loading new Object");
                    foreach (string file in System.IO.Directory.GetFiles(directory))
                    {
                        extension = file.Split(".")[1];

                        if (extension == "obj")
                            objPath = file;
                        if (extension == "mtl")
                            objPath_material = file;
                        if (extension == "xml")
                            objPath_xml = file;
                    }

                    UI.progressCurrent++;

                    loadObjectToScene(objPath, objPath_material, objPath_xml);

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

                objPath = string.Empty;
                objPath_material = string.Empty;
                objPath_xml = string.Empty;

                Debug.Log("Loading new Object");
                foreach (string file in System.IO.Directory.GetFiles(directory))
                {
                    extension = file.Split(".")[1];

                    if (extension == "obj")
                        objPath = file;
                    if (extension == "mtl")
                        objPath_material = file;
                    if (extension == "xml")
                        objPath_xml = file;
                }

                UI.progressCurrent++;

                loadObjectToScene(objPath, objPath_material, objPath_xml);

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
    protected void loadObjectToScene(string objectPath, string materialPath, string xmlPath)
    {
        foreach (int n in skiplist) // go over every number in the list
        {
            if (n == counter)
                return;
        }

        loadedObject = new OBJLoader().Load(objectPath, materialPath);

        loadedObject.transform.localScale = new Vector3(-0.01f, -0.01f, -0.01f);
        loadedObject.transform.position = new Vector3(-40.0f + counter % 8 * 10.0f, 0, 10.0f + counter / 8 * 10.0f);
        loadedObject.transform.Rotate(new Vector3(90.0f, 180.0f, 0.0f));

        GameObject childGameObject;
        MeshRenderer childGameObjectRenderer;

        for (int child = 0; child < loadedObject.transform.childCount; child++)
        {
            childGameObject = loadedObject.transform.GetChild(child).gameObject;
            ExhibitObject exibObj = childGameObject.AddComponent<ExhibitObject>();
            childGameObjectRenderer = childGameObject.GetComponent<MeshRenderer>();
            childGameObjectRenderer.material.shader = Shader.Find("Standard");
            childGameObjectRenderer.material.SetFloat("_Glossiness", 0.0f);
            childGameObject.tag = "ExhibitObject";
            childGameObject.layer = 6;
           
            InspectorObjectController inspectorObjectController = childGameObject.AddComponent<InspectorObjectController>();
            inspectorObjectController.objectName = childGameObject.gameObject.name;
            inspectorObjectController.inspectController = inspectController;

            if (xmlPath != "")
            {
                XDocument document = XDocument.Load(xmlPath);
                XElement description = document.Element("Description");
                inspectorObjectController.extraInfo = description.Value;
            }
        }
    }

}
