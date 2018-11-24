using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    //bird prefab that will be spawned
    private GameObject birdPrefab;

    

    //is there a bird in the trigger area?
    bool ocupied = false;

     void FixedUpdate()
    {
        if (!ocupied && !sceneMoving())
            spawnNext();

    }

    void spawnNext()
    {
        birdPrefab = GameObject.Find("soda_can(Clone)");
        
        //Instantiate(birdPrefab, transform.position,Quaternion.identity);
        ocupied = true;
    }

     void OnTriggerExit2D(Collider2D collision)
    {
        ocupied = false;
    }

    bool sceneMoving()
    {
        // Find all Rigidbodies, see if any is still moving a lot
        Rigidbody2D[] bodies = FindObjectsOfType(typeof(Rigidbody2D)) as Rigidbody2D[];
        foreach (Rigidbody2D rb in bodies)
            if (rb.velocity.sqrMagnitude > 5)
                return true;
        return false;
    }



}
