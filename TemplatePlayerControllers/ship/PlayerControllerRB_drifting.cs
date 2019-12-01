using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRB_drifting : MonoBehaviour {

    private Rigidbody2D rb;
    [SerializeField] [Range(0,50)] private int playerSpeed = 37; // how fast player speeds up
    [SerializeField] [Range(0,50)] private float rotationSpeed = 2.2f; //speed player rotates at 
    [SerializeField] [Range(0,100)] private float maxSpeed = 16; //speed player moves
    // [SerializeField] [Range(0,100)] private float maxBoost = 10; //speed player moves
    [SerializeField] private bool breaking = false;
    [SerializeField] private float boostForce = 0;
    [SerializeField] private float boostScale;
    [SerializeField] private float BOOSTCOOLDOWN;
    
    private Vector2 targetPos = Vector2.zero;
    private float xAxis, yAxis;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update() { 
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");   
    }

    void FixedUpdate() {
        HandleInput(); // Player Input
    }

    void HandleInput() {
        // Check for breaking
        float remaingBoostCooldown = TickBoostCooldown();
        breaking = (yAxis < 0);
        if(breaking) {
            if(remaingBoostCooldown == 0) {
                Brake(Mathf.Abs(yAxis) * (playerSpeed/100f));
            }
            yAxis = 0;
        } else {
            if(boostForce > 0) {
                BurstForward();
            }
        }

        // Rotation
        Rotate(transform, xAxis * (rotationSpeed + rb.angularDrag));

        // Regular movement
        ThrustForward(yAxis * playerSpeed);


        // Filter speed
        ClampVelocity();

        // TODO Angular Momentum from slowdown -> speedup on slowing rotation
        // idea is to calculate difference in velocity between fixed updates, 
            // then add that as torque, 
            //  but scaled with respect angular difference between transform.up and the movement direction (rb.velocity.normalized)
    }

    #region Maneuvering API
        private void ClampVelocity() {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
        private void Rotate(Transform t, float amount) {
            rb.AddTorque(-amount);
        }

        private void ThrustForward(float amount) {
            rb.AddForce(transform.up * amount);
        }

        private void BurstForward() {
            // Add force
            rb.AddForce(transform.up * (boostForce * 1000f) * boostScale, ForceMode2D.Impulse);
            rb.velocity = rb.velocity.normalized * (boostForce * 1000f) * boostScale;

            // Setup cooldown
            boostForce = 0;
            BOOSTCOOLDOWN = 75;
        }

        private float TickBoostCooldown() {
            BOOSTCOOLDOWN = Mathf.Clamp(BOOSTCOOLDOWN - 1f, 0, Mathf.Infinity);
            return BOOSTCOOLDOWN;
        }

        private void Brake(float amount) {
            var brake = (rb.velocity.normalized * amount);
            if(brake.magnitude * 4f > rb.velocity.magnitude) {
                boostForce += rb.velocity.magnitude;
                rb.velocity = Vector2.zero;
                BurstForward();
            } else {
                boostForce += brake.magnitude;
                rb.velocity =  rb.velocity - brake;
            }
        }
    #endregion

}


