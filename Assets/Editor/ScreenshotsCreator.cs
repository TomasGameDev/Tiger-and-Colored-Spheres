#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScreenshotsCreator : MonoBehaviour
{
    [MenuItem("Assets/Take screenshot")]
    static void TakeScreenshot()
    {
        string screenshotName = "Screenshots/Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + " x" + Screen.height + " .png";
        ScreenCapture.CaptureScreenshot(screenshotName);

        screenshotName = "Screenshots/Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + " x" + Screen.height + " (1) .png";

        ScreenCapture.CaptureScreenshot(screenshotName);
        Debug.Log("Screenshot saved as: " + screenshotName);
    }
}
#endif