using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private CustomInput input = null;
    public GameObject pausePanel;
    
    void Awake()
    {
        input = new CustomInput();
        input.Base.Pause.performed += ctx => TriggerPausePanel();
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
        Debug.Log(pausePanel.activeSelf);
        if (pausePanel.activeSelf)
        {
             Time.timeScale = 1;
             pausePanel.SetActive(false);
        }
        else if (!pausePanel.activeSelf)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
    }

    public void BackToMenu()
    {
        Debug.Log("Changing scene to : TitleScreen");
        SceneManager.LoadScene("TitleScreen");
    }
}
