# battle-ships with Domain-Driven-Design

An implementation of the popular board game 'Battle Ships' using Domain-Driven-Design as the architecture with event sourcing.

The client application looks like this on macOS terminal:

<img src="https://github.com/ibo549/battle-ships/blob/main/game_looks_like_this_in_macos_terminal.png" width="500">

## How to build 
- Install [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0)
- Clone the repository
- In project root, execute `dotnet build`

## Run the client application
- `cd BattleShip.Client`
- `dotnet run`
- p.s. use a terminal that has full unicode support to display emojis like 🌊🌊🌊🌊🌊 ❌ ✅ 💥 🛳 💀 🎉 

macOS terminal (tested on zsh shell) and Windows Terminal (PowerShell + Command Prompt) supports emojis.

Old school cmd on Windows doesn't support emojis, you can use the modern [Windows Terminal](https://github.com/microsoft/terminal/releases/tag/v1.4.3243.0)

If you use cmd, you will still see hit, miss, won and the board, but the board display will be miserable. 

### BattleShip.Domain 
Uses _In-Memory-Event-Store_ to save the state of the board (that is, the events happening in the board such as hit, miss, sunk, win etc.) and publishes them to interested parties. 
Board state is restored from the event store and it's the single source of truth. 

`BattleShipBoard` is the **Aggregate Root** 

Uses a 2D Array, (not a jagged array) internally as _the board_. _Ship Id's_ are stored on the matrix, and we have O(1) access for lookup to understand whether it's occuppied with a ship or empty. 

**Random Ship Placement** is _non-deterministic_ and it's thread safe. 
When searching for available space to place a given ship of size **S**, starts the search from a random point, then chooses a random direction to look for.
The first scan direction is determined at run-time as well, (not pre-defined in the algorithm) so that after hitting a ship, you wouldn't be able to guess whether it's positioned horizontally, vertically, leaning left or right.
Keeps the visited starting points for **back tracking** to cut out. If there is at least available space of **S** in the board, first it 'reserves' the spot for the ship and then eventually places the ship to that spot. 
It can populate any given size of board full of ships, _randomly_.

Produces domain events that you can typically expect in a BattleShip game such as `ShipAdded`, `ShipSunk`, `ShotMissed`, when `AllShipsSunk`, it means, the clients should ring the Game Over bell. 

Utilizes a lightweight framework called [CQRSlite](https://github.com/gautema/CQRSlite) underneath to achieve event sourcing, restoring aggregate state, syncing changes, routing the events to consumers etc.

### BattleShip.Projections 
Consumes the events occuring in the domain and saves them to _In-Memory-Projection-Store_ in the way that it likes; only the data required to be presented to the user.

Tells the client application (in this case a console app) to display the results after every shot. Uses _projection storage_ to re-draw the board.
Any client implementing `IGameDisplay` can display the game in the way they implement.

For example a mobile client (say built with Xamarin) can display the game in a graphical display instead of the emojis 🙃 in the terminal.
Their source code will be almost similar with the console client except for the displaying part.

### BattleShip.Client - emojis in terminal 🥳
A console client implementation that let's you play a one-sided game against computer.

Upon `run`ing the client, it _commands_ to create a (10x10) battleship board. 
Board in _Domain_, actually supports a size of (`MaxInt x MaxInt`). 
You can change the command parameter (`CreateBoard`) to create for example a 20x20 board, if you wish [*1]. 

Later it shoots 3 commands to add a BattleShip and 2 Destroyers on random available area on board.

From there on, it asks user to enter letter coordinates in the form of A1 - where A is the column and 1 is the row number. 
A `Command` gets fired after a basic validation (that is, whether the letter part is actually a valid letter, or number part is actually a number. Doesn't do an actual game state validation - that happens in the domain) 

Result(s) are immediately shown to user after being received from the domain. The board is re-drawn to the console after every shot.
A single shot can cause 3 events in the system for example - ShipHit, ShipSunk, AllShipsSunk. As a player we are aiming for our last shot to cause these 3 events so that we win!! 🎉🎉

[*1]It won't let you create a dimension more than the letters in the English alphabet - as one coordinate of the board is a letter from the alphabet for this client.
Other clients can implement a different coordinate input/display mechanism, as long as they translate it to correct `BoardPoint` in _Domain_

BTW it looks like this on a Windows Terminal

<img src="https://github.com/ibo549/battle-ships/blob/main/winterminal.PNG" width="500">


## Further Work 
All PR's are welcomed for improvements or for adding missing commands (such as Adding Ship to a pre-defined location).

A mobile application with nice graphics can be designed using the BattleShip.Domain and BattleShip.Projections libraries.
