using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public RectTransform loadingImage;
    public float progress;
    public string sceneName = "Game";
    void Update()
    {
        if (progress < 1)
        {
            loadingImage.anchorMax = new Vector2(progress, loadingImage.anchorMax.y);

            progress += Time.deltaTime;

            if (progress >= 1)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
