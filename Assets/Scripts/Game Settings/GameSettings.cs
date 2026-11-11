using UnityEngine;
using TMPro;

public class GameSettings : MonoBehaviour
{
    public TMP_Dropdown fpsDropdown;

    public TMP_Dropdown resolutionDropdown;

    public TMP_Dropdown displayModeDropdown;

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
        if (Input.GetKeyDown(KeyCode.F8))
        {
            Debug.Log("Game resolution: " +
                Screen.width + "x" + Screen.height);

            Debug.Log("Monitor resolution: " +
                Screen.currentResolution.width + "x" +
                Screen.currentResolution.height);

            Debug.Log("Fullscreen mode: " +
                Screen.fullScreenMode);
        }
    }

    private void Start()
    {
        LoadFPS();
        LoadResolution();
        LoadDisplayMode();
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

        int displayModeIndex = displayModeDropdown.value;

        FullScreenMode selectedMode = displayModes[displayModeIndex];

        Screen.SetResolution(
            resolution.x,
            resolution.y,
            selectedMode
        );

        PlayerPrefs.SetInt("ResolutionIndex", selectedIndex);
        PlayerPrefs.Save();

        Debug.Log("Resolution set to " +
            resolution.x + "x" + resolution.y +
            " | Mode: " + selectedMode);
    }

    private readonly FullScreenMode[] displayModes =
    {
        FullScreenMode.Windowed,
        FullScreenMode.FullScreenWindow,
        FullScreenMode.ExclusiveFullScreen
    };

    public void ApplyDisplayMode()
    {
        int selectedIndex = displayModeDropdown.value;

        FullScreenMode selectedMode = displayModes[selectedIndex];

        Vector2Int resolution = resolutions[resolutionDropdown.value];

        Screen.SetResolution(
            resolution.x,
            resolution.y,
            selectedMode
        );

        PlayerPrefs.SetInt("DisplayMode", selectedIndex);
        PlayerPrefs.Save();

        Debug.Log("Display mode set to " + selectedMode);
    }

    private void LoadDisplaySettings()
    {
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 1);
        int savedDisplayModeIndex = PlayerPrefs.GetInt("DisplayMode", 1);

        savedResolutionIndex = Mathf.Clamp(
            savedResolutionIndex, 0, resolutions.Length - 1
        );

        savedDisplayModeIndex = Mathf.Clamp(
            savedDisplayModeIndex, 0, displayModes.Length - 1
        );

        // Update dropdowns
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        displayModeDropdown.value = savedDisplayModeIndex;
        displayModeDropdown.RefreshShownValue();

        // Apply saved settings
        Vector2Int resolution = resolutions[savedResolutionIndex];

        Screen.SetResolution(
            resolution.x,
            resolution.y,
            displayModes[savedDisplayModeIndex]
        );
    }
}