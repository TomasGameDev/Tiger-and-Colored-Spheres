using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TigerAndColoredSpheres
{
    public class SoundsManager : MonoBehaviour, IOnToggle
    {
        static SoundsManager _instance;
        public static SoundsManager instance
        {
            get
            {
                if (_instance == null) _instance = GameObject.Find("Sounds manager").GetComponent<SoundsManager>();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public AudioClip[] audioClips;
        bool isMuted;
        static AudioClip FindSound(string soundName)
        {
            for (int a = 0; a < instance.audioClips.Length; a++)
            {
                if (instance.audioClips[a].name.Contains(soundName)) return instance.audioClips[a];
            }
            return null;
        }
        AudioSource ad;
        public AudioSource backgroundMusic;
        private void Start()
        {
            ad = GetComponent<AudioSource>();
            instance = this;
        }
        public static void PlaySound(string soundName)
        {
            if (!instance.isMuted) instance.ad.PlayOneShot(FindSound(soundName));
        }

        public static void PlaySoundOutScene(string soundName)
        {
            if (instance.isMuted) return;
            GameObject newAd = Instantiate(instance.ad.gameObject);
            Destroy(newAd.GetComponent<SoundsManager>());
            AudioClip clip = FindSound(soundName);
            newAd.GetComponent<AudioSource>().clip = clip;
            newAd.GetComponent<AudioSource>().Play();
            newAd.name = clip.name + " (Auto destroy)";
            DontDestroyOnLoad(newAd);
            Destroy(newAd, clip.length + 1f);
        }

        public void OnToggle(string commandName)
        {
            switch (commandName)
            {
                case "Mute sound":
                    isMuted = true;
                    break;
                case "Unmute sound":
                    isMuted = false;
                    break;
                case "Mute music":
                    backgroundMusic.enabled = false;
                    break;
                case "Unmute music":
                    backgroundMusic.enabled = true;
                    break;
                default:
                    isMuted = true;
                    backgroundMusic.enabled = true;
                    break;
            }
        }
    }
}