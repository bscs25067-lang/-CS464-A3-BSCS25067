# CS464 Assignment 03 · Abdullah Saeed · BSCS25067
- **Video (60–120 s):** [YouTube (unlisted) or Google Drive link]
- **Started from:** A new project based off of Lab 6. Assignment 2 was a 3rd person player with a follow camera which clashed with the mechanics I wanted here so I created a new project.
- **Controls:** WASD for Movement
- **To win a level:** Collect the specified number of coins each level while surviving the enemies

## Levels
| Level | Screenshot | New idea | Combined with |
|---|---|---|---|
| Level01 | <img src="Docs/a3/level01.png" width="320"> | Coins, Jump pads and, speed boosts | Nothing hurts yet and safe introduction of mechanics |
| Level02 | <img src="Docs/a3/level02.png" width="320"> | Static spikes, health & shields, and knockback | Jump pads clearing a spike pit, and climbing |
| Level03 | <img src="Docs/a3/level03.png" width="320"> | AI Guard (Patrol, Chase, and Return) | Evading the guards using speed boosts while avoiding spikes |
| Level04 | <img src="Docs/a3/level04.png" width="320"> | [new idea] | [combined with] |
| Level05 | <img src="Docs/a3/level05.png" width="320"> | [new idea] | [combined with] |
## The numbers
| Number | Value | Why this value |
|---|---|---|
| Patrol speed | [m/s] | [e.g. how it compares with your run speed] |
| Chase range | [m] | [e.g. what a guard can and can't see from its route] |
| Knockback | 6, Impuslse | Depends on the speed of the player, but on average it pushes you 1-2 units away from the hazard |
| I-frames | 1s | A good duration to tell the player they have been hit, and to clear and obstacle |
| Power-up duration | 20s for speed, infinite for shield | 20s gives ample amount of boost to cover the ground and get out of tight spots, infinite for the shield makes it fair as they give u a second chance |


## Short answers
**1. Why does a trigger need a Rigidbody on at least one of the two objects?**
The rigidbody component is a driver component for Unity physics, without it Unity does not simulate physics, and since Triggers are a part of Unitys physics system they will not work without it. Hence a Rigidbody component is required on atleast one of the objects interacting with the triggered object to trigger it.

**2. Which ForceMode do your knockback and your Part A force use, and why each?**
Used Impulse for both to apply a instant force impulse because it gives us a full change in momentum on the object.

**3. What does your code do when a second power-up is picked up while one is running?** it consumes it and restarts the effect of the powerup.

**4. Why can `==` fail as an arrival check, and when does it work?**
Sometimes the player can overshoot at highspeeds which causes a small difference to exist which can fail the `==` check. Difference in y axis can also cause `==` to fail as `==` checks for exact equality on each axis. 