# Adventurer's Guild v2.0
[ Not actually a sequel but the second major iteration of a game idea I've been working on and designing for a while ] 

Still _very_ early in development.

## Game Pitch
Play as a retired adventurer who now leads a quest-completing, dungeon-delving, dragon-slaying group of would-be heroes. Renovate a run-down fortress to act as your base of operations and build up the most prestigious and glorious guild of adventurers the realms have ever seen!

- Repair a beat-down fortress to unlock new features and benefits for your adventurers!
- Explore a procedurally generated map to uncover factions and quests in the world!
- Guide your adventurers through the events they face when overcoming quests but watch out - wrong decisions can have dire consequences!
- Grow your prestige, develop your parties and rid the land of evil!

## Cool tech stuff
This is a side project I've been working on for a few months now. It's taken a few redesigns for the game to come together and is not even ready for playtesting but it has allowed me to test and stretch my development muscles in many ways. It's set up using an MVC framework where the model layer represents the data types and non-Monobehavior logic in the game, the controller layer is where most of the Monobehaviors can be found and the view layer corresponds to the UI elements.

I've made a few cool tools to help with content generation for the game.

### The Quest Editor
In order to support the development of the questing system for this game, I made an extension to Unity to create a graph based editor to string together the events that make up a quest. This editor saves these quest trees to JSON objects which are then loaded at runtime to make up the content for the game.

![image](https://github.com/user-attachments/assets/154db131-97e0-44a7-a0e8-6cea0a3a1041)

This allows me to create and easily maintain the conditions for the branching paths in a quest tree as well as balance and tune them for difficulty.

### The Dialogue Editor
Using the quest editor as a base, I also created a dialogue editor to set up the dialogue for cutscenes in game. This dialogue editor is [Twine](https://twinery.org/) compliant, which allows the extension of the Twine dialogue objects with logic and markers from the actual game itself.

The Dialogue Editor still has some work that needs to be done to fully separate its base functionality from the quest editor but given I'm not currently focused on the development of the dialogue system in game, it's not highest priority to work on right now.

## The Procedural Map
I've had some experience making maps from noise models and randomness before but I wanted to try something different for this game. Rather than just randomly creating a very large map and then trying to populate it to be interesting, I've taken inspiration from the board game Carcassone, where tiles in the map need to have corresponding edge types (that is to say, a map tile with one side ending in a road needs to meet with another map tile with a road side). This way as the player explores the map, it populates by seeing which sides it already knows about and creates terrain to match, otherwise it picks a random terrain type that future revealed tiles will have to match. See the image below for details.

![image](https://github.com/user-attachments/assets/f859bb92-5ab5-4b81-8c69-6019c388c4c0)

While the art could still use some work, the system works well and can support extension for new terrain types. I'm currently looking to extend this system even further by looking into related piece-wise random generation algorithms like Wave Function Collapse.

