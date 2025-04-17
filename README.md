# BaseGameLayout

**BaseGameLayout** is a foundational Unity project designed to streamline the creation of new games by including common structures and tools. It integrates key frameworks to enhance sound design and dependency management, making it an efficient starting point for future projects.

---

## Features

- **Pre-configured Wwise**: Seamless music and sound design integration for dynamic audio experiences.
- **VContainer Integration**: A lightweight and flexible dependency injection (DI) framework, perfectly suited for clean, modular codebases.
- **Reusable Components**: Common utilities and base classes for rapid game development.

---

## Getting Started

### Prerequisites

- **Unity Version**: Tested with Unity [22.3.45f1].
- **Wwise**: Install the [Audiokinetic Wwise Unity Integration](https://www.audiokinetic.com/library/) for your version.
- **VContainer**: Add the [VContainer package](https://github.com/hadashiA/VContainer) via Unity Package Manager.

### Installation

1. Clone this repository:
   ```bash
   git clone https://github.com/QuietDreamGames/BaseGameLayout.git
   ```
2. Open the project in Unity.

3. Set up Wwise:
   - Follow the steps to integrate your Wwise project.
   - Update the Wwise settings under Edit > Project Settings > Wwise.
4. Verify VContainer setup:
   - Ensure that the VContainer package is included and configured for dependency injection.

---

### Usage
 - Starting a New Project:
    1. Clone this repository.
    2. Rename the folder and update the project name in Unity.
    3. Begin development with a pre-configured base.
 - Adding New Features:
   1. Use the VContainer-based DI to ensure modular and testable code.
   2. Extend or modify Wwise integration as needed for advanced sound design.

---

### Contributing
Contributions are welcome! If you have improvements or find issues, feel free to open a pull request or submit an issue.

---

### License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

### Acknowledgments
[Audiokinetic Wwise](https://www.audiokinetic.com/en/)
[VContainer](https://github.com/hadashiA/VContainer)