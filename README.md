# Predicting Physics Research Project #



## What is the topic? ## 
Many games make use of physics and physics engines. Many of which use pseudo realistic real world physics, spanning from elastic collisions in pool games to gravity and other forces in open world games. Often you'll see that predicting these physics plays an important role in games, we can do this by using the same formulas that were used to program them in the first place. (The ones that also apply in the real world.)

![image](https://spaceapetech.files.wordpress.com/2016/05/angrybirdstrajectory.png?w=676)

(*An example of this would be angry birds, predicting the trajectory of the projectiles.*)

![image](https://api.luckbox.com/v1/optimize-image/2019-09-cs-go-grenade-trajectory-guide.jpg)

(*CSGO grenade trajectory*)

## Implementation  ##

### Calculating velocity and direction  ###
I created a unity project to see if I can predict Unity's physics. I started this off by creating a "SmartAI" character making use of predictions and "DumbAI" characters playing the role of a simple hostile. I made the simple hostiles shoot projectiles at the SmartAI. The SmartAI does not see these projectiles until they are within a radius indicated by the circle surrounding him.

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

(*SmartAI spotting projectiles within viewrange.*)

The variables of the projectile are of course made private. The SmartAI can not reach any variables besides what he can see and thus is made obvious to him (current transform)

![ezgif com-gif-maker (3)](https://user-images.githubusercontent.com/35961897/103555921-4d6b7d00-4eb1-11eb-9afb-aa3a9991f6b5.gif)

(*SmartAI moving aside when in the trajectory of an incoming projectile example (1/2).*)

![ezgif com-gif-maker (2)](https://user-images.githubusercontent.com/35961897/103561279-80197380-4eb9-11eb-80ea-e45c63244f36.gif)

(*SmartAI moving aside when in the trajectory of an incoming projectile example (2/2).*)

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

(*SmartAI moving aside when in the trajectory of an incoming projectile code.*)


### Predicting trajectory with gravity and drag  ###
To predict the physics of an object with gravity and drag, we first write down / calculate the forces applied to our object.

```C#
        float timeframe = Time.fixedDeltaTime;

        Vector3 gravitationalAcceleration = Physics.gravity * timeframe * timeframe;

        float friction = 1f - timeframe * rigidbody.drag;

        Vector3 velocity = direction * speed * timeframe;
```

(*The gravitational acceleration, just like in our real world, is -9.81 * s^2. Air resistance is done by Unity for us, and velocity is speed / time.*)

The next step was to apply my formulas to the position where the projectile will spawn to predict the trajectory.
This I did a set amount of times so I could connect the dots and visualize the trajectory.

```C#
        for (int i = 0; i < HowManyTimesWeRepeatTheProcess; ++i)
        {
            velocity += gravitationalAcceleration;
            velocity *= friction;
            pos += velocity;
            results[i] = pos;
        }
```

(*Applying our increments*)

Now, all that was left to do is was check and see if everything checks out and then to demonstrate the result. I did this by spawning a projectile with a set velocity and rigidbody. I connected the resulting positions of the previously predicted trajectory using Unity's debugging features and I added the ability to change the velocity and direction at real time.

![ezgif com-gif-maker (6)](https://user-images.githubusercontent.com/35961897/103575211-a8ad6780-4ed1-11eb-84db-7545e71a03e1.gif)

(*Result*)

### Predicting bounce ###

The next thing I wanted to tackle was predicting a bounce result. I was able to predict the bounce direction by using Vector3.reflect() and applying a physics material to the projectile (Without one the projectile has no bounce).

![ezgif com-gif-maker (5)](https://user-images.githubusercontent.com/35961897/103587423-7f97d180-4ee7-11eb-95ed-de7f456a9fe0.gif)

(*Predicted bounce direction*)

This covers the direction, there's is however more to consider when you deal with a bounce, such as the velocity falloff from a part of the kinetic energy turning into angular energy and not to forget friction. For this 3D elastic collision formulas can be applied which of course also depend on the shape of the colliding objects.

## Difficulties ##
When it came to predicting the velocity of a projectile after bouncing off of a wall, I was able to predict the resulting direction from using a raycast and using the hit normal in Vector3.reflect(), this however did not create the wished result. This bounce would be an ideal result that you would see with for instance a basketbal, but when I fired my test projectile at a wall, it had no bounce whatsoever. After some digging I managed to find out I needed to put a physics material on the projectile. Now the bounce direction was correct but there was no velocity fall off. I looked into 3D Elastic collision but it was tricky to find anything but sphere to sphere collisions. While sphere to box collision was what I was looking for.

##  Result ##
With predicting physics and my research done I was able to predict trajectories of objects that use game engine physics. Also without access to any variables but the current transform, I was able to calculate the direction in two frames. If I had more resources I wpuld have made an AI dodging every bullet coming at him (as long as his speed and the radius of his field of view isn't too low). This could make for excellent AI in games. (Such as OpenAI). What I learned could make intriguing game concepts such as self shooting sentry guns that will (almost) never miss, AIs that are incredibly overpowered in video games or shockingly realistic AIs.

## Conclusion 
Next steps to follow up on this research would be perfecting the bounce velocity and predicting physics the other way around, where one can enter a location and get a trajectory to that point instead of entering a velocity and seeing where the projectile would end up.

I definitely learned a lot and was happy to learn all this, I think this will help me much in my future endeavors of becoming a game developer. I hope my research can help others by summarizing all this.


## References ##

https://tech.spaceapegames.com/2016/07/05/trajectory-prediction-with-unity-physics/

https://answers.unity.com/questions/1349122/predicting-projectile-path-after-bouncing-off-phys.html

https://courses.lumenlearning.com/boundless-physics/chapter/collisions/#:~:text=If%20two%20particles%20are%20involved,m%201%20)%20v%202%20i%20.

https://atmos.illinois.edu/courses/atmos100/userdocs/3Dcollisions.html

https://forum.unity.com/threads/how-to-predict-the-physics-simple-sphere-boucing-on-walls.91652/

https://en.wikipedia.org/wiki/Drag_(physics)

https://www.plasmaphysics.org.uk/collision3d.htm
