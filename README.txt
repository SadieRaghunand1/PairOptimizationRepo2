Cross the Bridge

Developer: Sadie Raghunand

Endless runner where the player must move side to side and jump to avoid colliding with ever-faster clouds while crossing an endless bridge. As the game progresses, clouds move faster and appear more often.  Hitting a cloud causes a decrease in health, of which the player starts with 3.  However, after passing a certain number of clouds, set by increasing intervals of checkpoints, a healing object will appear, which heals the player for 1 health.  The player gains points for each cloud passed and loses points if they collide with one.  The number of points is determined by the size of the cloud.

Controls:
AD/Left/Right arrows - move
Space - jump

Optimizations done by Ovidio Juan:

1. Displaying text score - Unity has a new feature called “SetText” optimized to prevent string allocations and boxing.

2. Applying cooldown timer - Cache the seconds so it's only used once

3. Recursive pool releasing - Adding a while true condition and caching “release Wait” allows to reuse those 2 objects instead of new instances.

4. Retrieving transform component - Can just directly call transform since it derives from monobehaviour. 

5. Caching Components - Cache components material, renderer and WaitForseconds at start to reduce gc allocations.

6. OnCollisionEnter - You can cache the reference or also use TrygetComponent bit faster than getcomponent, and avoids gc if object is null





