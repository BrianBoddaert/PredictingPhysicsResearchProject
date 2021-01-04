using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    private Vector3 dir;
    private float speed = 15;
    // Start is called before the first frame update
    void Start()
    {
        dir = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * (speed * Time.deltaTime);
    }


    void SetDirection(Vector3 direction)
    {
        dir = direction;
    }
}
