### Predicting Physics Research Project ###



## What is the topic? ## 
Many games make use of physics and physics engines. Many of which use pseudo realistic real world physics, spanning from elastic collisions in pool games to gravity and other forces in open world games. Often you'll see that predicting these physics plays an important role in games, we can do this by using the same formulas that were used to program them in the first place.

![image](https://spaceapetech.files.wordpress.com/2016/05/angrybirdstrajectory.png?w=676)
An example of this would be angry birds, predicting the trajectory of the projectiles.


## Implementation  ##
I created a unity project to see if we can predict the unity physics engine. I started this off by creating a "SmartAI" character making use of predictions and "DumbAI" characters playing the role of simple hostiles. I made the simple hostiles shoot projectiles at the SmartAI. The SmartAI does not see these projectiles until they are within a radius indicated by the circle surrounding him. The variables of the projectile (besides it's current position) are of course made private. The SmartAI can not reach any variables besides what he can see and thus is made obvious to him.
![ezgif com-gif-maker (3)](https://user-images.githubusercontent.com/35961897/103555921-4d6b7d00-4eb1-11eb-9afb-aa3a9991f6b5.gif)

##  Result ##
## Conclusion ##
