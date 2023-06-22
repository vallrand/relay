# Relay

### Changelog

1. Created empty 3D URP Template.
2. Using [Unity Pipe Runner](https://catlikecoding.com/unity/tutorials/swirly-pipe/) as reference added pipe generator, that bends meshes along the arcs.
3. Added system to map from 2D torus surface into 3D.
4. UI Placeholders (HUD, menu, upgrade screen)
5. Added Player/Camera controller (using Unity new input system)
6. Various enemy types *(Reuse Weapon/Health components)* and spawner logic.
7. Show level progress, allow to choose upgrade in between levels.

### Notes

1. Performance for Vertex recalculation for bent meshes can be improved in various ways
    1. recalculate vertex shader, won't be able to update **culling bounds** or **collision**.
    2. recompute as a background job.
2. Level layout should be pregenerated along with enemy placements. (config, not gameobjects). More flexibility with balancing. Last chamber can be special, etc.
