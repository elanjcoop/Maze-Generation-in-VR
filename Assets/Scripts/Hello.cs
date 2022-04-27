using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//This is the GameManager class

public class Hello : MonoBehaviour
{
    public CommandManager myCommandManager;

    public GameObject CubePrefabOrig;
    public GameObject SpherePrefabOrig;
    public GameObject CapsulePrefabOrig;
    public GameObject CylinderPrefabOrig;

    public GameObject myXRControllerLeftGO;
    public GameObject myXRControllerRightGO;

    public GameObject myXROrigin;

    public GameObject LeftWristUI;
    public GameObject RightWristUI;

    public TextMeshProUGUI sorryText;
    
    public InputActionReference gripReference1 = null;
    public InputActionReference rightPrimaryReference = null;
    public InputActionReference leftMenuReference = null;

    public InputActionReference xReference = null;
    public InputActionReference yReference = null;

    private float offsetTime = 2f;
    private float timer = 0f;

    private bool menuOpen = false;

    private int selectedObjects = 0;

    private string currentObject = "cube";
    
    private bool startSelected;
    private bool endSelected;

    /*
    Assign proper buttons to methods on awake.
    */
    private void Awake() {
        gripReference1.action.started += gripHelper;
        rightPrimaryReference.action.started += primaryHelper;
        leftMenuReference.action.started += menuHelper;
        xReference.action.started += undoHelper;
        yReference.action.started += redoHelper;
    }

    // Start is called before the first frame update
    /*
    Set error text and left wrist to inactive.
    */
    void Start()
    {
        sorryText.gameObject.SetActive(false);
        LeftWristUI.gameObject.SetActive(false);
    }

    /*
    Isotropically change the scale of XROrigin.
    float: dynamic value that is assigned in the Unity inspector based on a slider
    */
    public void changeScale(float newScale) {
        print("NEW SCALE JUST DROPPED: " + newScale);
        Vector3 newScaleVector = new Vector3(newScale, newScale, newScale);
        myXROrigin.transform.localScale = newScaleVector;
    }

    /*
    Check if the grip has been pressed.
    */
    private void gripHelper(InputAction.CallbackContext context) {
        print("Hit the grip button!");
    }

    /*
    Check if the menu button has been pressed.
    */
    private void menuHelper(InputAction.CallbackContext context) {
        DisplayWristUI();
    }

    /*
    When menu is pressed, we toggle the Left Wrist UI.
    */
    public void DisplayWristUI() {
        if (menuOpen) {
            LeftWristUI.SetActive(false);
            menuOpen = false;
        } else {
            LeftWristUI.SetActive(true);
            menuOpen = true;
        }
    }

    /*
    Our main create method. We check to see the current object selected
    and then ensure it's in a legal, discretized position by rounding it
    and destroying it if it is not.
    Add create command to the stack.
    */
    private void primaryHelper(InputAction.CallbackContext context) {
        print("Hit the primary button!");
        GameObject tempGameObject;
        float newX = 0f;
        float newY = 0f;
        float newZ = 0f;
        switch(currentObject) {
            case "cube":
                tempGameObject = Instantiate(CubePrefabOrig);

                newX = myXRControllerRightGO.transform.position.x;
                newX = Mathf.Round(newX);
                newZ = myXRControllerRightGO.transform.position.z;
                newZ = Mathf.Round(newZ);

                newY = myXRControllerRightGO.transform.position.y;
                newY = Mathf.Floor(newY) + 0.5f;
                break;
            case "capsule":
                tempGameObject = Instantiate(CapsulePrefabOrig);
                
                newX = myXRControllerRightGO.transform.position.x;
                newX = Mathf.Round(newX);
                newZ = myXRControllerRightGO.transform.position.z;
                newZ = Mathf.Round(newZ);
                
                newY = 1f;
                break;
            case "cylinder":
                tempGameObject = Instantiate(CylinderPrefabOrig);
                
                newX = myXRControllerRightGO.transform.position.x;
                newX = Mathf.Round(newX);
                newZ = myXRControllerRightGO.transform.position.z;
                newZ = Mathf.Round(newZ);
                
                newY = myXRControllerRightGO.transform.position.y;
                newY = Mathf.Floor(newY) + 0.5f;
                break;
            case "sphere":
                tempGameObject = Instantiate(SpherePrefabOrig);
                
                newX = myXRControllerRightGO.transform.position.x;
                newX = Mathf.Floor(newX) + 0.5f;
                newZ = myXRControllerRightGO.transform.position.z;
                newZ = Mathf.Floor(newZ) + 0.5f;
                
                newY = 1f;
                break;
            default:
                tempGameObject = Instantiate(CubePrefabOrig);
                break;
        }
        

        tempGameObject.transform.position = new Vector3(newX, newY, newZ);
        print("X: " + newX);
        print("Y: " + newY);
        print("Z: " + newZ);

        if (checkForCollision(tempGameObject.transform.position)) {
            showErrorMessage();
            Destroy(tempGameObject, 0f);
            return;
        }

        if (createdOutOfBounds(tempGameObject, newX, newY, newZ)) {
            return;
        }

        //creation command
        addCommand("CREATECMD", tempGameObject, Quaternion.identity, Vector3.zero);
        peekCommand();
        

    }

    /*
    Check if position is out of bounds, and delete if it is.
    GameObject: prefab that specifies the legal parameters of a placement.
    floats: position of the instantiated object.
    */
    bool createdOutOfBounds(GameObject tempGameObject, float newX, float newY, float newZ) {
        switch(currentObject) {
            case "cube":
                if (newX > 5 || newX < -5) {
                    Destroy(tempGameObject, 0f);
                    showErrorMessage("Illegal x position.");
                    return true;
                }

                if (newY > 2.5 || newY < 0) {
                    Destroy(tempGameObject, 0f);
                    showErrorMessage("Illegal y position.");
                    return true;
                }

                if (newZ > 5 || newZ < -5) {
                    Destroy(tempGameObject, 0f);
                    showErrorMessage("Illegal z position.");
                    return true;
                }
                break;
            case "capsule":
                if (newX > 5 || newX < -5) {
                    Destroy(tempGameObject, 0f);
                    showErrorMessage("Illegal x position.");
                    return true;
                }

                if (newY > 1.5 || newY < 1) {
                    Destroy(tempGameObject, 0f);
                    showErrorMessage("Illegal y position.");
                    return true;
                }

                if (newZ > 5 || newZ < -5) {
                    Destroy(tempGameObject, 0f);
                    showErrorMessage("Illegal z position.");
                    return true;
                }
                break;
            case "cylinder":
                if (newX > 5 || newX < -5) {
                    Destroy(tempGameObject, 0f);
                    showErrorMessage("Illegal x position.");
                    return true;
                }

                if (newY > 2.5 || newY < 0) {
                    Destroy(tempGameObject, 0f);
                    showErrorMessage("Illegal y position.");
                    return true;
                }

                if (newZ > 5 || newZ < -5) {
                    Destroy(tempGameObject, 0f);
                    showErrorMessage("Illegal z position.");
                    return true;
                }
                break;
            case "sphere":
                if (newX > 5 || newX < -5) {
                    Destroy(tempGameObject, 0f);
                    showErrorMessage("Illegal x position.");
                    return true;
                }

                if (newY > 1.5 || newY < 1) {
                    Destroy(tempGameObject, 0f);
                    showErrorMessage("Illegal y position.");
                    return true;
                }

                if (newZ > 5 || newZ < -5) {
                    Destroy(tempGameObject, 0f);
                    showErrorMessage("Illegal z position.");
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }


    /*
    Check for collisions on placement of colliders.
    Vector3: position of the newly place collider.
    */
    bool checkForCollision(Vector3 newPos) {
        Collider[] colliders = Physics.OverlapBox(newPos, new Vector3(0f, 0f, 0f));
        for(int i = 0; i < colliders.Length; i++){
            if(colliders[i].tag == "Collider"){
            //if a collider is found that you want to avoid then the function will return
            //and thus, not reach the Instantiate()
            return true;
            }
        }
        return false;
    }


    // Update is called once per frame
    /*
    We have a timer set for the error message, and turn it off when the timer
    has reached the offset. We also disable the left controller rays depending
    on if an object is selected.
    */
    void Update()
    {
        if (sorryText.gameObject.activeSelf) {
            timer += Time.deltaTime;
            if(timer > offsetTime) {
                timer = 0f;
                sorryText.gameObject.SetActive(false);
            }

        }

        if (selectedObjects != 0) {
            RightWristUI.SetActive(true);
            myXRControllerLeftGO.GetComponent<XRRayInteractor>().enabled = true;
            myXRControllerLeftGO.GetComponent<LineRenderer>().enabled = true;
            myXRControllerLeftGO.GetComponent<XRInteractorLineVisual>().enabled = true;
        } else {
            RightWristUI.SetActive(false);
            myXRControllerLeftGO.GetComponent<XRRayInteractor>().enabled = false;
            myXRControllerLeftGO.GetComponent<LineRenderer>().enabled = false;
            myXRControllerLeftGO.GetComponent<XRInteractorLineVisual>().enabled = false;
        }
    }

    /*
    Display a message to error text.
    string: message to be displayed, DEFAULT null
    */
    public void showErrorMessage(string errorMessage = null) {
        if (errorMessage != null) {
            sorryText.text = "Error\n" + errorMessage;
        } else {
            sorryText.text = "ERROR!\nCannot place that object there!";
        }
        sorryText.gameObject.SetActive(true);
        timer = 0;
    }

    /*
    Display a message to error text.
    string: message to be displayed
    */
    public void showMessage(string message) {
        sorryText.text = message;
        sorryText.gameObject.SetActive(true);
        timer = 0;
    }

    /*
    Set the next object to be placed upon a creation.
    string: type of object to be placed next
    */
    public void setCurrentObject(string objectType) {
        currentObject = objectType;
        if (objectType == "cube") {
            print("CUBE TIME");
        } else if (objectType == "capsule") {
            print("CAPSULE TIME");
        } else if (objectType == "cylinder") {
            print("cylinder TIME");
        } else if (objectType == "sphere") {
            print("SPHERE TIME");
        }
    }

    public void addSelected() {
        selectedObjects += 1;
    }

    public void minusSelected() {
        selectedObjects -= 1;
    }

    public int getSelected() {
        return selectedObjects;
    }

    /*
    Check if there exists a maze start collider.
    */
    public bool hasStart() {
        var objects = GameObject.FindGameObjectsWithTag("Collider");
        foreach (var obj in objects) {
            CubeScript c = obj.GetComponent<CubeScript>();
            if (c.isMazeStart) {
                return true;
            }
        }
        return false;
    }

    /*
    Check if there exists a maze end collider.
    */
    public bool hasEnd() {
        var objects = GameObject.FindGameObjectsWithTag("Collider");
        foreach (var obj in objects) {
            CubeScript c = obj.GetComponent<CubeScript>();
            if (c.isMazeEnd) {
                return true;
            }
        }
        return false;
    }

    //X button
    void undoHelper(InputAction.CallbackContext context) {
        print("undo 2");
        myCommandManager.undoRequested();
    }

    //Y button
    void redoHelper(InputAction.CallbackContext context) {
        print("redo 2");
        myCommandManager.redoRequested();
    }

    //fresh user action -> add a command
    //x undo -> createUndoCommand()
    //y redo button -> createRedoCommand()
    public void addCommand(string commandName, GameObject originalGameObject, Quaternion originalRotation, Vector3 originalPosition) {
        myCommandManager.createRedoCommand(commandName, originalGameObject, originalRotation, originalPosition);
    }

    /*
    Preview the command to console
    */
    public void peekCommand() {
        print("Peek command: " + myCommandManager.redoStack.Peek().commandName);
    }
    

}
