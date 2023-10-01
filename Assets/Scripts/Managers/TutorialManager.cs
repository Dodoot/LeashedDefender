using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;

    private bool _gameStarted;
    private bool _isPaused;

    private void Update()
    {
        if (!_gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _animator.SetBool("No", true);
                _gameStarted = true;
                EnemiesManager.StartWavePlease();
            }
        }
        else
        {
            if (!_isPaused && Input.GetKeyDown(KeyCode.Escape))
            {
                _animator.SetBool("No", false);
                _isPaused = true;
                TimeManager.StopTime();
            }
            else if (_isPaused && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)))
            {
                _animator.SetBool("No", true);
                _isPaused = false;
                TimeManager.ResetTime();
            }
        }
    }
}
