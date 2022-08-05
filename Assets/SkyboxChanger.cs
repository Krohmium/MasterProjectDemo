using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    [SerializeField] private List<Material> skyboxes;
    private int skyboxLength, currentSkybox = 0;
    // Start is called before the first frame update
    void Start()
    {
        skyboxLength = skyboxes.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            currentSkybox = (currentSkybox + 1) % skyboxLength;
            GetComponent<Skybox>().material = skyboxes[currentSkybox];
        }
    }
}
