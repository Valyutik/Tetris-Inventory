# Tetris Inventory – Team Educational Project

## Project Overview

**Tetris Inventory** is a prototype of a game inventory system where items occupy multiple grid cells and may have their own shape and orientation.

This project was developed as our first team-based practice. Alongside the technical implementation, we focused on learning collaboration workflows, version control, and structured development processes.

### Project Goals

- Implement a functional and intuitive inventory UX;
- Design a system composed of multiple independent modules;
- Practice team workflows (Git, Trello, task distribution, code review).

---

## Technical Stack

### Core Technologies
- **Unity**
- **C#**
- **UI Toolkit** (UXML/USS)

### Architectural & Applied Approaches
- MVP pattern for UI and logic separation (Model / View / Presenter)
- Modular runtime structure
- Configurable item data using `ScriptableObject`
- Drag-and-drop system for item interaction

### Team Tools
- **Git + GitHub** for version control
- **Trello** for task management and tracking
- Pull Request workflow for integrating changes

---

## Key Features

At its current stage, the project supports:

- Grid-based inventory with items of various sizes;
- Item rotation within the inventory;
- Drag-and-drop item interaction;
- Item deletion via a dedicated drop zone;
- Item generation based on predefined rules;
- Additional storage area (stash);
- Popups and tooltips for user feedback;
- Basic UI-related audio feedback.

---

## Architecture Overview

The project is divided into several major layers:

### 1. Core
Base contracts, entry point, and model storage.

### 2. Inventory Domain
Inventory logic including:
- Item model
- Grid and cells
- Item generation
- Rotation logic
- Drag-and-drop handling
- Stash/storage logic

### 3. UI Layer (UI Toolkit)
UXML/USS documents and visual components for screens, buttons, popups, and supporting UI elements.

### 4. Systems / Services
High-level services such as audio, content management, and orchestration logic.

### Why This Structure Matters

- Business logic is isolated from the visual layer, making it easier to test and extend.
- New features can be implemented modularly without rewriting the entire system.
- Enables parallel development across different subsystems within the team.

---

## Team & Workflow

The project was developed by a **team of three developers** as our first collaborative development experience.

### Work Organization

- Created a backlog and broke features into small tasks;
- Managed tasks using a **Trello** board;
- Worked in Git branches (`feature/fix/chore`) to protect the main branch;
- Integrated changes via Pull Requests;
- Discussed architectural decisions before merging.

### Simplified Git Flow (for educational context)

- `master` — stable version;
- Feature branches for isolated development;
- Pull Request + review + merge into main branch;
- Short iterations with frequent synchronization.

---

## Demo

Below is a demonstration of the inventory system:

![Gameplay Demo](gameplay.gif)

The GIF demonstrates:
- Drag-and-drop interaction
- 90° item rotation
- Placement validation
- Collision handling

---

## Possible Improvements

- Extend placement rules (complex shapes, constraints, exceptions);
- Implement save/load functionality;
- Improve UX (animations, conflict highlights, position previews);
- Strengthen error handling and user guidance;
- Add automated tests for core logic;
- Improve performance and scalability with a large number of items.