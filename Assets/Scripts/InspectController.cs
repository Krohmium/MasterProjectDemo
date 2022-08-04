using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectController : MonoBehaviour
{
    [SerializeField] private GameObject objectNameBG;
    [SerializeField] private Text objectNameUI;
    [SerializeField] private float onScreenTimer;
    [SerializeField] private Text extraInfoUI;
    [SerializeField] private GameObject extraInfoBG;
    [SerializeField] private int fontSize = 30;
    [HideInInspector] public bool startTimer;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        objectNameBG.SetActive(false);
        extraInfoBG.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowName(string objectName)
    {
        objectNameBG.SetActive(true);
        objectNameBG.GetComponent<RectTransform>().sizeDelta = new Vector2(objectName.Length * fontSize * 0.5f + 30, fontSize*1.4f +10f);
        objectNameUI.GetComponent<RectTransform>().sizeDelta = new Vector2(objectName.Length * fontSize * 0.5f + 30, fontSize * 1.4f+10f);
        objectNameUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(objectNameUI.GetComponent<RectTransform>().anchoredPosition.x, fontSize * -0.5f);
        objectNameUI.text = objectName;
        objectNameUI.fontSize = fontSize;
    }
    public void HideName()
    {
        objectNameBG.SetActive(false);
        objectNameUI.text = "";
    }
    public void ShowInfo(string newInfo)
    {
        extraInfoBG.SetActive(true);
        extraInfoUI.text = newInfo;
        extraInfoUI.fontSize = fontSize;
    }
    public void HideInfo()
    {
        extraInfoBG.SetActive(false);
        extraInfoUI.text = "";
    }
    public void ShowCloseUp()
    {
    }
    public void HideCloseUp()
    {
    }
}
