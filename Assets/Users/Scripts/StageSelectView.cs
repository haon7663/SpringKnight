using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectView : MonoBehaviour
{
    [SerializeField] SwipeUI swipeUI;
    [SerializeField] Text stageText;

    Transform[] stages;

    void Start()
    {
        stages = new Transform[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)
        {
            stages[i] = swipeUI.transform.GetChild(i);
        }
    }
    void Update()
    {
        stageText.text = stages[swipeUI.currentPage].name;
    }
}
