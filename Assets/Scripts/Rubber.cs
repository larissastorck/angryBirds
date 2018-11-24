using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubber : MonoBehaviour {
    // The Rubber objects
    public Transform leftRubber;
    public Transform rightRubber;
    float g = 4f;


    void adjustRubber(Transform bird, Transform rubber, string r)
    {
        // Rotation
        Vector2 dir = rubber.position - bird.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //print("RubberAngle"+ r + " :" + angle);

        rubber.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Length
        float dist = Vector3.Distance(bird.position, rubber.position);
        dist += bird.GetComponent<Collider2D>().bounds.extents.x;
        rubber.localScale = new Vector2(dist, 1);

        float cosAlpha = Mathf.Cos(angle);
        //initial velocity
        float v0 = Mathf.Sqrt(bird.position.x* bird.position.x* g/ 
            bird.position.x * Mathf.Sin(2*angle) - 2 * bird.position.y * cosAlpha * cosAlpha);
        //print("V0:" + v0);

    }

    public float getAngle(Transform bird, Transform rubber)
    {
        float angle;
        Vector2 dir = rubber.position - bird.position;

        return angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
    }


    public void OnTriggerStay2D(Collider2D coll)
    {
        // Stretch the Rubber between bird and slingshot
        adjustRubber(coll.transform, leftRubber,"L" );
        adjustRubber(coll.transform, rightRubber, "R");

        getAngle(coll.transform, leftRubber);

    }

    

    void OnTriggerExit2D(Collider2D coll)
    {
        // Make the Rubber shorter
        leftRubber.localScale = new Vector2(0, 1);
        rightRubber.localScale = new Vector2(0, 1);

        

    }

}
