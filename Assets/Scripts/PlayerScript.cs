using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;
    public float jumpForce;
    public Text scoreText;
    public Text livesText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public Animator anim;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    private int scoreValue = 0;
    private int livesValue = 3;
    private int maxlivesValue =3;
    private int winmusicValue =0;
    private int mover = 0;
    private bool facingRight = true;
    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        scoreText.text = scoreValue.ToString();
        livesText.text = "Lives: " + livesValue.ToString();
        anim = GetComponent<Animator>();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        musicSource.clip = musicClipOne; 
        musicSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }

        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (hozMovement > 0 || hozMovement < 0)
            {
                anim.SetBool("Run", true);
            }

        if (hozMovement == 0)
        {
            anim.SetBool("Run", false);
        }
        if (vertMovement > 0 && isOnGround == false)
        {
            anim.SetBool("Jump", true);
        }

        if (vertMovement == 0 && isOnGround == true)
        {
            anim.SetBool("Jump", false);
        }

        {
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            scoreText.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            livesText.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }
       
        if(livesValue == 0)
        {
            loseTextObject.SetActive(true);
        }

       if (scoreValue == 9 && winmusicValue == 0)
       {
           winTextObject.SetActive(true);
           musicSource.clip = musicClipTwo;
           musicSource.Play();
           winmusicValue += 1;
       }

       if (scoreValue == 4 && mover == 0)
       {
           transform.position = new Vector3(70.0f, 0.0f, 0.0f);
           livesValue = maxlivesValue;
           livesText.text = "Lives: " + livesValue.ToString();
           mover +=1;
       }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
