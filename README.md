# Police Interview
An experiment with a ChatGPT powered game.

This is a collaboration of Reed Berkowitz and Eric Evans. You can read more about it here:
[Blog Post](https://medium.com/p/6c47797d997a/ "AI In Games: Complicated Characters
Connecting LLM Driven Characters To The Game State
(on medium.com)")

In this demo, the game state and rules are enforced as the LLM is used to evaluate and generate natural language. 

## Playing the game

You can play it in a web-browser (using your own OpenAI API Key) here: [Playable Demo](https://barelyagame.itch.io/ai-police-interview/ "WebGL build, installed and running on itch.io.")

1. In the upper right of the game screen, look for the field to enter the OpenAI API key. You must put in a key with credits to access GPT-4.
2. Note that there are no keyboard shortcuts. When you enter the API Key, you must click the "Apply" button with the mouse. When you enter dialog to talk to Molly you have to click "Enter" after typing your message.


## How to run the game in the Unity Editor

The game is designed for a WebGL build. However, to see the inner workings you'll want to run it from within the Unity Editor.

1. Install Unity. The current build uses Unity version 2022.3.10f. Probably newer versions will work as well.
2. Clone the repo to your computer.
3. In the Unity Hub, choose Open > Add project from disc.
4. Click the project to open it in Unity.
5. Look in the Assets > Scenes folder and open the InterviewMollyStone scene.
6. Click the Play button to start the game.
7. A good place to start exploring is to click in the Hierarchy on the CharacterPrompt object and the other "Prompt" and "State" objects and look at their properties in the Inspector panel. Another object to look at is the InterviewOrchestrator.

