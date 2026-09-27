The prototype that I planned on making was a system like Minecraft's placing and destroying block system. This way the player could build simple scructures or could reach new 
heights by placing blocks underneath themselves.

The main place I used an interface was to help the system interact and change a blocks color when the player interacts with it. This was I could potentaially call for the player to interacrt
with something else without falling into the inheritance trap. I also used the start and update inizialization codes to ensure I had the important compenents set up properly before the player
began using them such as the character controller or the movement system. In addition to this I used Time.Delta tine to ensure the movement and speed at which the player looks around remains
the same reguardless of the it would be played on. 

I had to reduce the scope from also being able to place different kinds of blocks and deleting them since I spent so much time getting the blocks to place properly. However I was able to add
additional interaction with the blocks being able to change color, though it is more random rather than select colors. If I was to take this further I would work on having a few different 
blocks the player could swap between and destroy, but I had to cut back a little to get this complete on time. (Github was frustrating).
