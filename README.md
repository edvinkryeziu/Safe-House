# Safe House
> A first-person horror game built in Unity where you are hunted by a procedurally animated spider creature through a dark forest. Your only safety lies in the scattered houses and shelters across the map.
> 
<img width="500" height="300" alt="ProceduralLegShowcase" src="https://github.com/user-attachments/assets/753f8fc0-98bf-4426-ac2b-76724902567c" />
<img width="500" height="300" alt="InventoryShowcase" src="https://github.com/user-attachments/assets/4726152c-d7c9-4f2d-ac0d-40a4372a5540" />
<img width="500" height="300" alt="ChaseShowcase" src="https://github.com/user-attachments/assets/be266df4-136a-4e54-a8f8-ea3b79f23700" />

---

## Overview
Safe House is a survival horror game developed in Unity using C#. The game features a fully custom procedurally animated spider enemy, a modular inventory system, and immersive first-person gameplay. The project was built from scratch as a learning exercise to develop deep knowledge of Unity systems, C# architecture and game development patterns.

---

## Key Features

- **Procedural Spider Animation** — 8-legged spider enemy with fully procedural leg movement using Inverse Kinematics, terrain-adaptive body height and surface normal rotation
- **Modular Inventory System** — hotbar with item picking, dropping, swapping and equipping, built entirely on C# events and interfaces for reusability across projects
- **Enemy AI State Machine** — patrol, chase and idle states with field of view detection, line of sight raycasting and NavMesh pathfinding
- **Interaction System** — interface-driven interaction for doors, pickable items, notes and weapons
- **Equipment System** — dynamic item equipping with weapon sway, attack animations and item-specific behaviour via IUsable interface
- **First Person Controller** — smooth movement, camera look, stamina system with cooldown, head bob and footstep audio
- **Note/Document System** — in-world readable notes with typewriter text effect and optional inventory pickup
- **Settings System** — resolution, fullscreen, graphics quality and audio volume with PlayerPrefs persistence
- **Trigger Zone System** — modular UnityEvent-based trigger zones for tutorials, cutscenes and world events

---

## Technical Highlights

### Procedural Leg Animation
The most technically challenging system in the project. Each of the spider's 8 legs independently:
- Raycasts downward from a step origin point to detect ground position
- Triggers a step when horizontal distance from the planted position exceeds a threshold (XZ-only distance to prevent Y-axis jitter)
- Smoothly lerps the IK target to the new ground position using a fixed-duration progress value
- Arcs upward mid-step using a sine curve for natural foot lift
- Coordinates with the opposite leg to prevent simultaneous stepping

Body height is calculated by averaging **planted leg Y positions only** (stepping legs excluded), with outlier exclusion using median filtering to prevent extreme terrain from spiking the body upward. Body rotation uses averaged surface normals from all leg raycasts fed into `Quaternion.LookRotation`.

### Modular Inventory Architecture
Built entirely on C# events and interfaces — no direct script references between systems:
- `IPickable` and `IInteractable` interfaces decouple interaction detection from item behaviour
- Static C# events (`Action<T>`) pass data between `InventorySystem`, `InventoryUI` and `EquipmentSystem`
- `ItemData` ScriptableObjects define all item data independently of scene objects
- `IUsable` interface allows any equipped item to define its own behaviour without modifying `EquipmentSystem`

### Enemy AI
State machine with three states (Idle, Patrol, Chase) driven by:
- Distance-based patrol triggering with random waypoint selection
- Field of view detection using dot product angle calculation
- Line of sight verification via raycast before entering chase state
- Persistent chase until player exceeds `loseRange` distance

---

## Technologies Used
- **Unity 6 LTS** (6000.3.x)
- **C#**
- **Universal Render Pipeline (URP)**
- **Unity Animation Rigging** — Two Bone IK for procedural leg animation
- **Unity NavMesh** — Enemy pathfinding
- **New Input System** — All player input handling
- **TextMeshPro** — UI text rendering

---

## Project Structure
Since this repository contains scripts only, here is a brief overview of the key scripts:

| Script | Description |
|--------|-------------|
| `PlayerController.cs` | Movement, jumping, sprinting, stamina |
| `CameraController.cs` | Mouse look, head bob, inventory state |
| `InventorySystem.cs` | Item storage, pickup, drop logic |
| `InventoryUI.cs` | Hotbar display, slot interaction, item moving |
| `EquipmentSystem.cs` | Item equipping, weapon sway, IUsable handling |
| `EnemyAI.cs` | State machine, FOV detection, NavMesh chasing |
| `ProceduralLeg.cs` | Per-leg IK target positioning and step logic |
| `EnemyBody.cs` | Body height and rotation from leg data |
| `PlayerInteraction.cs` | Raycast-based interaction and pickup detection |
| `Door.cs` | Smooth door rotation with audio |
| `NoteItem.cs` | In-world readable notes with typewriter effect |

---

*Developed as a learning project to build expertise in Unity, C# systems design and game development patterns.*
