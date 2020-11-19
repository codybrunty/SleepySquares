using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNextSquarePosition : MonoBehaviour
{
    [SerializeField] private GameObject nextSquare;

    private void Start()
    {
        Vector3 nextSquarePos = Camera.main.ScreenToWorldPoint(nextSquare.transform.position); ;
        gameObject.transform.position = nextSquarePos;
    }

    public void AdjustPosition()
    {
        Vector3 nextSquarePos = Camera.main.ScreenToWorldPoint(nextSquare.transform.position); ;
        gameObject.transform.position = nextSquarePos;
    }

}
