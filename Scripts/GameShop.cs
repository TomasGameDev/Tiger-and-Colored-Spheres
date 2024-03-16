using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TigerAndColoredSpheres
{
    public class GameShop : MonoBehaviour
    { 
        static GameShop _instance;
        public static GameShop instance
        {
            get
            {
                if (_instance == null) _instance = GameObject.Find("Game shop").GetComponent<GameShop>();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        [Space]
        [Header("Balance")]
        [Space]
        public int ballPrice;
        public int startBalance;
        [Space]
        [Header("UI")]
        [Space]
        public Text balanceText;

        public static int balance
        {
            get
            {
                return PlayerPrefs.GetInt("BALANCE");
            }
            set
            {
                instance.balanceText.text = value.ToString();
                PlayerPrefs.SetInt("BALANCE", value);
            }
        }
        private void Start()
        {
            if (startBalance > 0 && PlayerPrefs.GetInt("START_BALANCE") == 0)
            {
                balance = startBalance;
                PlayerPrefs.SetInt("START_BALANCE", 1);
            }
            balance = balance;
        }
        public static void BuyAbility(AbilityAttribute ability)
        { 

            if (balance >= ability.prtice)
            {
                if (TigerPlayer.instance.currentAbility != null)
                {
                    PlayerHasAbility();
                    return;
                }
                balance -= ability.prtice;
                AbilitiesManager.ActivateAbility(ability);
            }
            else
            {
                PurchaseUnsuccessful();
            }
        }

        public static void PurchaseUnsuccessful()
        {
            PanelsManager.OpenPurchaseUnsuccessfulPanel();
        }

        public static void PlayerHasAbility()
        {

        }

    }
}