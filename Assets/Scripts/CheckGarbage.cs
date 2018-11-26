using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckGarbage : MonoBehaviour {


    int score = 10;
    public Text guiScore;
    
    

    private void Start()
    {
        guiScore.text = "Score: 10";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (transform.name == "metal_trash")
        {
            
            if (collision.gameObject.name == "soda_can(Clone)(Clone)")
            {
                getPoint(collision);
            }
            else
            {
                losesPoint(collision);
            }
        }
        if (transform.name == "paper_trash")
        {
            if (collision.gameObject.name == "news_paper(Clone)(Clone)")
            {
                getPoint(collision);
            }
            else
            {
                losesPoint(collision);
            }
        }

        if (transform.name == "glass_trash")
        {
            if (collision.gameObject.name == "glass_bottle(Clone)(Clone)")
            {
                getPoint(collision);
            }
            else
            {
                losesPoint(collision);
            }
        }

        if(transform.name == "plastic_trash")
        {
            if (collision.gameObject.name == "bottle(Clone)(Clone)")
            {
                getPoint(collision);
            }
            else
            {
                losesPoint(collision);
            }
        }


    }

    void losesPoint(Collider2D collision)
    {
        
        if (score <= 0)
        {
            gameOver();
        }
        score = score - 1;
        //print("-score: " + score);
        guiScore.text = "Score: " + score;
        Destroy(collision.gameObject);
    }

    void getPoint(Collider2D collision)
    {
        score = score + 1;
       // print("score: " + score);
        guiScore.text = "Score: " + score;
        Destroy(collision.gameObject);
    }

    void gameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

}
