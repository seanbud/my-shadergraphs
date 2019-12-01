using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRB_ship : MonoBehaviour {

    private Rigidbody2D rb;
    [SerializeField] [Range(0,50)] private int playerSpeed = 37; // how fast player speeds up
    [SerializeField] [Range(0,50)] private float rotationSpeed = 2.2f; //speed player rotates at 
    [SerializeField] [Range(0,100)] private float maxSpeed = 16; //speed player moves
    private float xAxis, yAxis; // Input
    
    private Vector2 targetPos = Vector2.zero;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update() {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        HandleInput(); // Player Input
    }
    
    void HandleInput() {

        // Check for breaking
        if(yAxis < 0) {
            Brake(Mathf.Abs(yAxis) * (playerSpeed/100f));
            yAxis = 0;
        }

        // Regular movement
        ThrustForward(yAxis * playerSpeed);

        // Rotation
        Rotate(transform, xAxis * rotationSpeed);

        // Filter speed
        ClampVelocity();
    }

    #region Maneuvering API
        private void ClampVelocity() {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
        private void Rotate(Transform t, float amount) {
            t.Rotate(0, 0, -amount);
        }

        private void ThrustForward(float amount) {
            rb.AddForce(transform.up * amount);
        }

        private void Brake(float amount) {
            var brake = (rb.velocity.normalized * amount);
            if(brake.magnitude > rb.velocity.magnitude) {
                rb.velocity = Vector2.zero;
            } else {
                rb.velocity =  rb.velocity - brake;
            }
        }



    #endregion

}


