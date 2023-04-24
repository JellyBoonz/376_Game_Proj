using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/***************** To Do *****************
1. The Player is NOT being controlled using
delta time. May need to fix.


*****************************************/
public class PlayerController : MonoBehaviour
{
    // public float moveSpeed = 5f;
    public float stepSize = 1f;
    private bool shouldMove = false;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private bool hasPlayedSound = false;

    public GameObject enemy1;
    public GameObject enemy2; 
    public AudioSource crunchSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Read user input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Check if the user has clicked a movement key
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            shouldMove = true;
            moveDirection = new Vector3(horizontal, 0f, vertical);
        }
    }

    private void FixedUpdate()
    {
        // Move the character in discrete steps if shouldMove flag is true
        if (shouldMove)
        {
            rb.MovePosition(transform.position + moveDirection.normalized * stepSize);
            shouldMove = false;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves the enemy game object
        if (collision.gameObject == enemy1 || collision.gameObject == enemy2)
        {
            if (!hasPlayedSound)
            {
                crunchSound.Play();
                hasPlayedSound = true;
                // Start a coroutine to wait for the sound to finish playing
                StartCoroutine(WaitForSound());
            }
        }
    }

    IEnumerator WaitForSound()
    {
        // Wait for the duration of the audio clip
        yield return new WaitForSeconds(crunchSound.clip.length);

        // Load the new scene
        SceneManager.LoadScene("Menu");
    }
}
