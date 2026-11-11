using UnityEngine;
using TMPro;

public class GameSettings : MonoBehaviour
{
    public TMP_Dropdown fpsDropdown;

    public TMP_Dropdown resolutionDropdown;

    private readonly Vector2Int[] resolutions =
    {
        new Vector2Int(1280, 720),
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1440),
        new Vector2Int(3840, 2160)
    };
    
    private int[] fpsOptions = { 30, 60, 120, 144 };

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            Debug.Log("Game resolution: " +
                Screen.width + "x" + Screen.height);

            Debug.Log("Monitor resolution: " +
                Screen.currentResolution.width + "x" +
                Screen.currentResolution.height);
        }
    }

    private void Start()
    {
        LoadFPS();
        LoadResolution();
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

    public void ApplyResolution()
    {
        int selectedIndex = resolutionDropdown.value;

        Vector2Int resolution = resolutions[selectedIndex];

        Screen.SetResolution(
            resolution.x,
            resolution.y,
            FullScreenMode.FullScreenWindow
        );

        PlayerPrefs.SetInt("ResolutionIndex", selectedIndex);
        PlayerPrefs.Save();

        Debug.Log("Resolution set to " +
            resolution.x + "x" + resolution.y);
    }

    private void LoadResolution()
    {
        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", 1);

        savedIndex = Mathf.Clamp(savedIndex, 0, resolutions.Length - 1);

        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();

        Vector2Int resolution = resolutions[savedIndex];

        Screen.SetResolution(
            resolution.x,
            resolution.y,
            FullScreenMode.FullScreenWindow
        );
    }
}