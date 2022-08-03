using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectorObjectController : MonoBehaviour
{
    [SerializeField] public string objectName;
    [TextArea] [SerializeField] public string extraInfo;
    [SerializeField] public InspectController inspectController;

    public void ShowObjectName()
    {
        inspectController.ShowName(objectName);
    }
    
    public void HideObjectName()
    {
        inspectController.HideName();
    }

    public void ShowExtraInfo()
    {
        inspectController.ShowInfo(extraInfo);
    }

    public void HideExtraInfo()
    {
        inspectController.HideInfo();
    }

    public void ShowCloseUp()
    {
        inspectController.ShowCloseUp();
    }

    public void HideCloseUp()
    {
        inspectController.HideCloseUp();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
