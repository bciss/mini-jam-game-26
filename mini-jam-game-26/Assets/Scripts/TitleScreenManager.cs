using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TitleScreenManager : MonoBehaviour
{
    private CustomInput input = null;

    void Awake()
    {
        input = new CustomInput();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Base.Space.performed += ctx => OnSpacePerformed();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnSpacePerformed()
    {
            Debug.Log("Changing scene to : GameScene1");
            SceneManager.LoadScene("GameScene1");
    }
    private void OnDisable()
    {
        input.Disable();
    }
}
