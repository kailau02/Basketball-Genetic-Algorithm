using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeControl : MonoBehaviour
{
    Rigidbody2D rb2d;
    public float minAngle = 70f;
    public float maxAngle = 100f;
    public float bufferAngle = 5f;
    public float lerpTime = 10f;
    public float targetVel = 200f;

    public bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        this.rb2d = GetComponent<Rigidbody2D>();
        rb2d.centerOfMass = Vector2.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkIsActive();
        float currVel = rb2d.angularVelocity;
        float currAngle = transform.eulerAngles.z;

        // ACTIVE
        if (activated) {
            if (currAngle >= maxAngle) {
                transform.rotation = Quaternion.Euler(0,0,maxAngle + 0.001f);
                rb2d.angularVelocity = 0f;
            } else {
                rb2d.angularVelocity = Mathf.Lerp(currVel, targetVel, lerpTime);
            }
        }
        // NOT ACTIVE
        else {
            if (currAngle <= minAngle) {
                transform.rotation = Quaternion.Euler(0,0,minAngle - 0.001f);
                rb2d.angularVelocity = 0f;
            } else {
                rb2d.angularVelocity = Mathf.Lerp(currVel, -targetVel, lerpTime);
            }        
        }
    }

    void checkIsActive() {
        try
        {
            Transform ball = GameObject.FindWithTag("ball").transform;
            Rigidbody2D ballRb2d = ball.GetComponent<Rigidbody2D>();
            Transform target = GameObject.FindWithTag("target").transform;
            float[] Y = Game.geneticAlg.networks[Game.currNetworkIndex].feedForward(new float[] {ball.position.x, ball.position.y, ballRb2d.velocity.x, ballRb2d.velocity.y, rb2d.angularVelocity, activated ? 1 : 0, target.position.x, target.position.y});
            activated = Y[0] > 0;
        }
        catch (System.Exception){}
    }

    void Update() {
        // Get the current rotation of the GameObject
        float currentAngle = transform.eulerAngles.z;

        // Clamp the rotation to the specified range
        float clampedAngle = Mathf.Clamp(currentAngle, minAngle - 0.001f, maxAngle + 0.001f);

        // Set the rotation of the GameObject to the clamped angle
        transform.eulerAngles = new Vector3(0f, 0f, clampedAngle);
    }

    public void resetHinge() {
        transform.rotation = Quaternion.Euler(0,0,70);
        activated = false;
    }
}
