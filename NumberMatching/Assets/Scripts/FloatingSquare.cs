using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSquare : MonoBehaviour{

    float thrust = 2f;
    [SerializeField] Rigidbody rb = default;

    public void FloatBurst(){
        rb.isKinematic = false;
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = UnityEngine.Random.Range(-1f, 1f);
        float z = UnityEngine.Random.Range(-1f, 1f);
        Vector3 direction = new Vector3(x, y, z);
        rb.AddForce(direction * thrust, ForceMode.Impulse);

        rb.AddTorque(new Vector3(x, y, z) * 50f);
    }

    public void QuickBurst()
    {
        rb.isKinematic = false;

        int highOrLow = UnityEngine.Random.Range(0, 2);
        float x = 0f;
        if (highOrLow==0)
        {
            x = UnityEngine.Random.Range(-1f, -.25f);
        }
        else
        {
            x = UnityEngine.Random.Range(.25f, 1f);
        }
        highOrLow = UnityEngine.Random.Range(0, 2);
        float y = 0f;
        if (highOrLow == 0)
        {
            y = UnityEngine.Random.Range(-1f, -.25f);
        }
        else
        {
            y = UnityEngine.Random.Range(.25f, 1f);
        }


        float z = UnityEngine.Random.Range(-1f, 0f);
        Vector3 direction = new Vector3(x, y, z);
        rb.AddForce(direction * thrust * 5f, ForceMode.Impulse);

        rb.AddTorque(new Vector3(x,y,z) * 250f);
    }

    public void StopFloat()
    {
        StopMovement();
        gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        gameObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    private void StopMovement()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


}
