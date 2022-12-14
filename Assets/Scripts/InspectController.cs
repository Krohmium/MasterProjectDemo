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
    [SerializeField] private Text helpText;
    [SerializeField] private GameObject helpBG;
    [SerializeField] private int fontSize = 14;
    [HideInInspector] public bool startTimer;
    private bool helpActive = false;
    // Start is called before the first frame update
    void Start()
    {
        objectNameBG.SetActive(false);
        extraInfoBG.SetActive(false);
        helpBG.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (helpActive)
            {
                HideHelp();
                helpActive = false;
            }
            else
            {
                ShowHelp();
                helpActive = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            IncreaseFontSize();
        }
        else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            DecreaseFontSize();
        }
    }
    private void IncreaseFontSize()
    {
        fontSize = fontSize + 2 % int.MaxValue;
        DisplayFontSize();
    }
    private void DecreaseFontSize()
    {
        fontSize = Mathf.Max(fontSize -2, 2);
        DisplayFontSize();
    }
    private void DisplayFontSize()
    {
        objectNameUI.fontSize = fontSize;
        extraInfoUI.fontSize = fontSize;
        helpText.fontSize = fontSize;
    }

    public void ShowName(string objectName)
    {
        objectNameBG.SetActive(true);
        objectNameBG.GetComponent<RectTransform>().sizeDelta = new Vector2(objectName.Length * fontSize * 0.5f + 30, fontSize*1.4f +10f);
        objectNameUI.GetComponent<RectTransform>().sizeDelta = new Vector2(objectName.Length * fontSize * 0.5f + 30, fontSize * 1.4f+10f);
        objectNameUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(objectNameUI.GetComponent<RectTransform>().anchoredPosition.x, fontSize * -0.5f);
        objectNameUI.text = objectName;
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
    public void HideHelp()
    {
        helpBG.SetActive(false);
    }
    public void ShowHelp()
    {
        helpBG.SetActive(true);
    }
}
