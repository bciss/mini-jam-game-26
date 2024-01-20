using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

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
        
    }
    private void OnDisable()
    {
        input.Disable();
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
