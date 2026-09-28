using UnityEngine;
using UnityEngine.UIElements;

public class HighScoreManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";
    public float currentHighScore;

    [SerializeField]
    private PanelRenderer panel;

     [SerializeField]
    private VisualElementReference<Label> labelHighScoreReference;

    private Label highScoreText;

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
       
        labelHighScoreReference.UnregisterReferenceUnloadedCallback(HandleHighScoreLabelReferenceCallback);
        
    }

    private void OnUIReload(PanelRenderer panel, VisualElement root)
    {
       
        labelHighScoreReference.RegisterReferenceResolvedCallback(HandleHighScoreLabelReferenceCallback);
       
    }

    private void HandleHighScoreLabelReferenceCallback(Label label)
    {
        //Debug.Log("High Score Label Reference Callback");
        highScoreText = label;
        //highScoreText.text = "Callback!";
    }

    void Start()
    {
        // Load the saved high score when the game starts (defaults to 0 if none exists)
        currentHighScore = PlayerPrefs.GetFloat(HighScoreKey, 0);
        Debug.Log("Current High Score: " + currentHighScore);
        highScoreText.text = "High Score: " + currentHighScore.ToString();
    }

    public void CheckAndSaveHighScore(float newScore)
    {
        // Only update if the new score exceeds the stored high score
        if (newScore > currentHighScore)
        {
            currentHighScore = newScore;
            PlayerPrefs.SetFloat(HighScoreKey, currentHighScore);
            PlayerPrefs.Save(); // Force write to disk
            Debug.Log("New High Score Saved: " + currentHighScore);
            
        }
        highScoreText.text = "High Score: " + currentHighScore.ToString();
    }
}
