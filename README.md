Reinforcement Learning Integration in Unity Game
Overview
This project demonstrates the integration of reinforcement learning (RL) within a Unity game environment. The project focuses on training autonomous agents using RL algorithms, enabling them to learn optimal behaviors within the game world. The training process has been fully implemented, and this guide will walk you through setting up and running the project.
Table of Contents
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Project Structure](#project-structure)
- [Training the Agents](#training-the-agents)
- [Running the Project](#running-the-project)
- [Customization](#customization)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)
Prerequisites
Before you begin, ensure you have the following software installed on your machine:
- Unity (version X.X.X or later)
- Python (version 3.X.X or later)
- ML-Agents Toolkit (version X.X.X)
Installation
1. **Clone the Repository**
```bash
git clone https://github.com/yourusername/rl-unity-integration.git
cd rl-unity-integration
```

2. **Set Up the Unity Project**
- Open Unity Hub and click on "Open" to import the cloned project.
- Ensure that the Unity version matches the project's version specified in the `ProjectSettings/ProjectVersion.txt`.

3. **Install Python Dependencies**
- Navigate to the `python` folder within the project directory.
- Install required Python packages:
```bash
pip install -r requirements.txt
```

4. **Set Up ML-Agents**
- Follow the [ML-Agents installation guide](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Installation.md) to ensure that the ML-Agents Toolkit is correctly configured.
Project Structure
- `Assets/`: Contains all Unity assets, including scripts, scenes, and prefabs.
- `python/`: Contains Python scripts used for training agents.
- `TrainingResults/`: Stores training results, including models and logs.
Training the Agents
To train the agents using reinforcement learning:

1. **Configure the Training Environment**
- Open the `TrainingScene` in Unity.
- Modify agent parameters in the Unity Inspector as needed.

2. **Start the Training Process**
- Ensure that the Unity editor is running.
- In the command line, navigate to the `python` directory.
- Run the training script:
```bash
mlagents-learn config/trainer_config.yaml --run-id=<your-run-id> --train
```
- The training progress will be displayed in the terminal, and results will be saved in the `TrainingResults/` directory.

3. **Monitor Training**
- Use TensorBoard to visualize training metrics:
```bash
tensorboard --logdir=TrainingResults
```
Running the Project
Once training is complete, you can test the trained agents within the game:

1. **Switch to Play Mode**
- Open the `GameplayScene` in Unity.
- Press the Play button in the Unity Editor.

2. **Observe Agent Behavior**
- The trained agents should now exhibit learned behaviors based on the training.
Customization
- **Modify the Game Environment:**
  - You can customize the game environment by editing the `GameplayScene` and adjusting game objects, terrain, and other elements.

- **Change Agent Behavior:**
  - To retrain agents with different behaviors, modify the training configurations in the `config/trainer_config.yaml` file and repeat the training process.
Troubleshooting
- **Issue: Unity Editor crashes during training.**
  - **Solution:** Ensure that your system meets the required hardware specifications and that Unity is updated to the latest version.

- **Issue: Training process is slow.**
  - **Solution:** Reduce the training complexity by lowering the number of agents or simplifying the environment.
Contributing
Contributions are welcome! Please fork the repository and submit a pull request with your changes. For major changes, please open an issue first to discuss what you would like to contribute.
License
This project is licensed under the MIT License. See the LICENSE file for details.
