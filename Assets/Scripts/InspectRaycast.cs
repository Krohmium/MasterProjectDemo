using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectRaycast : MonoBehaviour
{
    [SerializeField] private int rayLength = 25;
    [SerializeField] private LayerMask layerMaskInteract;
    private InspectorObjectController raycastedObj;

    [SerializeField] private Image crosshair;
    private bool isCrosshairActive;
    private bool doOnce;
    private bool closeUpActive;
    private bool extraInfoActive;

    [SerializeField] private Camera MainCamera;
    [SerializeField] private Camera CloseUpCamera;
    [SerializeField] private GameObject closeUpObjectParent;
    [SerializeField] private GameObject closeUpAnchor;

    private movement movementScript;

    private GameObject closeUpObject;

    // Start is called before the first frame update
    void Start()
    {
        movementScript = this.gameObject.transform.parent.transform.parent.GetComponentInParent<movement>();
        MainCamera.enabled = true;
        CloseUpCamera.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (!closeUpActive && Physics.Raycast(transform.position, fwd, out hit, rayLength, layerMaskInteract.value))
        {
            if(hit.collider.CompareTag("ExhibitObject"))
            {
                if(!doOnce)
                {
                    CrosshairChange(true);
                }
                raycastedObj = hit.collider.gameObject.GetComponent<InspectorObjectController>();
                raycastedObj.ShowObjectName();
                isCrosshairActive = true;
                doOnce = true;

                if(Input.GetKeyDown(KeyCode.I))
                {
                    if (!extraInfoActive)
                    {
                        raycastedObj.ShowExtraInfo();
                        extraInfoActive = true;
                    }
                    else
                    {
                        raycastedObj.HideExtraInfo();
                        extraInfoActive = false;
                    }
                }
                else if(Input.GetKeyDown(KeyCode.C))
                {
                    MainCamera.enabled = false;
                    CloseUpCamera.enabled = true;
                    movementScript.freeze = true;
                    MainCamera.transform.parent.transform.parent.GetComponent<camera>().freeze = true;
                    closeUpActive = true;

                    closeUpAnchor = new GameObject("ParentAnchor " + raycastedObj.gameObject.transform.name);
                    closeUpAnchor.transform.parent = closeUpObjectParent.transform;
                    closeUpAnchor.transform.localPosition = new Vector3(0, 0, 0);
                    closeUpAnchor.transform.localScale = new Vector3(1f, 1f, 1f);

                    closeUpObject = Instantiate(raycastedObj.gameObject);
                    closeUpObject.transform.parent = closeUpAnchor.transform;
                    closeUpObject.layer = 5;
                    closeUpObject.GetComponent<ExhibitObject>().SetForCloseup();
                    crosshair.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if(isCrosshairActive)
            {
                raycastedObj.HideObjectName();
                CrosshairChange(false);
                doOnce = false;

                if (extraInfoActive)
                {
                    raycastedObj.HideExtraInfo();
                    extraInfoActive = false;
                }
            }
        } 

        if (Input.GetKeyDown(KeyCode.Escape) && closeUpActive)
        {
            MainCamera.enabled = true;
            CloseUpCamera.enabled = false;
            movementScript.freeze = false;
            MainCamera.transform.parent.transform.parent.GetComponent<camera>().freeze = false;
            closeUpActive = false;
            crosshair.gameObject.SetActive(true);

            Destroy(closeUpObject);
            Destroy(closeUpAnchor);
        }
    }
    void CrosshairChange(bool on)
    {
        if (on && !doOnce)
            crosshair.color = Color.red;
        else
            crosshair.color = Color.white;
    }
}
