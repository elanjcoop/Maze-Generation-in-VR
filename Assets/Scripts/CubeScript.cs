using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CubeScript : MonoBehaviour
{

    public bool isSelected = false;
    public bool isMazeStart = false;
    public bool isMazeEnd = false;
    public bool isColliding = false;

    public GameObject cube;
    public string objectType;

    public Material regularMaterial;
    public Material selectedMaterial;
    public Material startMaterial;
    public Material endMaterial;

    public InputActionReference secondaryReference = null;
    public InputActionReference xReference = null;
    public InputActionReference yReference = null;
    public InputActionReference leftTriggerReference = null;
    public InputActionReference leftThumbStickReference = null;
    public InputActionReference rightThumbStickReference = null;

    Hello myGameManager;

    // Start is called before the first frame update
    /*
    Find the game object Hello to later call methods from the class.
    This is a prefab so we have to go the annoying route.
    */
    void Start()
    {
        myGameManager = GameObject.FindGameObjectWithTag("Hello").GetComponent<Hello>();
    }

    /*
    On awake, we assign buttons to methods.
    */
    private void Awake() {
        secondaryReference.action.started += deleteHelper;

        leftThumbStickReference.action.started += startMazeHelper;
        rightThumbStickReference.action.started += endMazeHelper;
    }

    // Update is called once per frame
    void Update()
    {
    }

    /*
    Set isColliding to either true or false depending on status.
    */
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Collider")) {
            isColliding = true;
        }
    }

    /*
    Set isColliding to either true or false depending on status.
    */
    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Collider")) {
            isColliding = false;
        }
    }

    /*
    Selecting an object to change its material or to remove its selection status.
    We also only change the mesh of non-starting materials.
    */
    public void select() {
        if (isSelected) {
            cube.GetComponent<MeshRenderer>().material = regularMaterial;
            isSelected = false;
            myGameManager.minusSelected();
            print("DESELECTED: " + myGameManager.getSelected());
        } else {
            if (myGameManager.getSelected() == 0) {
                cube.GetComponent<MeshRenderer>().material = selectedMaterial;
                isSelected = true;
                myGameManager.addSelected();
            }
            print("SELECTED: " + myGameManager.getSelected());
        }

        if (isMazeStart) {
            gameObject.GetComponent<MeshRenderer>().material = startMaterial;
        }
        if (isMazeEnd) {
            gameObject.GetComponent<MeshRenderer>().material = endMaterial;
        }
    }


    /*
    We use this method to destroy game objects and place a delete method
    on the command stack.
    */
    void deleteHelper(InputAction.CallbackContext context) {
        if (isSelected) {
            isSelected = false;
            if (isMazeStart) {
                isMazeStart = false;
            }
            if (isMazeEnd) {
                isMazeEnd = false;
            }

            myGameManager.minusSelected();
            cube.GetComponent<MeshRenderer>().material = regularMaterial;
            
            GameObject copyGO = Instantiate(gameObject);
            copyGO.SetActive(false);
            myGameManager.addCommand("DELETECMD", copyGO, Quaternion.identity, Vector3.zero);
            myGameManager.peekCommand();

            Destroy(cube, 0f);
        }
    }

    /*
    Ensure there is no start maze position, and if we find zero, we set
    this collider to be a start maze position. Remove the status if we find
    that we are already the maze start. Change mesh of the collider depending on start
    status.
    */
    void startMazeHelper(InputAction.CallbackContext context) {
        if (!isSelected) {
            return;
        }

        if (myGameManager.hasStart() && !isMazeStart) {
            myGameManager.showMessage("Please get rid of previous start to set the start here.");
        }

        if (isMazeEnd) {
            myGameManager.showMessage("Starting cube cannot also be end cube.");
            return;
        }

        if (isMazeStart) {
            myGameManager.showMessage("This cube is no longer the starting cube.");
            gameObject.GetComponent<MeshRenderer>().material = regularMaterial;
            isMazeStart = false;
            return;
        }

        if (isSelected && (myGameManager.getSelected() == 1) && !myGameManager.hasStart()) {
            print("Start maze here");
            myGameManager.showMessage("Maze start chosen!");
            isMazeStart = true;
            gameObject.GetComponent<MeshRenderer>().material = startMaterial;
        } else if (isSelected && (myGameManager.getSelected() != 1)) {
            myGameManager.showErrorMessage("Too many starting positions selected.");
        }
    }

    /*
    Ensure there is no end maze position, and if we find zero, we set
    this collider to be a end maze position. Remove the status if we find
    that we are already the maze end. Change mesh of the collider depending on end
    status.
    */
    void endMazeHelper(InputAction.CallbackContext context) {
        if (!isSelected) {
            return;
        }

        if (myGameManager.hasEnd() && !isMazeEnd) {
            myGameManager.showMessage("Please get rid of previous end to set the end here.");
        }

        if (isMazeStart) {
            myGameManager.showMessage("Ending cube cannot also be starting cube.");
            return;
        }

        if (isMazeEnd) {
            myGameManager.showMessage("This cube is no longer the ending cube.");
            isMazeEnd = false;
            gameObject.GetComponent<MeshRenderer>().material = regularMaterial;
            return;
        }

        if (isSelected && (myGameManager.getSelected() == 1) && !myGameManager.hasEnd()) {
            print("End maze here");
            myGameManager.showMessage("Maze end chosen!");
            gameObject.GetComponent<MeshRenderer>().material = endMaterial;
            isMazeEnd = true;
        }
        else if (isSelected && (myGameManager.getSelected() != 1)) {
            myGameManager.showErrorMessage("Too many ending positions selected.");
        }
    }


}
