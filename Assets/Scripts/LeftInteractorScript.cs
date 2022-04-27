using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftInteractorScript : MonoBehaviour
{
    private float offsetTime = 0.8f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*
    We check for each collider to see if the left hand is interacting with it.
    If it is, we set an offset timer and wait to select a collider until that timer has reset
    so the user isn't constantly selecting and deselecting.
    Heavy code that could use improvement.
    */
    void Update()
    {
        var objects = GameObject.FindGameObjectsWithTag("Collider");
        foreach (var obj in objects) {
            CubeScript c = obj.GetComponent<CubeScript>();
            if (c.objectType == "sphere") {
                if (Mathf.Abs(obj.transform.position.x - transform.position.x) < 0.8f && Mathf.Abs(obj.transform.position.y - transform.position.y) < 0.8f && Mathf.Abs(obj.transform.position.z - transform.position.z) < 0.8f) {
                    if(timer > offsetTime) {
                        print("RESET TIMER");
                        timer = 0f;
                        c.select();
                    } else if (timer == 0f) {
                        c.select();
                    }
                }
            } else {
                if (Mathf.Abs(obj.transform.position.x - transform.position.x) < 0.4f && Mathf.Abs(obj.transform.position.y - transform.position.y) < 0.4f && Mathf.Abs(obj.transform.position.z - transform.position.z) < 0.4f) {
                    if(timer > offsetTime) {
                        print("RESET TIMER");
                        timer = 0f;
                        c.select();
                    } else if (timer == 0f) {
                        c.select();
                    }
                }
            }
        }
        timer += Time.deltaTime;
    }
}
