using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PullAndRelease : MonoBehaviour {

    // The default Position
    Vector2 startPos;
    Vector2 simetricPoint;
    Vector2 xPoint;
    public float force;
    float g;
    float hmax;
    float dX;
    public int _numberOfElements = 10; // Number of elements should draw in path of parabola
    private int numOfTrajectoryPoints = 30;
    private List trajectoryPoints;

    public GameObject _trajectoryElement;
    System.Collections.Generic.List<GameObject> _trajectoryElementsContainer = new System.Collections.Generic.List<GameObject>();


    void Start()
    {
        startPos = transform.position;
        g = GetComponent<Rigidbody2D>().gravityScale;
        xPoint.y = 0;
        xPoint.x = 3;


        for (int i = 0; i < _numberOfElements; i++)
            _trajectoryElementsContainer.Add(Instantiate(_trajectoryElement) as GameObject);


        trajectoryPoints = new List();
        

    }


    void OnMouseUp()
    {
        // ToDo: fire the Bird
        // Disable isKinematic
        GetComponent<Rigidbody2D>().isKinematic = false;

        // Add the Force
        Vector2 dir = startPos - (Vector2)transform.position;
        GetComponent<Rigidbody2D>().AddForce(simetricPoint * force);

        // Remove the Script (not the gameObject)
        Destroy(this);

    }

    void OnMouseDrag()
    {
        // ToDo: move the Bird
        // Convert mouse position to world position
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Keep it in a certain radius
        float radius = 1.8f;
        Vector2 dir = p - startPos;
        if (dir.sqrMagnitude > radius)
            dir = dir.normalized * radius;

        // Set the Position
        transform.position = startPos + dir;

        simetricPoint = transform.position * -1;

        //float distance = Vector2.Distance(startPos, simetricPoint);
 
        float angle = Vector2.Angle(simetricPoint, xPoint);
        print("angle: " + angle);

        float initialVelocity = getInicialVelocity(transform.position, angle);
        //print("initialVelocity: " + initialVelocity);

        hmax = getHmax(initialVelocity, angle);
        //print("hmax: " + hmax);

        dX = getDistanceX(initialVelocity, angle);
        //print("dX: " + dX);

        Vector2 end = new Vector2(dX, -5);




    }

    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * force;
    }


    void setTrajectoryPoints(Vector3 pStartPosition, Vector3 pVelocity)
    {
        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
        float fTime = 0;

        fTime += 0.1f;
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
            Vector3 pos = new Vector3(pStartPosition.x + dx, pStartPosition.y + dy, 2);
            _trajectoryElementsContainer[i].transform.position = pos;
            _trajectoryElementsContainer[i].GetComponent<Renderer>().enabled = true;
            //_trajectoryElementsContainer[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
            fTime += 0.1f;
        }
    }


    float getInicialVelocity(Vector2 birdPos, float angle)
    {
        //print("g: " + g);
        float partOne = birdPos.x * birdPos.x * g;
        //print("x*x*g: " + partOne);

        float angle2 = angle * 2;
        //print("angle2: " + angle2);

        float sin = Mathf.Sin(angle2);
        //print("sin: " + sin);

        float cos = Mathf.Cos(angle) * Mathf.Cos(angle);
        //print("cos: " + cos);

        float partTwo = (birdPos.x * sin) - (2 * birdPos.y * cos);
        //print("partTwo: " + partTwo);

        float initialVelocity = Mathf.Sqrt(Mathf.Abs(partOne / partTwo));

        return initialVelocity;
    }
   
    float getHmax(float initialVelocity, float angle)
    {
        float v = initialVelocity * initialVelocity;
        //print("v: " + v);

        float sin = Mathf.Sin(angle) * Mathf.Sin(angle);
        //print("sin: " + v);


        float hMax = v * sin / 2 * g;

        return hMax;
    }

    float getDistanceX(float initialVelocity, float angle)
    {
        float v = initialVelocity * initialVelocity;
        //print("Dv: " + v);

        float sin = Mathf.Sin(angle);

        //print("Dsin: " + sin);

        float cos = Mathf.Cos(angle);
        //print("Dcos: " + cos);


        float xMax = (v * sin * cos)/ g;

        return xMax*2;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(simetricPoint, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(xPoint, 0.5f);

        Vector2 v = new Vector2(dX, hmax);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(v, 0.5f);

    }

    Vector2 getParabola(float angle, Vector2 currentPos )
    {
        Vector2 v;
        float sec = (1 / Mathf.Cos(angle)) * (1 / Mathf.Cos(angle));
        float a = -g * sec / 2 * 1.8f;
        float b = Mathf.Tan(angle);
        float y = a * (currentPos.x * currentPos.x) + b * currentPos.x;

        v.x = currentPos.x;
        v.y = y;

        return v;
    }

    Vector2 SampleParabola(Vector2 start, Vector2 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        Vector2 travelDirection = end - start;
        Vector2 result = start + t * travelDirection;
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            
            result.y += (-parabolicT * parabolicT + 1) * height;
            return result;
        }

        return result;

    }





}
