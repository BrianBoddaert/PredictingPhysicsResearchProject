using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartAI : MonoBehaviour
{
    List<SpottedProjectileInformation> projectilesInViewRange = new List<SpottedProjectileInformation>();

    public float viewRange = 5;
    public float speed = 10;
    public float size = 1;
    public Vector3 direction;


    // TEMP PUBLIC VARS FOR DEBUGGING
    Vector3 debugCenter;
    float debugRadius;
    Vector3 debugDirection;
    float debugWidth;

    //SpottedProjectileInformation[] projectilesInRange = new SpottedProjectileInformation[2];

    // Start is called before the first frame update
    void Start()
    {
      //  Time.timeScale = .3f;
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector3(0, 0, 0);
        PredictTrajectories();
        UpdateProjectilesInViewRange();
        transform.position += direction * (speed * Time.deltaTime);
       

    }

    void UpdateProjectilesInViewRange()
    {
        GameObject[] projectilesInScene = GameObject.FindGameObjectsWithTag("Projectile");

        for (int i = 0; i < projectilesInScene.Length; i++) // Loop through all projectilesInScene in the scene
        {
            SpottedProjectileInformation listedProjectile = new SpottedProjectileInformation();

            float distance = (projectilesInScene[i].transform.position - transform.position).magnitude;

            if (IsProjectileAlreadyInList(projectilesInScene[i], ref listedProjectile))// Check if already listed
            {
                if (distance > viewRange) // If not within range
                {
                   projectilesInViewRange.Remove(listedProjectile);
                    continue;
                }
            }
            else if (distance <= viewRange)  // Newly spotted projectile
            {
                 SpottedProjectileInformation projectile = new SpottedProjectileInformation();
                 projectile.gameObject = projectilesInScene[i];
                 projectile.previousPosition = projectilesInScene[i].transform.position;
                 projectile.width = projectile.gameObject.transform.localScale.x;
                 projectile.halfWidth = projectile.gameObject.transform.localScale.x / 2;
                 projectilesInViewRange.Add(projectile); // Save within list   
            }

        }
    }
    void PredictTrajectories()
    {
        for (int i = 0; i < projectilesInViewRange.Count; i++) // Loop through all projectilesInScene in the scene
        {
            SpottedProjectileInformation projectile = projectilesInViewRange[i];

            Vector3 projectilePosition = projectile.gameObject.transform.position;
            Vector3 projectileDistanceTravelledPerFrame = projectilePosition - projectile.previousPosition;

            projectile.direction = projectileDistanceTravelledPerFrame.normalized;
            projectile.velocity = projectileDistanceTravelledPerFrame.magnitude;

            projectile.previousPosition = projectilePosition;

            LayerMask mask = LayerMask.GetMask("SmartAI");
            RaycastHit hitInfo;

            Debug.DrawRay(projectilePosition - projectile.direction * projectile.halfWidth, projectile.direction * 50, new Color(1, 0, 0), 1);

            debugCenter = projectilePosition;
            debugRadius = projectile.halfWidth;
            debugDirection = projectile.direction;
            debugWidth = projectile.width;

            if (Physics.SphereCast(projectilePosition, projectile.halfWidth, projectile.direction, out hitInfo, 50, mask))
            {
                Debug.Log(projectile.gameObject.name);
                Quaternion rot = Quaternion.LookRotation(projectile.direction);
                rot *= Quaternion.Euler(0, 90, 0);
                Vector3 dir = rot * transform.forward;
                direction = dir;
            }
        }
    }

    bool IsProjectileAlreadyInList(GameObject o, ref SpottedProjectileInformation listedProjectile)
    {
        for (int j = 0; j < projectilesInViewRange.Count; j++)
        {
            if (projectilesInViewRange[j].gameObject.Equals(o))
            {
                listedProjectile = projectilesInViewRange[j];
                return true; 
            }
        }

        return false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            transform.Find("Visual").GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    private void OnDrawGizmos()
    {

        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(debugCenter + debugDirection * debugWidth, debugRadius);
    }

}



struct SpottedProjectileInformation
{
    public GameObject gameObject;
    public Vector3 previousPosition;
    public Vector3 direction;
    public float velocity;
    public float width;
    public float halfWidth;
    public Vector3 offsettingForceOnSmartAI;
}