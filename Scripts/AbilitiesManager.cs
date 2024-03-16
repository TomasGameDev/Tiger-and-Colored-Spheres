using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TigerAndColoredSpheres
{
    public class AbilitiesManager : MonoBehaviour
    {
        static AbilitiesManager _instance;
        public static AbilitiesManager instance
        {
            get
            {
                if (_instance == null) _instance = GameObject.Find("Abilities manager").GetComponent<AbilitiesManager>();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        [Space]
        [Header("Abilities")]
        [Space]
        public AbilityAttribute[] abilities;

        [Space]
        [Header("UI")]
        [Space]

        public RectTransform abilitiesPanel;
        public GameObject abilitiesPrefab;
        public float abilitiesWidthOffset = 20;

        private void Start()
        {
            CreateAbilitiesPanels();
        }

        public float GetPanelWidth(int index)
        {
            return (abilitiesPrefab.GetComponent<RectTransform>().sizeDelta.x + abilitiesWidthOffset) * index;
        }

        public static void ActivateAbility(AbilityAttribute ability)
        {
            TigerPlayer.instance.currentAbility = ability;
        }
        public List<AbilityPanel> abilityPanels = new List<AbilityPanel>();

        [System.Serializable]
        public class AbilityPanel
        {
            public GameObject panel;
            public Image icon;
            public Text priceText;
            public AbilityAttribute abilityAttribute;

            public AbilityPanel(GameObject _panel, Image _icon, Text _priceText, AbilityAttribute _abilityAttribute)
            {
                panel = _panel;
                icon = _icon;
                icon.sprite = _abilityAttribute.icon;
                priceText = _priceText;
                priceText.text = _abilityAttribute.prtice.ToString();
            }
        }
        public void CreateAbilitiesPanels()
        {
            for (int a = 0; a < abilities.Length; a++)
            {
                InitializeAbilityPanel(abilities[a], a);
            }
            abilitiesPanel.sizeDelta = new Vector2(GetPanelWidth(abilities.Length) + abilitiesWidthOffset, abilitiesPanel.sizeDelta.y);
        }

        public void InitializeAbilityPanel(AbilityAttribute abilityAttribute, int index)
        {
            GameObject abilityPanelObject = Instantiate(abilitiesPrefab);
            abilityPanelObject.name = abilityAttribute.abilityName;

            abilityPanelObject.GetComponent<RectTransform>().SetParent(abilitiesPanel);
            abilityPanelObject.transform.localScale = Vector3.one;

            abilityPanelObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(GetPanelWidth(index) + abilitiesWidthOffset, 0);

            AbilityPanel ballPanel = new AbilityPanel(abilityPanelObject,
                abilityPanelObject.transform.GetChild(0).GetComponent<Image>(),//Icon
                abilityPanelObject.transform.GetChild(1).GetComponent<Text>(),//Text
                abilityAttribute);

            abilityPanelObject.GetComponent<Button>().onClick.AddListener(delegate { OnBuyAbility(abilityAttribute); });

            abilityPanels.Add(ballPanel);
        }

        public void OnBuyAbility(AbilityAttribute ability)
        {
            GameShop.BuyAbility(ability);
        }
    }
}