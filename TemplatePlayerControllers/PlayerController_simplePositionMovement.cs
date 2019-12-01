using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_simplePositionMovemnt : MonoBehaviour
{

    public bool autofly = false;
    public bool updownfly = false;

    [SerializeField]
    [Range(5,50)]
    private int playerSpeed = 10; //speed player moves

    private Vector2 targetPos = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput(); // Player Input
        if(autofly) AutoFly(); // Simulate input with random auto flying
        if(updownfly) UpDownFly(); // Simulate input with random auto flying
    }
    
    void HandleInput()
    {
        if (Input.GetKey("up")) MoveUp();
        if (Input.GetKey("down")) MoveDown();
        if (Input.GetKey("left")) MoveLeft();
        if (Input.GetKey("right")) MoveRight();
    }

    void AutoFly() {
        // Reset target
        if(Vector2.Distance(transform.position, targetPos) < 3.0f) {
            if(Time.frameCount % 150 == 0)
                targetPos = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
        }

        if(Mathf.Abs(transform.position.x - targetPos.x) > 1.5f) {
            if(transform.position.x < targetPos.x) {
                MoveRight();
            } else if(transform.position.x > targetPos.x) {
                MoveLeft();
            } 
        }
        
        else if(Mathf.Abs(transform.position.y - targetPos.y) > 1.5f) {
            if(transform.position.y < targetPos.y) {
                MoveUp();
            } else if(transform.position.y > targetPos.y) {
                MoveDown();
            } 
        }
    }

    bool goingUp = true;
    void UpDownFly() {
        int max = 5, min = -5;
        if(goingUp) {
            if(transform.position.y < max) {
                MoveUp();
            } else {
                goingUp = false;
            }
        } else {
            if(transform.position.y > min) {
                MoveDown();
            } else {
                goingUp = true;
            }
        }
    }


    void MoveUp() {
        transform.position += Vector3.up * playerSpeed * Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(270, Vector3.forward);
    }
    void MoveDown() {
        transform.position += Vector3.down * playerSpeed * Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
    }
    void MoveLeft() {            
        transform.position += Vector3.left * playerSpeed * Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
    }
    void MoveRight() {
        transform.position += Vector3.right * playerSpeed * Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
    }
}


