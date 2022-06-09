using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using UnityEditor;
using System.IO;


namespace AsImpL
{
    public class MuseumGenerator : MonoBehaviour
    {

        [SerializeField]
        protected ImportOptions importOptions = new ImportOptions();

        string objectPath = string.Empty;
        string materialPath = string.Empty;
        string extension = string.Empty;

        string error = string.Empty;
        GameObject loadedObject;
        BoxCollider tempCollider;
        List<GameObject> loadedObjects = new List<GameObject>();
        protected bool PlayerInRange;

        int counter = 0;
        int row = 0;

        protected ObjectImporter objImporter;


        private void Awake()
        {
            objImporter = gameObject.GetComponent<ObjectImporter>();
            if (objImporter == null)
            {
                objImporter = gameObject.AddComponent<ObjectImporter>();
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            objectPath = "C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\figure-holding-the-hraschina-meteorite\\Karyatide_Hraschina_Saal4_EDIT_VW_lowres.obj";
            materialPath = "C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\figure-holding-the-hraschina-meteorite\\Karyatide_Hraschina_Saal4_EDIT_VW_lowres.mtl";

            foreach (string directory in System.IO.Directory.GetDirectories("C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\"))
            {
                if (counter > 2)
                    break;

                foreach (string file in System.IO.Directory.GetFiles(directory))
                {
                    extension = file.Split(".")[1];

                    if (extension == "obj")
                        objectPath = file;
                    if (extension == "mtl")
                        materialPath = file;
                }

                StartCoroutine(loadObjectToScene(objectPath, materialPath, counter));

                counter++;
                if (counter % 8 == 0)
                {
                    row++;
                }
            }
        }

        protected void Update()
        {
            if (PlayerInRange && Input.GetKeyDown(KeyCode.E))
            {
                int i = 0;
                foreach (string directory in System.IO.Directory.GetDirectories("C:\\Users\\Krohm\\Documents\\Uni\\Masterarbeit\\NHMV Models\\temp\\obj\\"))
                {
                    if (i < counter)
                    {
                        i++;
                        continue;
                    }

                    foreach (string file in System.IO.Directory.GetFiles(directory))
                    {
                        extension = file.Split(".")[1];

                        if (extension == "obj")
                            objectPath = file;
                        if (extension == "mtl")
                            materialPath = file;
                    }

                    StartCoroutine(loadObjectToScene(objectPath, materialPath));

                    if (counter == i)
                    {
                        if (counter % 8 == 0)
                        {
                            row++;
                        }
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


        protected IEnumerator loadObjectToScene(string objectPath, string materialPath, int wait = 0)
        {
            yield return new WaitForSeconds(5*wait);
            //loadedObject = new OBJLoader().Load(objectPath, materialPath);
            loadedObject = new GameObject(objectPath.Split(".")[0] + "-parent");
            objImporter.ImportModelAsync(objectPath.Split("\\")[^1].Split(".")[0], objectPath, loadedObject.transform, importOptions);

            loadedObject.transform.localScale = new Vector3(-0.01f, -0.01f, -0.01f);
            loadedObject.transform.position = new Vector3(-40.0f + counter % 8 * 10.0f, 8.75f, 10.0f + counter / 8 * 10.0f);
            //loadedObject.transform.Rotate(new Vector3(90.0f, 180.0f, 0.0f));

            GameObject childGameObject;
            MeshRenderer childGameObjectRenderer;

            for (int child = 0; child < loadedObject.transform.childCount; child++)
            {
                childGameObject = loadedObject.transform.GetChild(child).gameObject;
                //childGameObject.AddComponent<ExhibitObject>();
                //childGameObjectRenderer = childGameObject.GetComponent<MeshRenderer>();
                //childGameObjectRenderer.material.shader = Shader.Find("Standard");
                //childGameObjectRenderer.material.SetFloat("_Glossiness", 0.0f);

                //tempCollider = childGameObject.AddComponent<BoxCollider>();
                childGameObject.AddComponent<CapsuleCollider>();
                //childGameObject.AddComponent<Rigidbody>();
            }
        }


    }

}
