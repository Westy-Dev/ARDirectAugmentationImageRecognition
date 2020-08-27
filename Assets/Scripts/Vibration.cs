using UnityEngine;
using System.Collections;

//https://gist.github.com/aVolpe/707c8cf46b1bb8dfb363
// Original Code By aVolpe
// Edited by Ben Westcott
/// <summary>
/// Handles the vibration of a device
/// </summary>
public static class Vibration
{

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif
    /// <summary>
    /// Vibrates the device
    /// </summary>
    public static void Vibrate()
    {
        //If we are an Android device, call the native function
        if (isAndroid())
        {
            vibrator.Call("vibrate");
        }
        else
        {
            //Otherwise let Unity try and handle the device vibration using its device agnostic function
            Handheld.Vibrate();
        }
    }

    /// <summary>
    /// Vibrates the device for the given amount of time in milliseconds
    /// </summary>
    /// <param name="milliseconds">Time to vibrate in milliseconds</param>
    public static void Vibrate(long milliseconds)
    {
        //If we are an Android device, call the native function
        if (isAndroid())
        {
            vibrator.Call("vibrate", milliseconds);
        }
        else
        {
            //Otherwise let Unity try and handle the device vibration using its device agnostic function
            //NOTE: This does not have an override for time to vibrate
            Handheld.Vibrate();
        }
    }

    /// <summary>
    /// Cancels the vibration on the device
    /// </summary>
    public static void Cancel()
    {
        //If we are an Android device, call the native function to stop the vibrator
        if (isAndroid())
        {
            vibrator.Call("cancel");
        }

    }

    /// <summary>
    /// Checks if the running device is Android or not
    /// </summary>
    /// <returns><c>true</c> if the device is running Android, <c>false</c> if it isn't</returns>
    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	    return true;
#else
        return false;
#endif
    }
}
