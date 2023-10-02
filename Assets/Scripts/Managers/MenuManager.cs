using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void OnButtonPlayClicked()
    {
        SceneManager.LoadScene("Game");
    }
}
