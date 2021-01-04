using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWithGravity : MonoBehaviour
{
    // Start is called before the first frame update
    public  GameObject projectile;
    Vector3 shootDirection;
    public float bulletSpawnOffset;
    public float shootvelocity;
    void Start()
    {
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
          
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
          
        }


    }
    // Update is called once per frame
    void FixedUpdate()
    {
        shootDirection = (transform.forward + transform.up).normalized;
        Vector3[] trajectory = PlotTrajectory(projectile.GetComponent<Rigidbody>(),transform.position + shootDirection * bulletSpawnOffset, shootDirection, shootvelocity,200);

        for (int i = 0; i < trajectory.Length-1; i++)
        {
            Debug.Log(i);
          
           Debug.DrawLine(trajectory[i], trajectory[i+1], Color.blue, .05f); 
        }
    }
    
    public void FireProjectile() 
    {
        GameObject newBullet = Instantiate(projectile, transform.position + shootDirection * bulletSpawnOffset,Quaternion.LookRotation(shootDirection));
        // newBullet.GetComponent<Rigidbody>().AddForce(shootDirection * shootvelocity);
        newBullet.GetComponent<Rigidbody>().velocity = shootDirection * shootvelocity;
            
        Destroy(newBullet, 10);
    }

    public Vector3[] PlotTrajectory(Rigidbody rigidbody, Vector3 pos, Vector3 direction, float speed, int steps)
    {
        Vector3[] results = new Vector3[steps];

        float timestep = Time.fixedDeltaTime / Physics.defaultSolverVelocityIterations;
        Vector3 gravityAcceleration = Physics.gravity * timestep * timestep;
        float friction = 1f - timestep * 1 * rigidbody.drag;
        Vector3 velocity = direction * speed * timestep;
        
        for (int i = 0; i < steps; ++i)
        {
            velocity += gravityAcceleration;
            velocity *= friction;
            pos += velocity;
            results[i] = pos;
        }

        return results;

    }

}
