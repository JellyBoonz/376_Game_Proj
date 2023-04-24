using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    PathFinder pathFinder;
    // float stepSize = 1f;
    Grid grid;
    public GameObject pathObject;
    public Transform[] snakeParts;
    public GameObject otherSnake;
    private Vector3[] oldPositions;


    void Awake() {
		// grid = GetComponent<Grid> ();
        
        pathFinder = pathObject.GetComponent<PathFinder>();
        grid = pathFinder.GetComponent<Grid> (); //FIXME: Changed this. used to use the PathFinder's Public grid. Might be able to make it work that way...
    }   

    // Start is called before the first frame update
    void Start()
    {
        // pathFinder = new PathFinder();
    }

    // Update is called once per frame
    void Update()
    {
        pathFinder.FindPath(pathFinder.seeker.position, pathFinder.target.position);
        grid.CreateGrid();

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)){

                int stepCount = 0;
                MoveAlongPath(stepCount);
        }
        
    }
    
    // Updates enemy's position to be the first location in the 
    // path toward the player
    void MoveAlongPath(int stepCount) {
    int pathCount = grid.path.Count;

        oldPositions = new Vector3[snakeParts.Length];

        for (int i = 0; i < snakeParts.Length; i++){
                oldPositions[i] = snakeParts[i].position;
            }
        while (pathCount != 0 && stepCount < 1) {
            // Getting next coordinate for enemy 
            Vector3 nextPos = grid.path[0].worldPosition;
            Vector3 moveDir = (nextPos - snakeParts[0].position).normalized;

            // Rotate the game object to face the movement direction
            if (moveDir != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(moveDir);
            }
            // Move the rest of the snake parts
            for (int i = snakeParts.Length - 1; i > 0; i--) {
                snakeParts[i].position = snakeParts[i - 1].position;
                snakeParts[i].rotation = snakeParts[i - 1].rotation;
            }

            // Move the head of the snake to the next position
            snakeParts[0].position = nextPos;

            // pathFinder.FindPath(pathFinder.seeker.position, pathFinder.target.position);
            
            stepCount++;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves the enemy game object
        if (collision.gameObject == otherSnake)
        {
            for(int i = 0; i < snakeParts.Length; i++){
                snakeParts[i].position = oldPositions[i];
            }
        }
    }       
}
