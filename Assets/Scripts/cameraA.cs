using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraA : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // This is expressed in "units per second".
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * Time.deltaTime * speed;
        }
    }
}
