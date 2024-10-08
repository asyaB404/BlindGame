using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    private Vector2 _lastPos = Vector2.zero;
    [SerializeField] private GameObject touchPrefab;
    private readonly List<Touch> _nowTouches = new();
    private readonly Dictionary<int, GameObject> _touchObjsDict = new();


    private void Start()
    {
        Application.targetFrameRate = 240;
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // 在PC上使用鼠标模拟触摸输入
        Vector2 mousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            SimulateTouch(mousePosition, TouchPhase.Began);
            _lastPos = mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            SimulateTouch(mousePosition, _lastPos != mousePosition ? TouchPhase.Moved : TouchPhase.Stationary);
            _lastPos = mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
            SimulateTouch(mousePosition, TouchPhase.Ended);


#elif UNITY_ANDROID || UNITY_IOS
        // 在移动设备上使用真实的触摸输入
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                HandleTouch(Input.GetTouch(i));
            }
        }

#elif UNITY_WEBGL
    // 在WebGL中可以支持触摸或鼠标输入
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                HandleTouch(Input.GetTouch(i));
            }
        }
        else if (Input.GetMouseButton(0)) 
        {
            Vector2 mousePosition = Input.mousePosition;
            SimulateTouch(mousePosition);
        }
#endif
    }

    private void SimulateTouch(Vector2 position, TouchPhase phase)
    {
        // 模拟触摸输入处理逻辑
        var simulatedTouch = new Touch
        {
            fingerId = 0,
            position = position,
            phase = phase,
            tapCount = 1,
        };

        HandleTouch(simulatedTouch);
    }

    private void HandleTouch(Touch touch)
    {
        Vector3 touchPos = Vector3.zero;
        if (Camera.main != null)
        {
            touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0;
        }

        switch (touch.phase)
        {
            case TouchPhase.Began:
                Debug.Log("模拟触摸开始: " + touchPos);
                _nowTouches.Add(touch);
                GameObject o = Instantiate(touchPrefab, touchPos, Quaternion.identity);
                o.transform.localScale = Vector3.zero;
                o.transform.DOScale(1.25f, 0.1f);
                _touchObjsDict[touch.fingerId] = o;
                break;
            case TouchPhase.Moved:
                Debug.Log("模拟触摸移动: " + touchPos);
                _touchObjsDict[touch.fingerId].transform.position = touchPos;
                break;
            case TouchPhase.Ended:
                RemoveTouch(touch);
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Canceled:
                RemoveTouch(touch);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void RemoveTouch(Touch touch)
    {
        _nowTouches.Remove(touch);
        GameObject curTouchObj = _touchObjsDict[touch.fingerId];
        curTouchObj.GetComponent<SpriteRenderer>().DOFade(0, 0.1f).OnComplete(() => { Destroy(curTouchObj); });
        _touchObjsDict.Remove(touch.fingerId);
        Debug.Log("模拟触摸结束");
    }
}