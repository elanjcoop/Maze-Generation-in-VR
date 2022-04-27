using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SaveScript : MonoBehaviour
{

    string fname;
    string path;

    // Start is called before the first frame update
    /*
    Set the path of the file to application data.
    */
    void Start()
    {
        fname = System.DateTime.Now.ToString("HH-mm-ss") + ".txt";
        path = Path.Combine(Application.persistentDataPath, fname);
        print(fname);
        print(path);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    Ensure we have a maze start and a maze end, then build a string.
    We check for each collider's prefab type, position, and rotation, then
    reconstruct later in AR application.
    */
    public void saveGame() {
        var helloObject = GameObject.FindGameObjectWithTag("Hello");
        Hello myGM = helloObject.GetComponent<Hello>();
        if (!myGM.hasStart()) {
            myGM.showErrorMessage("Cannot SAVE until start position is chosen!");
        }
        if (!myGM.hasEnd()) {
            myGM.showErrorMessage("Cannot SAVE until end position is chosen!");
        }

        if (!myGM.hasStart() || !myGM.hasEnd()) {
            return;
        }

        
        string stringBuilder = "";

        var objects = GameObject.FindGameObjectsWithTag("Collider");
        foreach (var obj in objects) {
            CubeScript c = obj.GetComponent<CubeScript>();
            print("POSITION: " + obj.transform.position);
            stringBuilder += obj.transform.position + ",";
            print("ROTATION: " + obj.transform.eulerAngles);
            stringBuilder += obj.transform.eulerAngles + ",";
            print("TYPE: " + c.objectType);
            stringBuilder += c.objectType + ",";
            print("START POSITION? " + c.isMazeStart);
            stringBuilder += c.isMazeStart + ",";
            print("END POSITION? " + c.isMazeEnd);
            stringBuilder += c.isMazeEnd + "\n";
        }

        if (!File.Exists(path))
        {
            myGM.showMessage("SUCCESS!!!");
            // Create a file to write to.
            File.WriteAllText(path, stringBuilder);
            // Open the file to read from.
            string readText = File.ReadAllText(path);
            print(readText);
        } else {
            myGM.showErrorMessage("Could not save.");
        }
    }
}