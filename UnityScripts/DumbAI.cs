using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbAI : MonoBehaviour
{
    public GameObject projectile;[SerializeField]
    public GameObject smartAI;[SerializeField]

    private float movementSpeed = 3;
    float bulletSpawnOffset = 5;

    private float shootPase = .4f;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= shootPase)
        {
            timer = 0;
            ShootAtSmartAI();
        }
    }

    void ShootAtSmartAI()
    {
        Vector3 direction = (smartAI.transform.position - transform.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;

        GameObject newBullet = Instantiate(projectile, transform.position + direction * bulletSpawnOffset, rotation);
        Destroy(newBullet, 10);
    }
    //void GenerateRandomDestination()
    //{
    //    Vector3 currentPos = transform.position;
    //    Vector3 randomPos = new Vector3(currentPos.x + Random.Range(-10.0f, 10.0f), 0, currentPos.z + Random.Range(-10.0f, 10.0f));
    //    destination = randomPos;
    //}

    //void MoveToDestination()
    //{
        
    //    transform.position += Vector3.right;
    //}

}
