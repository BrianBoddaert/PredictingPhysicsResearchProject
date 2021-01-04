# Predicting Physics Research Project #



## What is the topic? ## 
Many games make use of physics and physics engines. Many of which use pseudo realistic real world physics, spanning from elastic collisions in pool games to gravity and other forces in open world games. Often you'll see that predicting these physics plays an important role in games, we can do this by using the same formulas that were used to program them in the first place.

![image](https://spaceapetech.files.wordpress.com/2016/05/angrybirdstrajectory.png?w=676)\

*An example of this would be angry birds, predicting the trajectory of the projectiles.*

## Implementation  ##

### Calculating velocity and direction  ###
I created a unity project to see if we can predict the unity physics engine. I started this off by creating a "SmartAI" character making use of predictions and "DumbAI" characters playing the role of simple hostiles. I made the simple hostiles shoot projectiles at the SmartAI. The SmartAI does not see these projectiles until they are within a radius indicated by the circle surrounding him.

```C#
        for (int i = 0; i < projectilesInScene.Length; i++)
        {
            SpottedProjectileInformation listedProjectile = new SpottedProjectileInformation();

            float distance = (projectilesInScene[i].transform.position - transform.position).magnitude;

            if (IsProjectileAlreadyInList(projectilesInScene[i], ref listedProjectile))
            {
                if (distance > viewRange) // Projectile on list but now out of sight.
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
                 projectilesInViewRange.Add(projectile); 
            }

        }
```

*SmartAI spotting projectiles within viewrange.*

The variables of the projectile are of course made private. The SmartAI can not reach any variables besides what he can see and thus is made obvious to him (current transform)

![ezgif com-gif-maker (3)](https://user-images.githubusercontent.com/35961897/103555921-4d6b7d00-4eb1-11eb-9afb-aa3a9991f6b5.gif)

*SmartAI moving aside when in the trajectory of an incoming projectile example (1/2).*

![ezgif com-gif-maker (2)](https://user-images.githubusercontent.com/35961897/103561279-80197380-4eb9-11eb-80ea-e45c63244f36.gif)

*SmartAI moving aside when in the trajectory of an incoming projectile example (2/2).*

The smartAI calculates the direction and velocity of the projectile by comparing its position in the last frame to its position now, then uses a spherecast (or raycasts in the case of a square projectile). Then sets its direction to move aside and prevent the projectile from hitting her.

```C#

   Vector3 projectileDistanceTravelledPerFrame = projectilePosition - projectile.previousPosition;
   projectile.direction = projectileDistanceTravelledPerFrame.normalized;
   projectile.velocity = projectileDistanceTravelledPerFrame.magnitude;

   projectile.previousPosition = projectilePosition;

   LayerMask mask = LayerMask.GetMask("SmartAI");
   RaycastHit hitInfo;

   if (Physics.SphereCast(projectilePosition, projectile.halfWidth, projectile.direction, out hitInfo, 50, mask))
   {
       Quaternion rot = Quaternion.LookRotation(projectile.direction);
       rot *= Quaternion.Euler(0, 90, 0);
       direction = rot * transform.forward // Dodge the projectile
   }
```

*SmartAI moving aside when in the trajectory of an incoming projectile code.*

### Predicting trajectory with gravity and drag  ###


## Difficulties ##
##  Result ##
## Conclusion ##
