using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristMenuManager : MonoBehaviour
{

    public Hello myGameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    Rotate a collider
    string: called by Unity helper to show which axis to rotate upon.
    */
    public void rotate(string axis) {
        var objects = GameObject.FindGameObjectsWithTag("Collider");
        foreach (var obj in objects) {
            CubeScript c = obj.GetComponent<CubeScript>();
            if (!c.isSelected) {
                continue;
            } else {
                Vector3 rotationVector = obj.transform.eulerAngles;
                int rotateDegrees = 90;
                switch(axis) {
                    case "x":
                        rotationVector.x += rotateDegrees;
                        Mathf.Round(rotationVector.x);
                        break;
                    case "y":
                        rotationVector.y += rotateDegrees;
                        Mathf.Round(rotationVector.y);
                        break;
                    case "z":
                        rotationVector.z += rotateDegrees;
                        Mathf.Round(rotationVector.z);
                        break;
                    default:
                        break;
                }

                //GameObject copyGO = Instantiate(gameObject);
                //copyGO.SetActive(false);
                Quaternion originalRotation = obj.transform.rotation;
                obj.transform.rotation = Quaternion.Euler(rotationVector);

                myGameManager.addCommand("ROTATECMD", obj, originalRotation, Vector3.zero);
                myGameManager.peekCommand();
                



                print("ROTATE " + axis + "BY " + rotateDegrees);
            }
        }
    }

    /*
    Move a collider. We check if this causes a collision.
    string: called by Unity helper to show which axis to move upon,
        and whether to move forward or backward.
    BUG: if two colliders are bordering, the collision detection states
        they are colliding, even if a push would not cause a collision.
    */
    public void push(string axisPlusNumber) {
        var objects = GameObject.FindGameObjectsWithTag("Collider");
        foreach (var obj in objects) {
            CubeScript c = obj.GetComponent<CubeScript>();

            var helloObject = GameObject.FindGameObjectWithTag("Hello");
            Hello myGM = helloObject.GetComponent<Hello>();

            if (!c.isSelected) {
                continue;
            } else {
                Vector3 position = c.transform.position;
                print("BEFORE: " + c.transform.position);
                string axis = axisPlusNumber.Substring(0, 1);
                string number = axisPlusNumber.Substring(1, 1);
                print(axis + number);
                
                switch(axis) {
                    case "x":
                        if (number == "1") {
                            position.x += 1;
                            print("PUSHING X");
                        } else {
                            position.x -= 1;
                        }
                        break;
                    case "y":
                        if (number == "1") {
                            position.y += 1;
                        } else {
                            position.y -= 1;
                        }
                        break;
                    case "z":
                        if (number == "1") {
                            position.z += 1;
                        } else {
                            position.z -= 1;
                        }
                        break;
                    default:
                        break;
                }


                switch(c.objectType) {
                    case "cube":
                        if (position.x > 5 || position.x < -5) {
                            myGM.showErrorMessage("Illegal x position.");
                        } else if (position.y > 2.5 || position.y < 0) {
                            myGM.showErrorMessage("Illegal y position.");
                        } else if (position.z > 5 || position.z < -5) {
                            myGM.showErrorMessage("Illegal z position.");
                        } else {
                            Vector3 originalPosition = c.transform.position;
                            c.transform.position = position;

                            if (c.isColliding) {
                                c.transform.position = originalPosition;
                                myGM.showErrorMessage("Moving might cause a collision.");
                            } else {    
                                myGameManager.addCommand("MOVECMD", obj, Quaternion.identity, originalPosition);
                                myGameManager.peekCommand();
                                print("AFTER: " + c.transform.position);
                            }
                        }
                        break;
                    case "capsule":
                        if (position.x > 5 || position.x < -5) {
                            myGM.showErrorMessage("Illegal x position.");
                        } else if (position.y > 1.5 || position.y < 1) {
                            myGM.showErrorMessage("Illegal y position.");
                        } else if (position.z > 5 || position.z < -5) {
                            myGM.showErrorMessage("Illegal z position.");
                        } else {
                            Vector3 originalPosition = c.transform.position;
                            c.transform.position = position;

                            if (c.isColliding) {
                                c.transform.position = originalPosition;
                                myGM.showErrorMessage("Moving might cause a collision.");
                            } else {    
                                myGameManager.addCommand("MOVECMD", obj, Quaternion.identity, originalPosition);
                                myGameManager.peekCommand();
                                print("AFTER: " + c.transform.position);
                            }
                        }
                        break;
                    case "cylinder":
                        if (position.x > 5 || position.x < -5) {
                            myGM.showErrorMessage("Illegal x position.");
                        } else if (position.y > 2.5 || position.y < 0) {
                            myGM.showErrorMessage("Illegal y position.");
                        } else if (position.z > 5 || position.z < -5) {
                            myGM.showErrorMessage("Illegal z position.");
                        } else {
                            Vector3 originalPosition = c.transform.position;
                            c.transform.position = position;

                            if (c.isColliding) {
                                c.transform.position = originalPosition;
                                myGM.showErrorMessage("Moving might cause a collision.");
                            } else {    
                                myGameManager.addCommand("MOVECMD", obj, Quaternion.identity, originalPosition);
                                myGameManager.peekCommand();
                                print("AFTER: " + c.transform.position);
                            }
                        }
                        break;
                    case "sphere":
                        if (position.x > 5 || position.x < -5) {
                            myGM.showErrorMessage("Illegal x position.");
                        } else if (position.y > 1.5 || position.y < 1) {
                            myGM.showErrorMessage("Illegal y position.");
                        } else if (position.z > 5 || position.z < -5) {
                            myGM.showErrorMessage("Illegal z position.");
                        } else {
                            Vector3 originalPosition = c.transform.position;
                            c.transform.position = position;

                            if (c.isColliding) {
                                c.transform.position = originalPosition;
                                myGM.showErrorMessage("Moving might cause a collision.");
                            } else {    
                                myGameManager.addCommand("MOVECMD", obj, Quaternion.identity, originalPosition);
                                myGameManager.peekCommand();
                                print("AFTER: " + c.transform.position);
                            }
                        }
                        break;
                    default:
                        break;
                }


            }
        }
    }


}
