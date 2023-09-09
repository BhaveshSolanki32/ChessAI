# Chess AI
This is a unity project made using c#. The main focus of this project was to make a working chess AI using minimax algorithm while maintaining industry standard code and project architecture. 
The game consist of 2 mode "human vs human" and "human vs AI".

**Downloadable demo**: https://bhavesh-solanki.itch.io/chess-minimax-ai


## Table of content

- [About](#About)
- [Screenshots/gifs](#Project-screenshots/GIFs)
- [Key features](#Key-features)
- [Installation](#Installation)
- [Usage](#Usage)
- [Code overview](#Code-overview)
- [Future plan](#Future-plan)
- [Acknowledgement](#Acknowledgement)
## About
The project implements the classic game of chess with it's AI which implementes using minimax algorithm.
The minimax algorithm creates a tree where each node represent a possible game state the algorithm goes to certain defined depth after reaching to max depth it calculates a score for each node at leaf stage using a heuristic function currently the function calculates the material advantage and piece mobility (heuristic function will be expanded with further updates). 
## Project screenshots/GIFs
## How the Algorithm works

The algorithm has mainly 2 component minimax algo which creates and traverse possiblity tree and a heuristic function that calculates score for a given board. Currently the heuristic function  considers piece mobility and material advantage. 

### Minimax algorithm
Minimax is a tree searching algorithm, popular for it's use in strategy turn based games like chess, tic tac toe etc. 
It is fundamental for creating intelligent game-playing agents that can make optimal decisions.

At its core, Minimax seeks to find the best possible move for a player by considering both its own and the opponent's potential moves. It operates as follows:

1. **Tree Generation**: The algorithm constructs a game tree that represents all possible moves and counter-moves for a given depth. Each level of the tree alternates between the player's and the opponent's turn.

2. **Evaluation**: A heuristic evaluation function is applied to the leaf nodes of the tree. This function quantifies the desirability of the game state for the player. In chess, for instance, this could involve factors like piece values, board control, and king safety.

3. **Backpropagation**: The evaluation scores are propagated up the tree. The player aims to maximize their score (hence "max" in Minimax), while the opponent aims to minimize it (hence "min").

4. **Decision Making**: As the algorithm traverses the tree, it chooses the move that leads to the highest-scored outcome for the player at the root node. This decision-making process assumes perfect play from both sides.

5. **Depth-Limited Search**: Minimax can be computationally expensive for deep trees, so it is often combined with depth-limited search or pruning techniques like Alpha-Beta pruning to improve performance.

### Heuristic function
Heuristic function  is used for evaluating state of the game based on various factors like piece mobility and material advantage.

The heuristic function works on the  formula:

**Σ(fi * wi)**

Here, fi represents the score returned by various evaluation factors (such as piece mobility and material advantage), and wi represents their associated weights or biases.

* **Piece Mobility**: This aspect of the heuristic function assesses how actively and effectively the player's pieces can move on the board. Higher piece mobility can be an indicator of a stronger position, as it allows for more tactical options and control over the board.

* **Material Advantage**: Material advantage takes into account the relative values of the pieces on the board. For example, in chess, queens are worth more than knights, which are worth more than pawns. Evaluating material balance helps the algorithm understand who has the upper hand in terms of the quantity and quality of pieces.

The heuristic function computes a score for each game state by applying this formula. The weights (wi) associated with each evaluation factor allow you to fine-tune the importance of these factors in the overall evaluation. Adjusting these weights can significantly impact the AI's decision-making process, enabling you to prioritize certain aspects of the game state based on your strategic objectives. 

The higher the score the better the game sate or postion is considerd. (Currently the heuristic function is not well tuned).
## Key features
* **Grid generation system** for spawning map tiles as gameobjects, with an editor script to help generate tileset in editor mode based on given parameters.
* **Chess game** with complete implementation of pieces movement/ legal moves, players turn handler,enemy piece capture etc. 
* **industry standard code** is used throughout the project to check the project the maintainablity and expansion of the project and to ease the workflow by using SOLID principles and observer pattern for one to many interaction and singleton for tools like world vector to grid vector conversation.

* **Minimax AI** the project successfully implements chess ai using minimax algorithm 

* **Heuristic function** uses currently uses 2 methods to calculate the score  of the chess game state namely piece mobility, material advantage.
## Installation

#### Build/final game
1. You can download the latest build from https://bhavesh-solanki.itch.io/chess-minimax-ai
2. Run the exe file to start the game

#### Project
1. Download the github repo. 
2. Open unity hub add the downloaded project. 
3. Open the project using unity 2021.3.16f1.
4. You are all set to play around and explore the project
## Code flow/Key scripts

1. **InputReceiver** reads an input from the user and forwards it to **InputTaskHandler**. 

2. **InputTaskHandler** acts as an mediator between the input layer and other layers. If a piece is selected it forwards it to **MovePiece** otherwise if a piece is clicked on, it forwards it to **SelectPiece**. 

3. **SelectPiece** gets all the possible tiles player can move to from **Ipiece** component of the selected piece and triggers an event that informs **DisplayTiles** to display the movable tiles. And now we wait for player to select a different piece or select a tile where player piece can move to.

4. Once the player clicks on the tile player can move to **InputTaskHandler** calls **MovePiece**. Which Moves the Piece to desired location and triggers event that tells the game a piece is moved.  It is this event that tells the game the player chance has ended.

5. In human vs human game the **InputReceiver** switches it's raycasting layer to different player pieces so it only takes input for that player.

6. In a human vs AI the **MovePiece** called event triggers a function in **AiTurnHandler** that collects all the requirement for the AI and forwards it to an Interface **IMiniMax** which is implemented by all minimax scripts (we have different minimax scripts for different types of minimax like one that implementes jobs, another is simple minimax).

7. **IMiniMax** returns a list of all root node of the minimax tree with score so **AiTurnHandler** can decide the which move to play based on difficulty. 

8. After getting the list possible moves with scores (currently AI chooses best move). It calls **MovePiece** to move the piece.
## Future plans

* **Optimization:** At the current stage lot optimization can be used to improve the Ai and make it more performant like move ordering (exploring better moves first) can significantly improve alpha beta pruning, time based deepening it can help us to remain in time limit and without compromising a lot on the output.

* **Multithreading:** (in progress) Multithreading can help significantly improve the performance and speed by parallelizing the task and dividing the tree into seveeral other smaller tree and solving them simultaneously. 

* **Heuristic function:** Heuristic function is a significant part of the ai I plan to improve the heuristic function by adding more criteria to it like king safety, pawn structure etc. Also improving the weights/bias of the heuristic function can improve the Ai perform better.

* **Machine learning:** A rudimentary form of machine learning can be used specifically supervised learning for tuning the weights/biases of the function, this is called feature tuning.

* **Developer menu:** Developer menu will be used for setting depth, chnaging biases, turning heuristic function methods on/off etc.

* **Difficulty adjustment:** Adding Difficulty adjuster for player to choose between the difficulty for which the ai will out the move accordingly.
## Acknowledgement


 * [Algorithms Explained – minimax and alpha-beta pruning by Sebastian lague](https://www.youtube.com/watch?v=l-hh51ncgDI)

  * [Chess pieces art by John Pablok](https://opengameart.org/content/chess-pieces-and-board-squares)
 

