using UnityEngine;

public class pause : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign your Pause Menu UI Panel here in the Inspector
    private bool isPaused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Or any other key you want to use
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Stops all time-based operations
        isPaused = true;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true); // Show the pause menu
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resumes normal time
        isPaused = false;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // Hide the pause menu
        }
    }

}
