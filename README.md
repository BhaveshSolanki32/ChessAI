# Chess AI
This is a unity project made using c#. The main focus of this project was to make a working chess AI using minimax algorithm while maintaining industry standard code and project architecture. 
The game consist of 2 mode "human vs human" and "human vs AI".

**Downloadable demo**: https://bhavesh-solanki.itch.io/chess-minimax-ai


## Table of content

- <a href="#about">About</a>
- <a href="#project-screenshots/gifs">Screenshots/gifs</a>
- <a href="#HowAlgo">How the AI Algorithm works and its optimization</a>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1. <a href="#miniMax">MiniMax Algorithm</a>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 2. <a href="#heuristic">Heuristic function</a>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 3. <a href="#alphaBeta">Alpha beta pruning and other optimization</a>
- <a href="#keyFeatures">Key features</a>
- <a href="#installation">Installation</a>
- <a href="#flow">Code flow</a>
- <a href="#plans">Future plan</a>
- <a href="#Acknowledgement">Acknowledgement</a>

<a name="about"></a>
## About
The project implements the classic game of chess along with an AI that utilizes the minimax algorithm.

The minimax algorithm constructs a tree where each node represents a potential game state. The algorithm traverses this tree to a defined depth. Once it reaches the maximum depth, it calculates a score for each leaf node using a heuristic function. Currently, the heuristic function calculates material advantage and piece mobility. (Note that the heuristic function will be expanded in future updates). And traverse up the tree to give the score to each root node.

Feel free to reach out if you have any questions or need further information! 

<a name="project-screenshots/gifs"></a>
## Project screenshots/GIFs
<a name="HowAlgo"></a>
## How the AI Algorithm works and its optimization

The algorithm has mainly 2 component minimax algo which creates and traverse possiblity tree and a heuristic function that calculates score for a given board. Currently the heuristic function  considers piece mobility and material advantage. 
<a name="miniMax"></a>
### Minimax algorithm
Minimax is a tree searching algorithm, popular for its use in strategy turn based games like chess, tic tac toe etc. 
It is fundamental for creating intelligent game-playing agents that can make optimal decisions.

At its core, Minimax seeks to find the best possible move for a player by considering both its own and the opponent's potential moves. It operates as follows:

1. **Tree Generation**: The algorithm constructs a game tree that represents all possible moves and counter-moves for a given depth. Each level of the tree alternates between the player's and the opponent's turn.

2. **Evaluation**: A heuristic evaluation function is applied to the leaf nodes of the tree. This function quantifies the desirability of the game state for the player. In chess, for instance, this could involve factors like piece values, board control, and king safety.

3. **Backpropagation**: The evaluation scores are propagated up the tree. The player aims to maximize their score (hence "max" in Minimax), while the opponent aims to minimize it (hence "min").

4. **Decision Making**: As the algorithm traverses the tree, it chooses the move that leads to the highest-scored outcome for the player at the root node. This decision-making process assumes perfect play from both sides.

5. **Depth-Limited Search**: Minimax can be computationally expensive for deep trees, so it is often combined with depth-limited search or pruning techniques like Alpha-Beta pruning to improve performance.

<a name="heuristic"></a>
### Heuristic function
Heuristic function  is used for evaluating state of the game based on various factors like piece mobility and material advantage.

The heuristic function works on the  formula:

**Σ(fi * wi)**

Here, fi represents the score returned by various evaluation factors (such as piece mobility and material advantage), and wi represents their associated weights or biases.

* **Piece Mobility**: This aspect of the heuristic function assesses how actively and effectively the player's pieces can move on the board. Higher piece mobility can be an indicator of a stronger position, as it allows for more tactical options and control over the board.

* **Material Advantage**: Material advantage takes into account the relative values of the pieces on the board. For example, in chess, queens are worth more than knights, which are worth more than pawns. Evaluating material balance helps the algorithm understand who has the upper hand in terms of the quantity and quality of pieces.

The heuristic function computes a score for each game state by applying this formula. The weights (wi) associated with each evaluation factor allow you to fine-tune the importance of these factors in the overall evaluation. Adjusting these weights can significantly impact the AI's decision-making process, enabling you to prioritize certain aspects of the game state based on your strategic objectives. 

The higher the score the better the game sate or postion is considerd. (Currently the heuristic function is not well tuned).
<a name="alphaBeta"></a>
### Alpha Beta pruning and other optimization

#### Alpha beta pruning

* Alpha-Beta pruning is an optimization technique used in minimax algorithms to reduce the number of nodes evaluated in a search tree.
* It aims to minimize the number of nodes explored by pruning branches of the tree that are guaranteed to be worse than the current best move.
* Alpha and Beta are two values that represent the minimum score a maximizing player is assured of and the maximum score a minimizing player is assured of, respectively. 
* When traversing the tree, if a node's evaluation falls outside the range defined by Alpha and Beta for its parent node. For a maximizing player, if Alpha becomes greater or equal to Beta, the search can be pruned. For a minimizing player, if Beta becomes less than or equal to Alpha, pruning can occur.
* Alpha-Beta pruning significantly reduces the search space and is especially valuable in deep or complex game trees, making it a fundamental technique in game-playing AI, such as chess engines.

#### Other optimization

* **Reducing Data size:** instead of using Dictionary<Vector2Int, GameObject> that whole game uses because of its ease of use and expandibilty the AI uses a 8 X 8 matrix of byte that stores the piece at thier postions. This helps a lot with the memory issue and has significant impact on processing, as passing the whole Dictionary again and again and many times creating its copies was very unoptimized.

* **disposing variables:** disposing the variables after they go out of scope is done by garbage collector but since all the operation Currently happens in single frame, we have to manually clear the Dictionary, also call the garbage collector manually to free some memory. This adds to the processing time but keeps the moemory usage in check.



<a name="keyFetures"></a>
## Key features
* **Grid generation system** for spawning map tiles as gameobjects, with an editor script to help generate tileset in editor mode based on given parameters.
* **Chess game** with complete implementation of pieces movement/ legal moves, players turn handler,enemy piece capture etc. 
* **industry standard code** is used throughout the project to check the project the maintainablity and expansion of the project and to ease the workflow by using SOLID principles and observer pattern for one to many interaction and singleton for tools like world vector to grid vector conversation.

* **Minimax AI** the project successfully implements chess ai using minimax algorithm 

* **Heuristic function** uses currently uses 2 methods to calculate the score  of the chess game state namely piece mobility, material advantage.
<a name="installation"></a>
## Installation

#### Build/final game
1. You can download the latest build from https://bhavesh-solanki.itch.io/chess-minimax-ai
2. Run the exe file to start the game

#### Project
1. Download the github repo. 
2. Open unity hub add the downloaded project. 
3. Open the project using unity 2021.3.16f1.
4. You are all set to play around and explore the project.

<a name="flow"></a>
## Code flow/Key scripts

1. **InputReceiver** reads an input from the user and forwards it to **InputTaskHandler**. 

2. **InputTaskHandler** acts as an mediator between the input layer and other layers. If a piece is selected it forwards it to **MovePiece** otherwise if a piece is clicked on, it forwards it to **SelectPiece**. 

3. **SelectPiece** gets all the possible tiles player can move to from **Ipiece** component of the selected piece and triggers an event that informs **DisplayTiles** to display the movable tiles. And now we wait for player to select a different piece or select a tile where player piece can move to.

4. Once the player clicks on the tile player can move to **InputTaskHandler** calls **MovePiece**. Which Moves the Piece to desired location and triggers event that tells the game a piece is moved.  It is this event that tells the game the player chance has ended.

5. In human vs human game the **InputReceiver** switches its raycasting layer to different player pieces so it only takes input for that player.

6. In a human vs AI the **MovePiece** called event triggers a function in **AiTurnHandler** that collects all the requirement for the AI and forwards it to an Interface **IMiniMax** which is implemented by all minimax scripts (we have different minimax scripts for different types of minimax like one that implementes jobs, another is simple minimax).

7. **IMiniMax** returns a list of all root node of the minimax tree with score so **AiTurnHandler** can decide the which move to play based on difficulty. 

8. After getting the list possible moves with scores (currently AI chooses best move). It calls **MovePiece** to move the piece.

<a name="plans"></a>
## Future plans

* **Optimization:** At the current stage lot optimization can be used to improve the Ai and make it more performant like move ordering (exploring better moves first) can significantly improve alpha beta pruning, time based deepening it can help us to remain in time limit and without compromising a lot on the output.

* **Multithreading:** (in progress) Multithreading can help significantly improve the performance and speed by parallelizing the task and dividing the tree into seveeral other smaller tree and solving them simultaneously. 

* **Heuristic function:** Heuristic function is a significant part of the ai I plan to improve the heuristic function by adding more criteria to it like king safety, pawn structure etc. Also improving the weights/bias of the heuristic function can improve the Ai perform better.

* **Machine learning:** A rudimentary form of machine learning can be used specifically supervised learning for tuning the weights/biases of the function, this is called feature tuning.

* **Developer menu:** Developer menu will be used for setting depth, chnaging biases, turning heuristic function methods on/off etc.

* **Difficulty adjustment:** Adding Difficulty adjuster for player to choose between the difficulty for which the ai will out the move accordingly.

<a name="Acknowledgement"></a>
## Acknowledgement


 * [Algorithms Explained – minimax and alpha-beta pruning by Sebastian lague](https://www.youtube.com/watch?v=l-hh51ncgDI)

 * [Chess pieces art by John Pablok](https://opengameart.org/content/chess-pieces-and-board-squares)
 

