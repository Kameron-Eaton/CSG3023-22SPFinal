/**** 
 * Created by: Bob Baloney
 * Date Created: April 20, 2022
 * 
 * Last Edited by: Kameron Eaton
 * Last Edited: April 28, 2022
 * 
 * Description: Controls the ball and sets up the intial game behaviors. 
****/

/*** Using Namespaces ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Ball : MonoBehaviour
{
    [Header("General Settings")]
    int score; //int storing score
    public Text ballTxt; //text showing number of lives
    public Text scoreTxt; //text showing score
    public Paddle paddle; //reference to paddle
   

    [Header("Ball Settings")]
    public int numberOfBalls; //number of lives
    public int initialForce; //force when ball is shot from paddle
    public int speed; //constant speed ball travels at
    bool isInPlay; //flag for whether ball is in play or not

    Rigidbody rb; //reference to rigidbody
    AudioSource audioSource; //reference to audio source





    //Awake is called when the game loads (before Start).  Awake only once during the lifetime of the script instance.
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>(); //initialze rigidbody
        audioSource = gameObject.GetComponent<AudioSource>(); //initialize audio source
    }//end Awake()


        // Start is called before the first frame update
        void Start()
    {
        SetStartingPos(); //set the starting position

    }//end Start()


    // Update is called once per frame
    void Update()
    {
        ballTxt.text = ("Balls: " + numberOfBalls.ToString()); //update number of lives on HUD
        scoreTxt.text = ("Score: " + score.ToString()); //update score on HUD

        if (!isInPlay)
        {
            Vector3 pos = new Vector3(); 
            pos.x = paddle.transform.position.x; //set vector's x to paddle's x position
            pos.y = gameObject.transform.position.y; //y remains constant
            pos.z = gameObject.transform.position.z; //z remains constant
            transform.position = pos; //set ball transform to transform of pos
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isInPlay = true; //true when ball is shot from paddle
                Move(); //shoot ball from paddle
            }//end if (Input.GetKeyDown(KeyCode.Space))
        }//end if (!isInPlay)

        
    }//end Update()


    private void LateUpdate()
    {
        if (isInPlay)
        {
            rb.velocity = speed * rb.velocity.normalized; //move ball at a constant normalized velocity
        }//end if (isInPlay)

    }//end LateUpdate()


    void SetStartingPos()
    {
        isInPlay = false;//ball is not in play
        rb.velocity = Vector3.zero;//set velocity to keep ball stationary

        Vector3 pos = new Vector3();
        pos.x = paddle.transform.position.x; //x position of paddel
        pos.y = paddle.transform.position.y + paddle.transform.localScale.y; //Y position of paddle plus it's height

        transform.position = pos;//set starting position of the ball 
    }//end SetStartingPos()

    void Move()
    {
        rb.AddForce(0, initialForce, 0); //shoot ball from paddle with inital force
    }//end Move()

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.Play(); //play sound on collision
        GameObject otherGO = collision.gameObject; //reference to collided with game object
        if(otherGO.tag == "Brick")
        {
            score += 100; //add to score when colliding with brick
            Destroy(otherGO); //destroy brick
        }//end if(otherGO.tag == "Brick")
    }//end OnCollisionEnter()

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGO = other.gameObject; //reference to triggered game object
        if(otherGO.tag == "OutBounds")
        {
            numberOfBalls -= 1; //reduce lives by 1
            if(numberOfBalls > 0)
            {
                Invoke("SetStartingPos", 2f); //reset starting position
            }//end if(numberOfBalls > 0)

            if(numberOfBalls <= 0)
            {
                Invoke("RestartScene", 2f); //reset level
            }//end if(numberOfBalls <= 0)
        }
    }//end OnTriggerEnter()

    void RestartScene()
    {
        SceneManager.LoadScene(0); //reloads the level
    }//end RestartScene()
}
