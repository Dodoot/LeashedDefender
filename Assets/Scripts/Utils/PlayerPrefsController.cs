using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{
    const string SOUNDS_VOLUME_KEY = "soundsVolume";
    const string MUSIC_VOLUME_KEY = "musicVolume";
    const string FULLSCREEN_KEY = "fullscreen";
    const string RESOLUTION_KEY = "resolution";
    const string RESOLUTION_SET_KEY = "resolutionSet";

    public static int SoundsVolume => PlayerPrefs.GetInt(SOUNDS_VOLUME_KEY, 3);
    public static int MusicVolume => PlayerPrefs.GetInt(MUSIC_VOLUME_KEY, 3);
    public static bool IsFullScreen => PlayerPrefs.GetInt(FULLSCREEN_KEY, 1) != 0;
    public static bool HasSetResolution => PlayerPrefs.GetInt(RESOLUTION_SET_KEY, 0) != 0;
    public static int ResolutionIndex => PlayerPrefs.GetInt(RESOLUTION_KEY, 0);

    public static void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void SetSoundsVolume(int value)
    {
        PlayerPrefs.SetInt(SOUNDS_VOLUME_KEY, value);
    }

    public static void SetMusicVolume(int value)
    {
        PlayerPrefs.SetInt(MUSIC_VOLUME_KEY, value);
    }

    public static void SetFullScreen(bool value)
    {
        PlayerPrefs.SetInt(FULLSCREEN_KEY, value ? 1 : 0);
    }

    public static void SetResolutionIndex(int value)
    {
        PlayerPrefs.SetInt(RESOLUTION_KEY, value);
    }

    public static void SetHasSetResolution(bool value)
    {
        PlayerPrefs.SetInt(RESOLUTION_SET_KEY, value ? 1 : 0);
    }
}