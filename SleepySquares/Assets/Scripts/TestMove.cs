using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMove : MonoBehaviour
{
    [SerializeField] GameObject canvas = default;
    [SerializeField] GameObject square = default;
    [SerializeField] GameObject p1 = default;
    [SerializeField] GameObject p2 = default;

    void Update()
    {
        gameObject.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, square.transform.position);
        Debug.Log("p1 " + p1.transform.position.x + " " + p1.transform.position.y);
        Debug.Log("p2 " + p2.transform.position.x + " " + p2.transform.position.y);
    }

}
