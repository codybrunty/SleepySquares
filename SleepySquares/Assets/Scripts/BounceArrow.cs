using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceArrow : MonoBehaviour
{

    public float delayTime = 0.25f;
    public float moveTime = 0.5f;
    public Vector3 moveAmmount = new Vector3(0f, 1f, 0f);

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = gameObject.transform.position;

    }

    public void OnEnable()
    {
        StartCoroutine(MoveArrow());
    }

    IEnumerator MoveArrow()
    {
        gameObject.transform.position = startPosition;
        Vector3 newPos = startPosition + moveAmmount;
        yield return new WaitForSeconds(delayTime);
        Hashtable hash = new Hashtable();
        hash.Add("position", newPos);
        hash.Add("time", moveTime);
        hash.Add("easetype", "easeInOutSine");
        hash.Add("looptype", "pingpong");
        iTween.MoveTo(gameObject, hash);
    }
}
