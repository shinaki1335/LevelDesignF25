using UnityEngine;
 using TMPro;

public class StopwatchDisplay : MonoBehaviour
{
    // Reference to a UI Text component (assign in Inspector)
    public TextMeshProUGUI timeText;
    // public TextMeshProUGUI timeText; // Use this instead if using TextMeshPro

    private float elapsedTime = 0f;
    private bool isRunning = true;

    private void Start()
    {
        isRunning = false;

    }
    void Update()
    {
        // Stop timer on Space press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isRunning)
            {
                isRunning = true;

            }
            else
            {
                isRunning = false;

            }
        }

        // Update timer if running
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
        }

        // Format and display time
        float seconds = Mathf.Floor(elapsedTime);
        float milliseconds = (elapsedTime - seconds) * 1000f;
        timeText.text = $"{seconds:0}.{milliseconds:000}";
    }
}
