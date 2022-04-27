using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{

    //command stack
    //new actions go to top of redo stack
    //current state of program is the top of the redo stack
    //most recent undo is on top of the undo stack
    //moving forward in "timeline" would be pulling off the undo stack, push onto redo stack
    //moving backward in "timeline" would be pulling off the redo stack, push onto undo stack

    public Stack<CommandStruct> redoStack = new Stack<CommandStruct>();
    public Stack<CommandStruct> undoStack = new Stack<CommandStruct>();

    public Hello myGameManager;

    public struct CommandStruct {
        public string commandName;
        public GameObject copiedGO;
        public Vector3 origPosition;
        public Vector3 newPosition;
        public Quaternion origRotation;
        public Quaternion newRotation;
        public bool isRedoCommand;
    }

    public CommandStruct myCommandStruct;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*
    Print the struct to the console.
    */
    public void printStruct() {
        print("Name is: " + myCommandStruct.commandName);
        print("Position is: " + myCommandStruct.origPosition);
    }

    //invariant are 

    /*
    The struct is created for a redo command.
    */
    public void createRedoCommand(string myCommandName, GameObject currentGameObject, Quaternion oldRotation, Vector3 oldPosition) {
        CommandStruct tempStruct = new CommandStruct();
        tempStruct.commandName = myCommandName;
        tempStruct.copiedGO = currentGameObject;
        tempStruct.origPosition = oldPosition;
        tempStruct.newPosition = currentGameObject.transform.position;
        tempStruct.origRotation = oldRotation;
        tempStruct.newRotation = currentGameObject.transform.rotation;
        tempStruct.isRedoCommand = true;
        redoStack.Push(tempStruct);
    }

    /*
    Ensure that stack is not empty, then enact an undo.
    */
    public void undoRequested() {
        //check redo stack
        //pop command off redostack
        //do important stuff
        //push command onto undo stack
        if (redoStack.Count == 0) {
            print("redo stack is empty, cannot undo");
            myGameManager.showErrorMessage("No moves left to undo.");
            return;
        } else {
            CommandStruct tempStruct = redoStack.Pop();
            print("just popped the redo stack. Size is now: " + redoStack.Count);
            //GameObject newCopyGO = Instantiate(tempStruct.copiedGO);
            tempStruct.isRedoCommand = false;
            executeCommand(ref tempStruct);
            //newCopyGO.SetActive(false);
            //tempStruct.copiedGO = newCopyGO;
            undoStack.Push(tempStruct);
        }
    }

    /*
    Ensure that stack is not empty, then enact a redo.
    */
    public void redoRequested() {
        //check undo stack
        //pop command off undostack
        //do important stuff
        //push command onto redo stack
        if (undoStack.Count == 0) {
            print("undo stack is empty, cannot redo");
            myGameManager.showErrorMessage("No moves left to redo.");
            return;
        } else {
            CommandStruct tempStruct = undoStack.Pop();
            print("redoRequested(). Just popped undo. Size is now: " + undoStack.Count);
            
            tempStruct.isRedoCommand = true;
            executeCommand(ref tempStruct);
    
            redoStack.Push(tempStruct);
        }
    }

    /*
    Depending on myCommand, we execute an action to undo/redo.
    CommandStruct: reference to a command struct to execute.
    */
    public void executeCommand(ref CommandStruct myCommand) {
        string fakeEnum = myCommand.commandName;
        switch(fakeEnum) {
            case "DELETECMD":
                if (myCommand.isRedoCommand) {
                    print("executeCommand(). About to destroy.");
                    GameObject newCopyGO = Instantiate(myCommand.copiedGO);
                    newCopyGO.SetActive(false);
                    Destroy(myCommand.copiedGO);
                    myCommand.copiedGO = newCopyGO;
                } else {
                    myCommand.copiedGO.SetActive(true);
                }
                break;
            case "ROTATECMD":
                if (myCommand.isRedoCommand) {
                    print("executeCommand(). About to rotate.");
                    myCommand.copiedGO.transform.rotation = myCommand.newRotation;
                } else {
                    myCommand.copiedGO.transform.rotation = myCommand.origRotation;
                }
                break;
            case "MOVECMD":
                if (myCommand.isRedoCommand) {
                    print("executeCommand(). About to move.");
                    myCommand.copiedGO.transform.position = myCommand.newPosition;
                } else {
                    myCommand.copiedGO.transform.position = myCommand.origPosition;
                }
                break;
            case "CREATECMD":
                if (myCommand.isRedoCommand) {
                    print("executeCommand(). About to create.");
                    myCommand.copiedGO.SetActive(true);
                } else {
                    myCommand.copiedGO.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
