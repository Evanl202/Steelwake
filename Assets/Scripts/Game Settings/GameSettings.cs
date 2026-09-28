using UnityEngine;
using TMPro;

public class GameSettings : MonoBehaviour
{
    public TMP_Dropdown fpsDropdown;
    
    private int[] fpsOptions = { 30, 60, 120, 144 };

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadFPS();
    }

    public void ApplyFPS()
    {
        int selectedFPS = fpsOptions[fpsDropdown.value];

        Application.targetFrameRate = selectedFPS;

        PlayerPrefs.SetInt("FPS", selectedFPS);
        PlayerPrefs.Save();

        Debug.Log("FPS set to " + selectedFPS);
    }

    private void LoadFPS()
    {
        int savedFPS = PlayerPrefs.GetInt("FPS", 60);

        int index = 1;

        for (int i = 0; i < fpsOptions.Length; i++)
        {
            if (fpsOptions[i] == savedFPS)
            {
                index = i;
                break;
            }
        }

        fpsDropdown.value = index;
        fpsDropdown.RefreshShownValue();

        Application.targetFrameRate = savedFPS;
    }
}