using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartBullet : MonoBehaviour
{
    private void FixedUpdate()
    {
        Vector3 dir = GetComponent<Rigidbody>().velocity.normalized;
        Debug.DrawLine(transform.position, transform.position + dir * 1, Color.red, .1f);
    }

}

