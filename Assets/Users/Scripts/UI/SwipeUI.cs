using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] float swipeTime = 0.2f;
    [SerializeField] float swipeDistance = 50f;

    [HideInInspector]
    public int currentPage = 0;
    int maxPage = 0;

    float[] scrollPageValues;
    float valueDistance = 0;
    float startTouchX;
    float endTouchX;
    bool isSwipeMode = false;

    void Awake()
    {
        scrollPageValues = new float[transform.childCount];

        valueDistance = 1f / (scrollPageValues.Length - 1f);

        for(int i = 0; i < scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance * i;
        }

        maxPage = transform.childCount;
    }

    void Start()
    {
        SetScrollBarValue(0);
    }

    public void SetScrollBarValue(int index)
    {
        currentPage = index;
        scrollbar.value = scrollPageValues[index];
    }

    void Update()
    {
        UpdateInput();
    }
    void UpdateInput()
    {
        if (isSwipeMode == true) return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            startTouchX = Input.mousePosition.x;
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchX = Input.mousePosition.x;
            UpdateSwipe();
        }
#endif
#if UNITY_ANDROID
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                startTouchX = Input.mousePosition.x;
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchX = Input.mousePosition.x;
                UpdateSwipe();
            }
        }
#endif
    }

    void UpdateSwipe()
    {
        if(Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
        {
            StartCoroutine(OnSwipeOneStep(currentPage));
            return;
        }

        bool isLeft = startTouchX < endTouchX;

        if(isLeft)
        {
            if (currentPage == 0)
                return;

            currentPage--;
        }
        else
        {
            if (currentPage == maxPage - 1)
                return;

            currentPage++;
        }

        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    IEnumerator OnSwipeOneStep(int index)
    {
        float start = scrollbar.value;
        float current = 0;
        float percent = 0;

        isSwipeMode = true;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / swipeTime;

            scrollbar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

            yield return null;
        }

        isSwipeMode = false;
    }
}
