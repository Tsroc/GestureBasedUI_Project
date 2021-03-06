# GestureBasedUI_Project

## Luch Mór
The goal of Luch Mór is to collect cheese whilst avoiding the local cat, Finnegan. Finnegan will awaken over time. The player can seek assistance from Draoi, a wizard.
Draoi can cause Finnegan to return to his sleep, or he can evolve Luch Mór, allowing the player to scare scare Finnegan away. 



### Keyboard commands
Keyboard commands are limited to movement, the player can move up(W, ↑), down(S, ↓), left(A, ←) and right(D, →).

### Voice commands

#### Main Menu

###### Play game
- play/start/begin game

	
###### Quit game
- quit/end/exit game


#### Game commands

###### Movement
Will direct the player to move in that direction until the edge of the map.
 - {action} {direction}
 - {action} : "go", "move"
 - {directions} : "up", "down", "left", "right"
 - e.g. "go up", "move down"
 
 ###### Paths
 Will direct the player to path to the destination.
 - {action} {destination}
 - {action} : "go to", "move to", "path to"
 - {destination} : "home", "wizard", "cheese"
 - e.g. "go to cheese", "move to home"
 
###### Actions
Attacking the cat should be done after the player has evolved.  
Stop will cancel previous given commands.
 - {cat} : "attack the cat", "vanquish the cat"
 - {stop} : "stop"
 
###### Wizard interactions
Seeks assistance from the local wizard. The wizard can put the cat back to sleep and cause the mouse to evolve.
 - {action} {spell}
 - {action} : "cast"
 - {spell} : "sleep", "evolve"
 - e.g. "cast sleep", "cast evolve"
 
###### Cheese interactions
Cheese interactions, the play can take the cheese and deposit it at his home.
 - {action} {object}
 - {action, take} : "take", "loot", "steal"
 - {action, drop} : "drop", "deposit"
 
###### Gameover menu
 - "exit"
 - "menu"
