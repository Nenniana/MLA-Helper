# MLA-Helper
MLA-Helper is a master's thesis project intended to enhance and increase transparency for the ML-Agents package in Unity.

## Dependencies
- "com.unity.ml-agents": "2.0.1",
- "com.unity.mathematics": "1.2.6",
- "com.unity.textmeshpro": "3.0.6"

## Showcase
### General
This a complete Unity project where MLA-Helper has been installed and set up for a simple numbers game and all dependencies installed. 
When play mode is enabled and an answer is requested, the game will generate two numbers between 0-10, and the trained model will attempt to decide if the sum is above or not above 10 and below or not below 10.
MLA-Helper will visualize the resulting model, input, actions, and action marks in the visualization overlay and inspector.

### Use
- Download Showcase Files.
- Open Showcase files as Unity 2021.3.18f1 project.
- Start the scene, and click 'Request Answer' to initialize the first response.
- Drag and zoom visualization to view layers.

## Package
### General
This custom package includes all elements within MLA-Helper, except dependencies, which must be installed alongside the package by the user.

### Use
- Install package and dependencies.
- Install the 'MLA-Helper-Visualization' layer:
  - Click on any scene component.
  - Click on the 'Layer' dropdown.
  - Select 'Add Layer...'
  - Add 'MLA-Helper-Visualization' as layer 31.
- Remove layer 31 from all camera culling masks in the original project.
