using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour{

    [SerializeField] AnimationCurve ease=default;


    public IEnumerator Shake(float totalTime, float magnitudeMax) {

        

        Vector3 originalPosition = transform.localPosition;
        float elapsedTime = 0.0f;


        while (elapsedTime < totalTime) {

            float magnitude = Mathf.Lerp(0f, magnitudeMax, ease.Evaluate(elapsedTime / totalTime));
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originalPosition.x+x, originalPosition.y+y, originalPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        transform.localPosition = originalPosition;

    }

}
