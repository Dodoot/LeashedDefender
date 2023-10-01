using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void OnButtonPlayClicked()
    {
        SceneManager.LoadScene("Game");
    }
}
