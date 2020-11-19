using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSomeTorque : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Square")
        {
            //Vector3 squarePosition = other.transform.position;
            //Vector3 cameraPosition = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, -Camera.main.transform.position.z);
            //Vector3 direction = cameraPosition - squarePosition;

            Rigidbody rb = other.GetComponent<Rigidbody>();
            Vector3 direction = transform.InverseTransformDirection(rb.velocity);
            Vector3 newDirection = new Vector3(direction.x, direction.y, -direction.z);

            rb.AddForce(newDirection, ForceMode.Impulse);
        }
    }
}
