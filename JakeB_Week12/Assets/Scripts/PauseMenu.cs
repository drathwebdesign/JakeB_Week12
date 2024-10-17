using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenuUI;
    public GameObject player;
    private playerMovement playerMovementScript;

    private bool isPaused = false;

    void Start() {
        playerMovementScript = player.GetComponent<playerMovement>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        playerMovementScript.enabled = true;
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        playerMovementScript.enabled = false;
    }
}