using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float thrust = 1f;

    float maxSpeed = 5f;
    Rigidbody2D rb;

    [SerializeField]
    GameObject boosterFlame;

    [SerializeField]
    GameObject explosionEffect;


    float elapsedTime = 0f;

    float score = 0f;
    float scoreMultiplier = 10f;

    [SerializeField]
    private PanelRenderer panel;

    [SerializeField]
    private VisualElementReference<Label> labelScoreReference;



    [SerializeField]
    private VisualElementReference<Button> buttonReference;

    private Label scoreText;

    private Button restartButton;

    private HighScoreManager highScoreManager;

    public GameObject borderParent;

    public InputAction moveForward;
    public InputAction lookPosition;


    private void Awake()
    {
        //Debug.Log("Awake");
        panel.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnEnable()
    {
        //Debug.Log("On Enable");
    }

    private void OnDestroy()
    {
        panel.UnregisterUIReloadCallback(OnUIReload);
        labelScoreReference.UnregisterReferenceUnloadedCallback(HandleScoreLabelReferenceCallback);
        buttonReference.UnregisterReferenceUnloadedCallback(HandleButtonReferenceCallback);

    }

    private void OnUIReload(PanelRenderer panel, VisualElement root)
    {
        Debug.Log("UI Reload on frame: " + Time.frameCount);
        labelScoreReference.RegisterReferenceResolvedCallback(HandleScoreLabelReferenceCallback);
        buttonReference.RegisterReferenceResolvedCallback(HandleButtonReferenceCallback);
    }

    private void HandleScoreLabelReferenceCallback(Label label)
    {
        //Debug.Log("Label Reference Callback");
        scoreText = label;
        //scoreText.text = "Callback!";
    }



    private void HandleButtonReferenceCallback(Button button)
    {
        Debug.Log("Button Reference Callback");
        restartButton = button;
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveForward.Enable();
        lookPosition.Enable();

        // If not assigned via the Inspector, try finding it in the scene automatically
        if (highScoreManager == null)
        {
            highScoreManager = FindAnyObjectByType<HighScoreManager>();
            //reset the high score to 0 for testing purposes
            //PlayerPrefs.SetFloat("HighScore", 0);
            highScoreManager.CheckAndSaveHighScore(0); // Ensure the high score is initialized
        }

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        //Debug.Log("Elapsed Time: " + elapsedTime + " seconds"); 
        //Debug.Log("Score: " + score);
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }

        if (Mouse.current.leftButton.isPressed || moveForward.IsPressed())
        {
            //Screen space — a coordinate system based on pixels where (0, 0) starts at the lower-left corner of the screen.

            //Vector3 mousePosition = Mouse.current.position.ReadValue();
            //does not support mobile
            //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            //Debug.Log("Left mouse button is pressed @ " + worldPosition);

            //Support mobile
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(lookPosition.ReadValue<Vector2>());
            //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            //Vector3 direction = worldPosition - transform.position;


            Vector3 direction = mousePosition - transform.position;
            direction.z = 0; // Ignore the z-axis for 2D movement   
            transform.up = direction.normalized; // Rotate the player to face the mouse position
            
            //thrustAudioSource.Play();

            if(rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }

            rb.AddForce(transform.up * thrust); // Apply force in the direction the player is facing       
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed; // Limit the player's speed to maxSpeed
            }

        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            boosterFlame.SetActive(true);            
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            boosterFlame.SetActive(false);
        }

        //Game Over logic: Check and save high score
            if (highScoreManager != null)
            {
                // Pass the current high game score and display whhen the scene is reloaded
                highScoreManager.CheckAndSaveHighScore(0);
            }
        

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Player collided with an obstacle!");
            // Handle collision logic here (e.g., reduce health, play sound, etc.)
            Destroy(gameObject); // Destroy the player object on collision
            //DestroyImmediate(gameObject,true);

            // Instantiate the explosion effect at the player's position
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosionEffect, 1f); // Destroy the effect after 1 second
            }

            restartButton.style.display = DisplayStyle.Flex; // Show the restart button

            //Game Over logic: Check and save high score
            if (highScoreManager != null)
            {
                // Pass the current game score to check and save if it's a new record
                highScoreManager.CheckAndSaveHighScore(score);
            }
            else
            {
                Debug.LogError("HighScoreManager reference is missing!");
            }

            //make the screen borders disappear on game over so obstacles fly off-screen instead of endlessly rebounding
            borderParent.SetActive(false);
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);        
    }

}
