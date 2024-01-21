using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Unity.Tutorials.Core.Editor;

public class PlayerInfos
{
    // in game values
    public float moneyAmount;
    public float suspicionAmount;

    // meta game values
    public int level = 0;
}

public class GameManager : MonoBehaviour
{
    
    public AudioMixer masterMixer;
    public static GameManager Instance { get; private set; }
    private CustomInput input = null;
    public GameObject pausePanel;
    public GameObject optionPanel;
    public DayLoop dayLoop;
    public Camera cam;
    public PlayerCameraController cameraController;
    public Vector2 mouseLook;
    public float mouseSensitivity = 100f;
    private float xRotation;
    private float yRotation;
    
    void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(this); }
        DontDestroyOnLoad(this);
        Time.timeScale = 1;

        input = new CustomInput();
        input.Base.Pause.performed += ctx => TriggerPausePanel();
        input.Base.Denied.performed += ctx => dayLoop.DeniedPressed();
        input.Base.Approved.performed += ctx => dayLoop.ApprovedPressed();
    }
    
    private void OnEnable()
    {
        input.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Look();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    private void Look()
    {
        mouseLook = input.Base.Rotation.ReadValue<Vector2>();

        float mouseX = -mouseLook.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseLook.y * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        yRotation -= mouseX;
        xRotation = Mathf.Clamp(xRotation, -75, 75);
        yRotation = Mathf.Clamp(yRotation, -75, 75);
        
        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void TriggerPausePanel()
    {
        Debug.Log("Escape pressed");
        if (pausePanel.activeSelf)
        {
             Time.timeScale = 1;
             pausePanel.SetActive(false);
        }
        else if (optionPanel.activeSelf)
        {
            optionPanel.SetActive(false);
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 0;
            optionPanel.SetActive(false);
            pausePanel.SetActive(true);
        }
    }

    public void Option()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        optionPanel.SetActive(!optionPanel.activeSelf);
    }

    public void BackToMenu()
    {
        Debug.Log("Changing scene to : TitleScreen");
        SceneManager.LoadScene("TitleScreen");
    }

    #region Settings

    public void SetMasterLvl(float masterLvl)
    {
        masterMixer.SetFloat("Master Volume", masterLvl);
    }
    public void SetMusicLvl(float musicLvl)
    {
        masterMixer.SetFloat("Music Volume", musicLvl);
    }
    public void SetSFXLvl(float sfxLvl)
    {
        masterMixer.SetFloat("SFX Volume", sfxLvl);
    }

    #endregion
}
