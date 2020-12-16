using CitizenFX.Core;
using CitizenFX.Core.UI;
using System;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;

namespace BadgerClient
{
    public class BadgerClient : BaseScript
    {
        public BadgerClient()
		{
            EventHandlers["BadgerCopyOutfit:GetOutfit"] += new Action<int>(GetOutfit);
            EventHandlers["BadgerCopyOutfit:SetOutfit"] += new Action<IList<dynamic>>(SetOutfit);

            TriggerEvent("chat:addSuggestion", "/copyoutfit", "Copies the outfit of another player", new[]
            {
                new { name = "PlayerID", help = "ID of the player whose outfit you want to copy" }
            });
        }

        // Get current outfit and send to server
        private void GetOutfit(int playerToGiveOutfit)
		{
            int ped = PlayerPedId();
            IList<dynamic> outfit = new List<dynamic>();

            // Get outfit components
            for (int i = 1; i <= 11; i++)
            {
                int component = GetPedDrawableVariation(ped, i);
                outfit.Add(component);
            }

            // Send outfit back to server
            TriggerServerEvent("BadgerCopyOutfit:SendOutfitToServer", outfit, playerToGiveOutfit);
        }

        // Set outfit
        private void SetOutfit(IList<dynamic> outfit)
		{
            int ped = PlayerPedId();

            for (int i = 1; i <= 11; i++)
			{
                SetPedComponentVariation(ped, i, outfit[i-1], 1, 0);
			}

            Screen.ShowNotification("~y~[BadgerCopyOutfit]\n~w~Outfit successfully copied");
		}
    }
}
