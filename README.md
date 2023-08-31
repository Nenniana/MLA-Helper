# MLA-Helper
MLA-Helper is a master's thesis project intended to enhance usability and increase transparency for the ML-Agents package in Unity.

## Features
- Visualization of loaded ONNX model during runtime.
- Visualization of observations, actions, and action marks with label information to enable easier debugging.
- Switching between constant and dynamic observation and action mask data.
- Complete decoupling between observations and action masks in ML-Agents and the rest of the project.

## General Notes
- Dynamic Observations and Action Masks values will always reflect the current state space, whereas Visualization values reflect the previous space.
- MLA-Helper only supports visualization of a single agent at a time.
- MLA-Helper currently only supports discrete action space.

## Dependencies
- "com.unity.ml-agents": "2.0.1",
- "com.unity.mathematics": "1.2.6",
- "com.unity.textmeshpro": "3.0.6"

## Showcase Project
### General Information
This a complete Unity project where MLA-Helper has been installed and set up for a simple numbers game and all dependencies installed. 
When play mode is enabled and an answer is requested, the game will generate two numbers between 0-10, and the trained model will attempt to decide if the sum is above or not above 10 and below or not below 10.
MLA-Helper will visualize the resulting model, input, actions, and action marks in the visualization overlay and inspector.

### Use
- Download Showcase Files.
- Open Showcase files as Unity 2021.3.18f1 project.
- Start the scene, and click 'Request Answer' to initialize the first response.
- Drag and zoom visualization to view layers.

## Package
### General Information
This custom package includes all elements within MLA-Helper, except dependencies, which must be installed alongside the package by the user.

### Use - New ML-Agents Project:
1. Follow the '**General Setup**' instructions.
2. Create a new agent script that inherits from the _MLAHelperAgent_ class.
3. Fill in the desired **Vector Observation Space Size** and all **Discrete Branches** for the agent's _Behavior Parameters_ in the editor.
4. Open the MLA-Helper Central Hub by clicking on MLA-Helper in the Unity Menu.
5. Supply the _MLAHelperAgent_ in the '**Input ML-Agents agent**', whereafter all information will be loaded.
6. Follow the '**Observations and Action Masks for a loaded model**' instructions.
7. Click '**Deploy MLA-Helper in Current Scene**'.
8. Follow **the 'Hookup Observations and Action Masks to project**' instructions.
9. Once the Model has been trained:
   1. Supply the ONNX Model in the ModelConstructor component of the deployed MLA-Helper prefab.
   2. Call '_RequestNewStep_' from MLAHelperAgent whenever the agent and visualization should update.

### Use - Existing ML-Agents Project:
1. Follow the '**General Setup**' instructions.
2. Ensure that any ML-Agents agent inherits from _MLAHelperAgent_ instead of the _Agent_ class (MLA-Helper works with only one agent at a time).
3. Fill in the **Model**, **Vector Observation Space Size**, and all **Discrete Branches** for the agent's _Behavior Parameters_ in the editor.
4. Open the MLA-Helper Central Hub by clicking on MLA-Helper in the Unity Menu.
5. Supply the _MLAHelperAgent_ in the '**Input ML-Agents agent**', whereafter all information will be loaded.
6. Follow the '**Observations and Action Masks for a loaded model**' instructions.
7. Click '**Deploy MLA-Helper in Current Scene**'.
8. Follow '**Hookup Observations and Action Masks to project**' instructions.
9. Call '_RequestNewStep_' from MLAHelperAgent whenever the agent and visualization should update.

### General Steps
#### General Setup:
1. Install package and dependencies.
2. Install the '**MLA-Helper-Visualization**' layer:
   1. Click on any scene component.
   2. Click on the 'Layer' dropdown.
   3. Select 'Add Layer...'
   4. Add '**MLA-Helper-Visualization**' as layer 31.
3. Remove layer 31 from all camera culling masks in the original project.

#### Observations and Action Masks for a loaded model:
1. Fill out Observations and Action Masks:
   1. Action Masks:
      1. The action mask collection will be filled with the required action masks for the loaded model, as well as their branch and index.
      2. It is recommended to give every action mask a descriptive name, as it will help identify actions chosen for the agent.
      3. Click '**Create All Uninitialized Action Masks**' to create action masks for all values.
   2. Observations:
      1. Fill the observation collection with all needed observations. MLA-Helper will inform you of how many are needed in relation to the loaded model.
      2. It is recommended to give every observation a descriptive name, as it will help identify each observation during runtime.
      3. Click '**Generate Reference**' to create each new observation.
   
#### Hookup Observations and Action Masks to project:
1. Add observation references to existing scripts and attach previously created referenced observations hereto in the inspector.
2. Add action mask references to existing scripts and attach previously created referenced action masks hereto in the inspector.
3. Set whether to use constant or dynamic information for each reference.
