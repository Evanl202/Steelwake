using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public int targetFPS = 60;

    private void Awake()
    {
        Application.targetFrameRate = targetFPS;
        DontDestroyOnLoad(gameObject);
    }
}