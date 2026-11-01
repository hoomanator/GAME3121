using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    float minSize = 0.5f;
    [SerializeField]
    float maxSize = 2f;

    [SerializeField]
    float minSpeed = 150f;
    [SerializeField]
    float maxSpeed = 350f;

    [SerializeField]
    float maxSpinSpeed = 10f;

    [SerializeField]
    float maxVelocity = 8f; // Max linear speed the obstacle can travel


    [SerializeField]
    GameObject bounceEffect;

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize; // Adjust speed based on size (smaller objects move faster)

        Vector2 randomDirection = Random.insideUnitCircle;

        rb.AddForce(randomDirection * randomSpeed);
        rb.AddTorque(Random.Range(-maxSpinSpeed, maxSpinSpeed));
    }

    // Update is called once per frame
    void Update()
    {

    }

    //If the Obstacle GameObjects become too fast, the game can effectively become impossible. You can modify your obstacle script to limit their velocity using Rigidbody2D.linearVelocity.magnitude.
    // FixedUpdate runs on the physics timestep — clamp velocity here so it
    // stays framerate-independent and in sync with the physics engine
    void FixedUpdate()
    {
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxVelocity);
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxSpinSpeed, maxSpinSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Instantiate the bounce effect at the collision point
        Vector2 contactPoint = collision.GetContact(0).point;
        Instantiate(bounceEffect, contactPoint, Quaternion.identity);

        //Destroy the effect after 1 second
        //Destroy(bounceEffect, 5f);

    }
}
