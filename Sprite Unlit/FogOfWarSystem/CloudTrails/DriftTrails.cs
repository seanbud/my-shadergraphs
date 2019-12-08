using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftTrails : MonoBehaviour {
    public TrailRenderer leftTrail, rightTrail;
    [SerializeField] private float maxTime, minTime, maxWidth, minWidth;

    private Rigidbody2D rb;
    [SerializeField] private float maxSpeed;
    
    private void Awake() {
        maxSpeed = GetComponentInParent<PlayerControllerRB_drifting2>().maxSpeed;
        rb = GetComponentInParent<Rigidbody2D>();

        // Length
        maxTime = leftTrail.time;
        minTime = maxTime * 0.2f;
        
        // Width
        maxWidth = leftTrail.widthMultiplier;
        minWidth = maxWidth * 0.2f;
    }
    
    private void FixedUpdate() {
        // Adjust trail width based on speed ratio
        var speedRatio = rb.velocity.magnitude / maxSpeed;
        var scale = Mathf.Clamp(Mathf.Pow(speedRatio, 3), minWidth, maxWidth);
        leftTrail.widthMultiplier = rightTrail.widthMultiplier = scale;
        // Debug.Log(leftTrail.widthMultiplier);

        // TODO adjust length based on torque
    }

}
