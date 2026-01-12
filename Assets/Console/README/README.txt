CONSOLE JAVIER PALOS

HOW TO REGISTER COMMANDS:

- Editor

1.- Create a new command in the assets by right-clicking: Create/Commands/Command.

2.- In that Scriptable Object, enter the command information:
	- Command name to execute.
	- Description.
	- Add to the parameters list the description of each one, and whether the parameters are required.
	- Add to the shortcuts list which ones activate the command, if you want to assign a shortcut.

3.- Add the new command to the command list inside CommandsSO.

- Code

4. In the GameCommands script, create a new function that calls the desired external function.

5. In that same script, inside the RegisterCommand function switch, add a case with the command name to execute, and inside it call the Register function from CommandRegistry. This function should be called as follows:
	- CommandRegistry.Register(command, (Action<paramType1, paramType2, paramType3>)FunctionName);
	- If there are no parameters: CommandRegistry.Register(command, (Action)FunctionName);


CONTROLS

1.- [LeftShift] opens and closes the console.

2.- [UpArrow] and [DownArrow] navigate through the command history.

3.- [Tab] autocompletes with the first command that matches what is written in the input field. It can be executed if there are at least 2 characters in the input field.