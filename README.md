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

### General Setup
- Install package and dependencies.
- Install the 'MLA-Helper-Visualization' layer:
  - Click on any scene component.
  - Click on the 'Layer' dropdown.
  - Select 'Add Layer...'
  - Add 'MLA-Helper-Visualization' as layer 31.
- Remove layer 31 from all camera culling masks in the original project.

### Use - New ML-Agents Project
- Follow 'General Setup' instructions.
- Create a new agent script that inherits from the MLAHelperAgent class.
- Fill in the desired Vector Observation Space Size and all Discrete Branches for the agent's Behavior Parameters in the editor.
- Open the MLA-Helper Central Hub by clicking on MLA-Helper in the Unity Menu.
- Supply the MLAHelperAgent in the 'Input ML-Agents agent,' whereafter all information will be loaded.
- Follow 'Observations and Action Masks for a loaded model' instructions.
- Follow 'Hookup Observations and Action Masks to project' instructions.
- Once the Model has been trained, open the Central Hub, ensure all information is loaded correctly, and click 'Deploy MLA-Helper in Current Scene.'

### Use - Existing ML-Agents Project
- Follow 'General Setup' instructions.
- Ensure that any ML-Agents agent inherits from MLAHelperAgent instead of the Agent class (MLA-Helper works with only one agent at a time).
- Fill in the Model, Vector Observation Space Size, and all Discrete Branches for the agent's Behavior Parameters.
- Open the MLA-Helper Central Hub by clicking on MLA-Helper in the Unity Menu.
- Supply the MLAHelperAgent in the 'Input ML-Agents agent,' whereafter all information will be loaded.
- Follow 'Observations and Action Masks for a loaded model' instructions.
- Click 'Deploy MLA-Helper in Current Scene.'
- Follow 'Hookup Observations and Action Masks to project' instructions.

### Observations and Action Masks for a loaded model
- Fill out Observations and Action Masks:
  - Action Masks:
    - The action mask collection will be filled with the required action masks for the loaded model, as well as their branch and index.
    - It is recommended to give every action mask a descriptive name, as it will help identify actions chosen for the agent.
    - Click 'Create All Uninitialized Action Masks' to create action masks for all values.
  - Observations:
    - Fill the observation collection with all needed observations. MLA-Helper will inform you of how many are needed in relation to the loaded model.
    - It is recommended to give every observation a descriptive name, as it will help identify each observation during runtime.
    - Click 'Generate Reference' to create each new observation.
   
### Hookup Observations and Action Masks to project
- Add observation references to existing scripts and attach previously created referenced observations hereto in the inspector.
- Add action mask references to existing scripts and attach previously created referenced action masks hereto in the inspector.
- Set whether to use constant or dynamic information for each reference.
