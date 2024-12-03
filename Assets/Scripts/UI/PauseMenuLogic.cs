using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PauseMenuLogic : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private GameObject pauseMenuUI; // El contenedor del menú de pausa
    [SerializeField] private MusicController musicController;
    private bool isPaused = false;
    private DefaultInputMap playerInput;

    private void Awake()
    {
        playerInput = new DefaultInputMap();
    }

    private void Start()
    {
        ActionEventsSubscription();
        pauseMenuUI.SetActive(false);
    }

    private void ResumeGameAction(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();
            musicController.ResumeAll();
        }
        else
        {
            PauseGame();
            musicController.PauseAll();
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo esté reanudado
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reiniciar la escena actual
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo esté reanudado
        SceneManager.LoadScene(0); // Cargar la escena principal (asumida como índice 0)
    }

    private void PauseGame()
    {
        hudCanvas.SetActive(false);
        pauseMenuUI.SetActive(true); // Mostrar el menú de pausa
        Time.timeScale = 0f; // Detener el tiempo del juego
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Ocultar el menú de pausa
        hudCanvas.SetActive(true);
        Time.timeScale = 1f; // Reanudar el tiempo del juego
        isPaused = false;
    }

    #region Input System Methods
    private void ActionEventsSubscription()
    {
        ControllsSubscription();
    }

    private void ActionEventsUnSubscription()
    {
        ControllsUnSubscription();
    }

    private void OnEnable()
    {
        playerInput.Menu.Enable();
    }

    private void OnDisable()
    {
        playerInput.Menu.Disable();
    }

    #region Action Events
    private void ControllsSubscription()
    {
        playerInput.Menu.Back.performed += ResumeGameAction;
    }

    private void ControllsUnSubscription()
    {
        playerInput.Menu.Back.performed -= ResumeGameAction;
    }
    #endregion
    #endregion
}
