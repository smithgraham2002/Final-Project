# Weeks of Pain
By Muhammad Mahdi, Andrew Minerva, and Graham Smith

[![Full Demo](https://img.youtube.com/vi/<VIDEO_ID>/hqdefault.jpg)]([https://youtube.com/shorts/lXtx30z4mqE?feature=share](https://youtu.be/W30Y73nG8Gk))
## Overview
We created a game in which a player searches a randomly generated dungeon for a goal. Along the way, enemies will chase the player if they are spotted, and if the player gets too close to an enemy, they will be attacked. 

## Approach
To procedurally generate a random dungeon each time the game is played, we employed a binary search partitioning algorithm. We would begin by defining a size for the dungeon, as well as a minimum room size. The algorithm would then begin dividing the dungeon space in half and storing the divisions in a binary search tree. The space would be divided continually until a specified number of iterations had been reached or the minimum room size was reached. The rooms are then shrunk so that they are not touching each other. Corridors are added between divisions that share a parent node in the binary search tree. Thus, a cohesive dungeon is generated randomly on each run.
To create enemies that would add some challenge to the game, while not being overwhelming, we created an enemy AI that would patrol the level until the player entered the enemy’s sight range. At that point, the enemy would begin to chase the player, following closely behind them. If the player entered the enemy’s attack range, the enemy would launch a ball at the player. The game restarts as soon as the player is hit by a projectile.
We can scale our game up by simply giving the binary search partitioning algorithm larger values for the total size of the dungeon. The number of enemies is proportionally related to the number of rooms that are generated by the algorithm, so the number of enemies will scale with the larger dungeons. The biggest limitation is system resources. Generating a dungeon in a 90x90 space with rooms of minimum size of 10x10 only takes a few seconds, while generating a dungeon in a 500x500 space with similarly sized rooms takes a considerably longer time (around 45 seconds on our hardware).

## Initial Idea
![image](https://github.com/smithgraham2002/Final-Project/assets/103609167/a74f12a9-d948-4f18-b207-931295d7c2e9)
The diagram above shows our initial sketch of our game. For the most part, the creation of the game went according to plan. We managed to include the procedural dungeon generation, the goal, and the enemies. We were even able to make the game three-dimensional, as we had originally intended. The one key feature missing from our final product is the staircase to the next floor. Our final version instead includes a goal that resets the level when touched. In a way, the goal acts similarly to the stairs, by letting you continue the game endlessly. However, it does lack the polish that the staircase would have brought. Additionally, while the initial sketch does not quite demonstrate it, we originally intended the game to be more of a dungeon-crawler. The final product is more of a horror game, with enemies that chase the player and cannot be defeated.
![image](https://github.com/smithgraham2002/Final-Project/assets/103609167/8043dca0-68df-46e4-ab98-562499d9910c)

## Feedback
Much of the feedback we received was regarding the transparent walls. While we had initially intended to make the dungeon with solid walls, we liked the way that the somewhat transparent walls added to the atmosphere. The walls look almost like a cage, and are often disorienting. We also received requests for more blood, and while we did not quite add this, we did add music that would play as the enemies approached to make the game scarier and more suspenseful.

## Comparisons to State-of-the-Art Algorithms
While our method of generating dungeon rooms is very similar to the state-of-the-art algorithms we found in our research, our method for creating the corridors between rooms is not quite as advanced as other methods. We simply created straight lines between rooms that shared a parent node in the binary search tree. Other methods also involve connecting rooms that share parent nodes. However, the way more advanced algorithms connect the rooms is more creative and organic. One method is to send out a line that would move randomly from one room until it connects to the target room. Then, created a corridor along that line. This results in naturally winding paths. Another method is to send a straight line out from one room and each time it collides with the boundary of the dungeon or a room other than the target room, reflect the velocity. Again, the corridor is created along the resulting line.

## Expansion
To expand our project, we could follow through on our original idea to have a staircase that would take the player to another level. We also though of keeping track of how many times the player had reached the goal and making the game more difficult based on their score. The difficulty could be scaled by adding more enemies, generating larger environments, or increasing the attack range of the enemies. Thus, the game could be turned into a high-score based game.

## Works Cited
https://github.com/SunnyValleyStudio/Unity_Procedural_Dungeon_binary_space_partitioning/blob/master/Version%201%20generating%20mesh%20for%20roads%20and%20corridors/DugeonGenerator.cs
We used this tutorial to code the binary search partitioning algorithm.

https://www.youtube.com/watch?v=f473C43s8nE
We used this tutorial to create the enemy AI.

Included in this website are all of the scripts we used for our game. 
