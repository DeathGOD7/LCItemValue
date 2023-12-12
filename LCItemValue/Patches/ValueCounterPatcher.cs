using GameNetcodeStuff;
using HarmonyLib;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace LCItemValue.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class ValueCounterPatcher
	{
		private static GameObject _itemValueCounter;
		private static TextMeshProUGUI _textValueCounter;
		private static float _displayTimeLeft;
		private const float DisplayTime = 5f;

		[HarmonyPrefix]
		[HarmonyPatch("PingScan_performed")]
		private static void onScan(CallbackContext context)
        {
			ItemValue.getLogger().LogDebug("Method is being invoked of (onScan)");
			_displayTimeLeft = DisplayTime;

			if (GameNetworkManager.Instance.localPlayerController == null)
				return;


			ItemValue.getLogger().LogDebug("Player Object Name : " + GameNetworkManager.Instance.localPlayerController.name);
			if (context.performed
                //&& !GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom
				)
			{
				ItemValue.getLogger().LogDebug("Actual code is being invoked of (onScan)");

				if (!_itemValueCounter)
				{
                    LoadValueCounter();
				}

				float heldValue = CalculateHeldValue();
				_textValueCounter.text = $"<align=\"left\">ITEMS VALUE : ${heldValue}";
				_displayTimeLeft = DisplayTime;
				if (!_itemValueCounter.activeSelf)
				{
					((MonoBehaviour)GameNetworkManager.Instance).StartCoroutine(ShowValues());
				}
			}

			

		}

		// <align="left">ITEMS VALUE: $20000
		// transform.localpos = 30 -75 0
		// transform.rotat = 0 0 0.0175 0.9998

		private static IEnumerator ShowValues()
		{
			_itemValueCounter.SetActive(true);
			while (_displayTimeLeft > 0f)
			{
				float displayTimeLeft = _displayTimeLeft;
				_displayTimeLeft = 0f;
				yield return (object)new WaitForSeconds(displayTimeLeft);
			}
			_itemValueCounter.SetActive(false);
		}

		private static void LoadValueCounter()
        {
			GameObject obj = GameObject.Find("/Systems/UI/Canvas/IngamePlayerHUD/BottomMiddle/ValueCounter");
			if (!obj)
			{
				ItemValue.getLogger().LogError("Failed to load ValueCounter object!");
			}
			_itemValueCounter = Object.Instantiate(obj.gameObject, obj.transform.parent, false);
			_itemValueCounter.name = "ItemValue UI";
			_itemValueCounter.GetComponentInChildren<Image>().enabled = false;

			//_itemValueCounter.transform.Translate(0f, 1f, 0f);

			_textValueCounter = _itemValueCounter.GetComponentInChildren<TextMeshProUGUI>();
			_textValueCounter.transform.localPosition = new Vector3(30f, -75f, 0f);
			_textValueCounter.transform.localRotation = new Quaternion(0f, 0f, 0.0175f, 0.9998f);
			_textValueCounter.text = $"<align=\"left\">ITEMS VALUE : $???";
		}

		private static float CalculateHeldValue()
		{
			float value = 0;

			PlayerControllerB player = GameNetworkManager.Instance.localPlayerController.GetComponentInChildren<PlayerControllerB>();

			if (player == null)
            {
				ItemValue.getLogger().LogDebug("Thats strange!!! No Player???");
			}
			ItemValue.getLogger().LogDebug("Player Name : " + player.playerUsername);

			foreach (GrabbableObject item in player.ItemSlots)
            {
				if (item == null) { continue; }

				if (item.itemProperties.name == "ClipboardManual" || item.itemProperties.name == "StickyNoteItem") {
					continue;
                }

				ItemValue.getLogger().LogDebug("Scrap Name: " + item.itemProperties.name);
				ItemValue.getLogger().LogDebug("Scrap Value: " + item.scrapValue);
				
				value += item.scrapValue;
            }

			return value;
		}



	}
}


//if (Object.FindObjectOfType<DepositItemsDesk>() != null)
//{
//	DepositItemsDesk depositItemsDesk = Object.FindObjectOfType<DepositItemsDesk>();

//	int num = 0;

//	for (int i = 0; i < depositItemsDesk.itemsOnCounter.Count; i++)
//	{
//		if (!depositItemsDesk.itemsOnCounter[i].itemProperties.isScrap)
//		{
//			if (!depositItemsDesk.itemsOnCounter[i].itemUsedUp)
//			{
//			}
//		}
//		else
//		{
//			num += depositItemsDesk.itemsOnCounter[i].scrapValue;
//		}
//	}
//	num = (int)((float)num * StartOfRound.Instance.companyBuyingRate);




//}