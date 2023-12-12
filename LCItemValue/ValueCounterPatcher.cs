using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCItemValue.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class ValueCounterPatcher
	{
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void countItemValuePatch(ref GrabbableObject[] ___ItemSlots)
        {
            int heldValue = 0;
            foreach (GrabbableObject item in ___ItemSlots)
            {
                heldValue += item.scrapValue;
            }

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