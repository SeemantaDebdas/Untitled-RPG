# Untitled RPG

[![Unity Version](https://img.shields.io/badge/Unity-6000.0.25f1-lightgrey.svg)]()
[![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey.svg)]()
[![Last Commit](https://img.shields.io/github/last-commit/SeemantaDebdas/Untitled-RPG)]()

---

## Introduction

**Untitled RPG** is a modular and scalable action-RPG prototype built in **Unity** that showcases a wide range of gameplay systems such as:

- 🎮 **Combat and traversal**, built using the **State Machine pattern**  
- 💬 **Branching dialogue and quest systems** 
- 🧰 **Inventory, shops, and abilities** designed using **MVC** and **Strategy patterns**  
- ⚙️ Highly **extensible architecture** leveraging **ScriptableObjects**, **Unity Events**, and **component-driven design**



> 🧪 The project aims to demonstrate clean code practices and powerful design patterns applied in a real-world Unity game, with a focus on **editor tooling, reusability, and gameplay polish**.

---

<details>
<summary><strong>📚 Table of Contents</strong></summary>

- [Introduction](#introduction)
- [Features](#features)  
  &nbsp;&nbsp;&nbsp;&nbsp;• [💬 Dialogue System](#dialogue-system)  
  &nbsp;&nbsp;&nbsp;&nbsp;• [🧰 Inventory & Shops](#inventory--shops)  
  &nbsp;&nbsp;&nbsp;&nbsp;• [🌀 Ability System](#ability-system)  
  &nbsp;&nbsp;&nbsp;&nbsp;• [🚶‍♂️ Movement System](#movement-system)  
  &nbsp;&nbsp;&nbsp;&nbsp;• [⚔️ Combat System](#combat-system)  
  &nbsp;&nbsp;&nbsp;&nbsp;• [🧭 Quest System](#quest-system)  
  &nbsp;&nbsp;&nbsp;&nbsp;• [🧩 Other Features](#other-features)
- [Contact](#contact)

</details>

---

## Features

### Dialogue System

The Dialogue System is designed to be both player-friendly and developer-extensible. Built with **Unity UI Toolkit**, it supports:

- 🧠 **Branching conversations** based on player choices and game events  
- 🗝️ **Conditions** like `hasItem`, `hasQuest`, or custom checks to enable/disable dialogue paths  
- ⚙️ **Triggerable game actions** directly from dialogue nodes (e.g., grant quest, add item)  
- 🧩 Structured using clean logic for modularity and ease of future expansion  

<table width="100%">
  <tr>
    <td align="center" valign="top" width="50%">
      <strong>Dialogue Editor</strong><br>
      <img 
        src="Untitled%20RPG/Assets/GithubImages/Dialogue%20Editor.png" 
        alt="Dialogue Editor" 
        width="480"
      />
    </td>
    <td align="center" valign="top" width="50%">
      <strong>Demo Video</strong><br>
      <a href="https://youtu.be/_hschBSbjhg" target="_blank">
        <img 
          src="https://ytcards.demolab.com/?id=_hschBSbjhg&title=Dialogue+System+Demo&lang=en&timestamp=1717545600&background_color=%230d1117&title_color=%23ffffff&stats_color=%23dedede&width=480&border_radius=10&duration=65" 
          alt="Dialogue System Demo"
          width="480"
        />
      </a>
    </td>
  </tr>
</table>


### Inventory & Shops

The **Inventory System** uses the **Model-View-Controller (MVC)** pattern to keep logic, UI, and data clearly separated.

- **Main Inventory**: A drag-and-drop grid with item stacking support. Great for general storage and organizing loot.
- **Action Inventory**: A separate quick-use bar for items like potions or spells. These can be triggered with number keys.

Features:
- Items can be dragged and dropped between both inventories.
- Only specific items are allowed in the action bar for better control.
- Stacking logic helps manage space efficiently.
- Designed for easy extension and modification.

The **Shop System** ties directly into the inventory:
- Buy and sell items at different prices.
- Items move instantly into or out of the inventory.
- Includes **filtering logic** to help sort items by category or type.

This system not only makes inventory management smoother but also showcases real-time system interactions using Unity Events and modular architecture.

### Ability System

The **Ability System** is built using the **Strategy Pattern**, where each behavior of an ability (damage, healing, movement, animation, etc.) is separated into its own strategy class.

- Abilities are created by combining smaller, reusable behavior components.
- This allows new abilities to be made by recombining existing strategies—like snapping together LEGO bricks.
- The design ensures high **modularity** and easy extension for future mechanics.

<table width="100%" style="margin-left: auto; margin-right: auto;">
  <tr>
    <td align="center" valign="top" width="100%">
      <strong>Demo Video</strong><br>
      <a href="https://youtu.be/DL6pmtKwd_U?si=ZqW8LaAqkeHNEIMO" target="_blank" style="text-decoration: none;">
        <img 
          src="https://ytcards.demolab.com/?id=DL6pmtKwd_U&title=Dialogue+System+Demo&lang=en&timestamp=1717545600&background_color=%230d1117&title_color=%23ffffff&stats_color=%23dedede&width=480&border_radius=10&duration=65" 
          alt="Dialogue System Demo"
          width="480"
          style="border-radius: 10px;"
        />
      </a>
    </td>
  </tr>
</table>

### Movement System

The movement system is built using the **State Machine pattern**. Key aspects include:

- Both player and enemy movements are managed through distinct states, each encapsulating its own logic.
- State transitions are configured via the Unity Inspector.
- New states and transitions can be added or removed easily without affecting existing functionality.
- This modular design keeps the movement logic flexible and maintainable.

<table width="100%">
  <tr>
    <td align="center" valign="top" width="50%">
      <strong>Dialogue Editor</strong><br>
      <img 
        src="Untitled%20RPG/Assets/GithubImages/State%20logic.png" 
        alt="Dialogue Editor" 
        width="480"
      />
    </td>
</table>

### Combat System

The combat system also uses the **State Machine pattern**, inspired heavily by the Arkham series. Key features include:

- 🛡️ Enemies coordinate to surround the player.
- 🎯 Enemies attack the player one at a time in an organized sequence.
- 🚶‍♂️ The player can select enemies and smoothly travel to them before executing an attack.
- ⚡ Enemies can be countered and stunned by the player.
- 🥋 Includes takedown mechanics for strategic combat finishes.
e.

<table width="100%" style="margin-left: auto; margin-right: auto;">
  <tr>
    <td align="center" valign="top" width="100%">
      <strong>Demo Video</strong><br>
      <a href="https://youtu.be/WtDJNf7_UFg?si=yXxq6v3jVbn9-k_1" target="_blank" style="text-decoration: none;">
        <img 
          src="https://ytcards.demolab.com/?id=WtDJNf7_UFg&title=Demo+Video&lang=en&timestamp=1717545600&background_color=%230d1117&title_color=%23ffffff&stats_color=%23dedede&width=480&border_radius=10&duration=65" 
          alt="Demo Video"
          width="480"
          style="border-radius: 10px;"
        />
      </a>
    </td>
  </tr>
</table>


### Quest System

The Quest System is closely tied to the Dialogue System, making storytelling and progression feel natural and immersive.

- 🎯 **QuestGiver** components can be triggered directly from dialogue nodes, allowing NPCs to offer quests based on player choices or conditions.
- 🏁 **QuestCompletion** components let NPCs complete quests, rewarding the player immediately—commonly by updating the inventory system.
- 🧩 Designed to be modular and scalable for expanding the RPG experience.


<table width="100%" style="margin-left: auto; margin-right: auto;">
  <tr>
    <td align="center" valign="top" width="100%">
       <strong>Demo Video</strong><br>
      <a href="https://youtu.be/W0RbNd481cw?si=Vz7hg2wN0JKsIPXz" target="_blank" style="text-decoration: none;">
        <img 
          src="https://ytcards.demolab.com/?id=W0RbNd481cw&title=Quest+System+Demo&lang=en&timestamp=1717545600&background_color=%230d1117&title_color=%23ffffff&stats_color=%23dedede&width=480&border_radius=10&duration=65" 
          alt="Quest System Demo"
          width="480"
          style="border-radius: 10px;"
        />
      </a>
    </td>
  </tr>
</table>


### Other Features

- 🖼️ **UI Functionality via State Machine Pattern**  
  Menus, HUDs, and pop-ups are controlled through a clean state-based system, enabling consistent transitions and easy expansion.

- 🧠 **ScriptableObject-based Event System**  
  Enables decoupled and reusable event broadcasting—making communication between systems like inventory, combat, and UI seamless and modular.

- 🤖 **Lightweight NPC System with Behavior Graphs**  
  Non-playable characters use Unity’s **Behavior Graph (Visual Scripting)** for basic AI, allowing quick creation of ambient world behavior without code.

- 🎮 **Input System Integration**  
  Built using Unity’s **New Input System**, supporting rebinding and flexible input handling for keyboard, mouse, and controllers.

- 🧱 **Flexible Interaction System**  
  Designed to easily handle different types of in-world interactions (items, chests, NPCs, etc.) using a mix of component and interface-driven architecture.


## Contact

* LinkedIn: **[Seemanta Debdas](https://www.linkedin.com/in/seemanta-debdas-a65b00220/)**
