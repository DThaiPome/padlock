![splash](/PagesAssets/splash.png)

## Overview of Padlock

***Padlock*** is a game concept I thought of when I was in high school. At the time, I was very inexperienced with good software design. However, I was somewhat experienced with the Unity game engine, and I figured this project would be easy enough to complete over a weekend. I did finish the project in a weekend, but, not knowing good software design, the code ended up being incredibly messy and clearly rushed. 

What I was most proud of with this project, at the time, was implementing a fill algorithm I found. Again, it was messy, but at the time I was very impressed with myself, and had a lot of fun learning how it worked. Overall, while this project does not really demonstrate my skills, it is still a neat product of my enthusiasm for games and programming. There is an executable in the repository to run the game and play it.

*Tokens are controlled using WASD or the arrow keys*

## Concept

The mechanics of the game are partly inspired by the [Pogo Pandemonium minigames from Crash Bash](https://crashbandicoot.fandom.com/wiki/Pogo_Pandemonium), an old party game that was released for the Playstation 1. I liked the concept of players "painting" tiles on a board as their own, and surrounding areas in order to claim that territory.

The game itself consists of two players facing off against each other with tokens on a board, is played in rounds. To win each round, each player must push the other player's token off of the board. Once one player wins at least five rounds and wins two more rounds than the other player, they win the game. The end-state is unfinished, so the game simply resets itself at this point.

![action image](/PagesAssets/white_token_pushed.png)

Moving a token will cause the tiles it moves across to change color. Every time a player surrounds an area completely with tiles of their color, with no gaps and no corners left unfilled, the encircled area will change to the player's color all at once. If the *other* player is within this encircled area when this happens, they will be forced out of the region, to whichever spot is closest. If the closest spot is off of the board, then the other player will be pushed outside the board, and they will lose the round.

The board itself is square, and each half of the board has barriers that are colored. The colored barriers will act as tiles of their respective color, making it easier for a player to claim territory on their respective side of the board.

