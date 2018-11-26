using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    //bird prefab that will be spawned
    private GameObject soda_can;
    private GameObject bottle;
    private GameObject glass_bottle;
    private GameObject news_paper;
    private List<GameObject> prefabs;
    
    //public GameObject prefabs;

    //is there a bird in the trigger area?
    bool ocupied = false;
    Vector2 startPos;



    private void Start()
    {
        startPos = transform.position;





    }

    void FixedUpdate()
    {



        //if (!ocupied && !sceneMoving())
        if (!ocupied && sceneMoving())
            spawnNext();

        
    }

    void spawnNext()
    {


        

        bottle = GameObject.Find("bottle(Clone)");
        glass_bottle = GameObject.Find("glass_bottle(Clone)");
        soda_can = GameObject.Find("soda_can(Clone)");
        news_paper = GameObject.Find("news_paper(Clone)");

        if (bottle!=null && glass_bottle!=null && soda_can!=null && news_paper!=null)
        {
            int i = Random.Range(0, 3);
            switch (i)
            {
                case 0:
                    createGarbage(bottle);
                    break;
                case 1:
                    createGarbage(glass_bottle);
                    break;
                case 2:
                    createGarbage(soda_can);
                    break;
                case 3:
                    createGarbage(news_paper);
                    break;

            }

        }else if (bottle != null)
        {
            createGarbage(bottle);
        }
        else if (glass_bottle != null)
        {
            createGarbage(glass_bottle);
        }
        else if (soda_can != null)
        {
            createGarbage(soda_can);
        }else if (news_paper != null)
        {
            createGarbage(news_paper);
        }

    }

    void createGarbage(GameObject prefabs)
    {
        
        prefabs.GetComponent<Rigidbody2D>().isKinematic = true;
        Instantiate(prefabs, transform.position, Quaternion.identity);
        GameObject.Destroy(prefabs);
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
            if (rb.velocity.sqrMagnitude > 50)
                return true;
        return false;
    }



}
