using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWithGravity : MonoBehaviour
{
    public GameObject projectile;
    Vector3 shootDirection;
    public float bulletSpawnOffset;
    public float shootvelocity;

    private void Start()
    {
        //Time.timeScale = .3f;
        shootDirection = (transform.forward + transform.up);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            shootvelocity += Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            shootvelocity -= Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            shootDirection.y += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            shootDirection.y -= Time.deltaTime;
        }
    }


    void FixedUpdate()
    {
        shootDirection = shootDirection.normalized;

        Vector3[] trajectory = PlotTrajectory(projectile.GetComponent<Rigidbody>(), transform.position + shootDirection * bulletSpawnOffset, shootDirection, shootvelocity, 200);

        for (int i = 0; i < trajectory.Length - 1; i++)
        {
            Debug.DrawLine(trajectory[i], trajectory[i + 1], Color.blue, .05f);
        }
    }

    public void FireProjectile()
    {
        GameObject newBullet = Instantiate(projectile, transform.position + shootDirection * bulletSpawnOffset, Quaternion.LookRotation(shootDirection));
        newBullet.GetComponent<Rigidbody>().velocity = shootDirection * shootvelocity;

        Destroy(newBullet, 10);
    }

    public Vector3[] PlotTrajectory(Rigidbody rigidbody, Vector3 pos, Vector3 direction, float speed, int steps)
    {

        float timeframe = Time.fixedDeltaTime;

        Vector3 gravitationalAcceleration = Physics.gravity * timeframe * timeframe;

        float friction = 1f - timeframe * rigidbody.drag;

        Vector3 velocity = direction * speed * timeframe;

        Vector3[] results = new Vector3[steps];

        for (int i = 0; i < steps; ++i)
        {
            RaycastHit hit;

            if (Physics.Raycast(pos+ direction* (projectile.transform.position.x/2) , direction, out hit,.5f) && hit.transform.tag == "Bounce")
            {
                //Debug.DrawLine(pos + direction * (projectile.transform.position.x / 2), pos + (direction * (projectile.transform.position.x / 2) +( direction * 1.1f)), Color.yellow, .1f);
                Debug.Log(hit.transform.name);
                velocity = Bounce(velocity, hit.normal);
            }

            velocity += gravitationalAcceleration;
            velocity *= friction;
            pos += velocity;
            results[i] = pos;

        }

        return results;

    }

    Vector3 Bounce(Vector3 velocity, Vector3 hitNormal)
    {
        //Vector3 normal = Vector3.Project(velocity, hitNormal);
        //Vector3 tangent = velocity - normal;
        //return tangent - normal;

        return Vector3.Reflect(velocity, hitNormal);

        // Elastic collision calculation
        //float mass1 = 1;
        //float mass2 = 1;
        //Vector3 v1f = ((mass1 - mass2) / (mass2 + mass1)) * velocity + ((2 * mass2) / (mass2 + mass1)) * new Vector3(0, 0, 0);


        //return v1f;


    }


    //Collider[] OverlappingObject = Physics.OverlapSphere(pos, projectile.transform.localScale.x, mask);

    //if (OverlappingObject.Length > 0)
    //{ 
    // velocity = Bounce(velocity, -OverlappingObject[0].gameObject.transform.forward);
    //}

}


