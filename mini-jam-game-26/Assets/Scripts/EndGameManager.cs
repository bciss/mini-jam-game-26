using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    private CustomInput input = null;
    public Camera cam;
    public Vector2 mouseLook;
    public float mouseSensitivity = 100f;
    private float xRotation;
    private float yRotation;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;
        input = new CustomInput();
        input.Base.Pause.performed += ctx => BackToTitle();
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Look();
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

    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
