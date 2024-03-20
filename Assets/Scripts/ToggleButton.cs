using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TigerAndColoredSpheres
{
    public class ToggleButton : MonoBehaviour, ILoadData
    {
        public string playerPrefsKey, positiveCommandName, negativeCommandName;
        public GameObject target;
        public bool status = true;
        public Image toggleImage;
        public Button toggleButton;
        private void Start()
        {
            toggleButton.onClick.AddListener(OnToggle);
        }

        void OnToggle()
        {
            status = !status;

            target.GetComponent<IOnToggle>().OnToggle(status ? positiveCommandName : negativeCommandName);
            toggleImage.enabled = status;
            PlayerPrefs.SetString(playerPrefsKey, status ? positiveCommandName : negativeCommandName);
        }

        public void LoadData()
        {
            string savedCommand = PlayerPrefs.GetString(playerPrefsKey);

            if (savedCommand == positiveCommandName)
            {
                status = false;

                OnToggle();
            }
            else if (savedCommand == negativeCommandName)
            {
                status = true;

                OnToggle();
            }
        }
    }
    public interface IOnToggle
    {
        public void OnToggle(string commandName);
    }
}