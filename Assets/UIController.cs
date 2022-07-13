using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public int progressCurrent=0;
    public int progressMax=0;
    public ProgressBar progress;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        progress = root.Q<ProgressBar>("loadingprogress");

    }

    // Update is called once per frame
    void Update()
    {
        if (progressMax != 0)
        {
            progress.value = (float)progressCurrent / (float)progressMax*100;
        }
        else
        {
            progress.value = 100;
        }
    }
}
