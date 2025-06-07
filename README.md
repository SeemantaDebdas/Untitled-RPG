# Untitled RPG

[![Unity Version](https://img.shields.io/badge/Unity-[6000.0.25f1]-lightgrey.svg)]()
[![Platform](https://img.shields.io/badge/Platform-[Windows]-lightgrey.svg)]()
[![Last Commit](https://img.shields.io/github/last-commit/[SeemantaDebdas]/Untitled-RPG)]()

## Table of Contents

* [Introduction](#introduction)
* [Features](#features)
    * [Dialogue System](#dialogue-system)
    * [Ability System](#ability-system)
    * [Combat System (Example)](#combat-system-example)
    * [Movement System (Example)](#movement-system-example)
    * [User Interface (UI)](#user-interface-ui)
    * [Art Style & Visuals](#art-style--visuals)
    * [Sound Design & Music](#sound-design--music)
    * [Other Features](#other-features)
* [Getting Started](#getting-started)
    * [Prerequisites](#prerequisites)
    * [Installation](#installation)
* [How to Play](#how-to-play)
* [Contributing](#contributing)
* [License](#license)
* [Contact](#contact)

---

## Introduction

This project explores classic RPG mechanics like combat, dialogue, quest, and inventory systems. A key focus is on clean coding and applying design patterns such as State Machine, MVC, Strategy, and Observer.
---

## Features

### Dialogue System

This game features a custom dialogue system created with Unity's UI Toolkit. It allows for branching conversations based on player choices and game events, using custom conditions (like hasItem and hasQuest) to prevent repetition.

Crucially, individual dialogue nodes can also trigger events. This means a conversation can directly lead to gameplay changes, like granting new objectives with Add Quest or providing rewards via Add Inventory Item.

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








### Ability System

Explain how the **ability system** functions. What types of abilities are there? How are they acquired and used? Discuss any interesting mechanics or design choices.

**Visuals:**

* ![Screenshot of the ability UI](assets/ability_ui_screenshot.png)
* ![GIF showing an ability in action](assets/ability_action_gif.gif)
* [Link to a video demonstrating different abilities](https://www.youtube.com/watch?v=your_ability_video_link)

### Combat System (Example)

If your game has combat, describe its core mechanics. What types of attacks are there? Are there different enemy types with unique behaviors? How does player progression relate to combat?

**Visuals:**

* ![Screenshot of a combat encounter](assets/combat_screenshot.png)
* ![GIF illustrating a combat scenario](assets/combat_gif.gif)
* [Link to a video showcasing the combat system](https://www.youtube.com/watch?v=your_combat_video_link)

### Movement System (Example)

Describe how the player character moves within the game world. What types of movement are supported (e.g., walking, running, jumping, flying)? Are there any unique movement mechanics?

**Visuals:**

* ![GIF demonstrating the movement system](assets/movement_gif.gif)
* [Link to a video showcasing different movement types](https://www.youtube.com/watch?v=your_movement_video_link)

### User Interface (UI)

Detail the important aspects of your game's **UI**. How are key game elements presented to the player (e.g., health, score, inventory)? Discuss any unique UI elements or design choices.

**Visuals:**

* ![Screenshot of the main game UI](assets/main_ui_screenshot.png)
* ![Screenshot of the inventory screen](assets/inventory_screenshot.png)

### Art Style & Visuals

Describe the overall **art style** of your game. What were your inspirations? Highlight any unique visual features or techniques you employed.

**Visuals:**

* ![Screenshot showcasing the game's environment](assets/environment_screenshot.png)
* ![Screenshot highlighting a character design](assets/character_design_screenshot.png)

### Sound Design & Music

Discuss the **sound design** and **music** in your game. What kind of atmosphere do they create? Are there any notable sound effects or musical themes?

**Visuals:**

* *(While you can't directly show sound, you can describe scenes where sound plays a key role and potentially link to a gameplay video that highlights the audio.)*
* [Link to a gameplay video with audio showcasing the sound design and music](https://www.youtube.com/watch?v=your_audio_showcase_video_link)

### Other Features

*(Add more subsections here for any other significant features your game includes, following the same format of description and visuals.)*

---

## Getting Started

Explain how someone can get the project up and running on their own machine.

### Prerequisites

List any software or tools that are required to open and run the Unity project (e.g., specific Unity version, external libraries).

* **Unity** \[Your Unity Version] or higher
* \[List any other dependencies, e.g., Visual Studio]

### Installation

Provide step-by-step instructions on how to clone the repository and open the project in Unity.

1.  **Clone** the repository: `git clone [Your_Repository_Clone_URL]`
2.  Open the **Unity Hub**.
3.  Click "**Add**" and select the cloned project folder.
4.  **Open** the project.

---

## How to Play

Provide basic instructions on how to play your game. What are the controls? What is the main objective?

* **Controls:**
    * Movement: **[Key(s)]**
    * Interact: **[Key(s)]**
    * Jump: **[Key(s)]**
    * Attack: **[Key(s)]**
    * \[Add more controls as needed]
* **Objective:** \[Explain the main goal of the game]

---

## Contributing

If you are open to contributions, explain how others can contribute to your project. Mention things like bug reports, feature requests, and pull requests.

1.  **Fork** the repository.
2.  **Create** a new branch (`git checkout -b feature/your-feature-name`).
3.  **Make** your changes.
4.  **Commit** your changes (`git commit -am 'Add some feature'`).
5.  **Push** to the branch (`git push origin feature/your-feature-name`).
6.  **Open a pull request**.

Please follow our [Code of Conduct](CODE_OF_CONDUCT.md) (optional).

---

## License

Specify the license under which your game is released. If you have a `LICENSE` file in your repository, link to it here.

This project is licensed under the **[Your License] License** - see the [LICENSE](LICENSE) file for details.

---

## Contact

Provide ways for people to contact you regarding the project (e.g., email, social media).

* Email: **[Your Email Address]**
* Twitter: **[@YourTwitterHandle]**
* Discord: **[Your Discord Link/Username#Tag]**
