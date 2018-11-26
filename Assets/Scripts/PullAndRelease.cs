using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PullAndRelease : MonoBehaviour {


    Vector2 startPos;//posicao 0,0 no transform
    public float force;// forca a ser aplicada
    Vector2 simetricPoint;// ponto simetrico da posicao do lixo
    Vector2 xPoint;//ponto de apoio para achar o angulo
    float g;// gravidade
    float hmax;// altura maxima da parabola
    float dX;// distancia da parabola
	
	//codigo da intern -> https://gamedev.stackexchange.com/questions/116195/displaying-trajectory-path
    public int _numberOfElements = 10; // Number of elements should draw in path of parabola
    private int numOfTrajectoryPoints = 30;
    public GameObject _trajectoryElement;
    private List trajectoryPoints;
    Boo.Lang.List<GameObject> _trajectoryElementsContainer = new Boo.Lang.List<GameObject>();

    void Start()
    {
		//startPos (0,0)
        startPos = GameObject.Find("slingshot").transform.position;
		
		//pegando a gravidade atribuida ao objeto
        g = GetComponent<Rigidbody2D>().gravityScale;
		
		//posicao do ponto de apoio
        xPoint.y = 0;
        xPoint.x = 3;
        trajectoryPoints = new List();

		//acredito ser o numero de bolinhas que aparecem no desenho da parabola (codigo da internet)
        for (int i = 0; i < _numberOfElements; i++)
            _trajectoryElementsContainer.Add(Instantiate(_trajectoryElement) as GameObject);

		//paleativo para evitar um bug que acontece as vezes
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

		
		//Posicao do ponto simetrico - eh sempre contraria a posicao do lixo
        simetricPoint = transform.position * -1;

		
		//agulo calculado entre os vetores
        float angle = Vector2.Angle(simetricPoint, xPoint);
        //print("angle: " + angle);

        
        float initialVelocity = getInicialVelocity(vi, angle);
        print("initialVelocity: " + initialVelocity);
       
	   
        hmax = getHmax(initialVelocity, angle);
        //print("hmax: " + hmax);
		
		
        dX = getDistanceX(initialVelocity, angle);
        //print("dX: " + dX);
		
		//posicao fim da parabola (eu nao lembro pq coloquei y = -5)
        Vector2 end = new Vector2(dX, -5);

		//codigo da internt
        for (float i = 1; i <= _numberOfElements; i++)
        {
            //float distributionTime = 0;
            Vector3 currentPosition = SampleParabola(startPos, end, hmax, i / (float)_numberOfElements);
            _trajectoryElementsContainer[(int)i - 1].transform.position = new Vector3(currentPosition.x, currentPosition.y, 0);

            Vector3 nextPosition = SampleParabola(startPos, end, hmax, (i + 1) / (float)_numberOfElements);
            float angleInR = Mathf.Atan2((nextPosition.y - currentPosition.y), (nextPosition.x - currentPosition.x));
            _trajectoryElementsContainer[(int)i - 1].transform.eulerAngles = new Vector3(0, 0, (Mathf.Rad2Deg * angleInR) - 90);
        }


    }


	//separei os calculos em partes -> https://wikimedia.org/api/rest_v1/media/math/render/svg/2eac9695d3dae15f7cc5c7301354826c8bd2f6c4
    float getInicialVelocity(Vector2 birdPos, float angle)
    {
        
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

	//calculo altura maxima -> https://wikimedia.org/api/rest_v1/media/math/render/svg/12be1b7cde89a51c88ef0307f7070cb2368a2079
    float getHmax(float initialVelocity, float angle)
    {
        float v = initialVelocity * initialVelocity;
        //print("v: " + v);
		
        float sin = Mathf.Sin(angle) * Mathf.Sin(angle);
        //print("sin: " + v);
		
        float hMax = v * sin / 2 * g;
        return hMax;
    }

	//calculo distancia ->https://wikimedia.org/api/rest_v1/media/math/render/svg/a448d71f7d3bcf42e1fd3efd04f87f5fe5396a6d
    float getDistanceX(float initialVelocity, float angle)
    {
        float v = initialVelocity * initialVelocity;
        //print("Dv: " + v);
		
        float sin = Mathf.Sin(angle*2);
        //print("Dsin: " + sin);
	
        float xMax = (v * sin) / g;
        return xMax;
    }

	//codigo da internet
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



}




