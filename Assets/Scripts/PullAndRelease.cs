using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PullAndRelease : MonoBehaviour {

    // The default Position
    Vector2 startPos;
    public float force = 1300;
    // Use this for initialization
    void Start()
    {
        startPos = GameObject.Find("slingshot").transform.position;

        if (GetComponent<Rigidbody2D>().useFullKinematicContacts)
        {
            GetComponent<Rigidbody2D>().useFullKinematicContacts = false;
        }

    }

    void OnMouseUp()
    {
        // Disable isKinematic


        GetComponent<Rigidbody2D>().isKinematic = false;

        // Add the Force
        Vector2 dir = startPos - (Vector2)transform.position;
        GetComponent<Rigidbody2D>().AddForce(dir * force);

        // Remove the Script (not the gameObject)
        Destroy(this);
    }

    void OnMouseDrag()
    {
        // Convert mouse position to world position
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Keep it in a certain radius
        float radius = 1.8f;
        Vector2 dir = p - startPos;
        if (dir.sqrMagnitude > radius)
            dir = dir.normalized * radius;

        // Set the Position
        transform.position = startPos + dir;
    }



}




