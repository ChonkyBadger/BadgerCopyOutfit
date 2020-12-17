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
            EventHandlers["BadgerCopyOutfit:SetOutfit"] += new Action<IList<dynamic>, IList<dynamic>>(SetOutfit);

            TriggerEvent("chat:addSuggestion", "/copyoutfit", "Copies the outfit of another player", new[]
            {
                new { name = "PlayerID", help = "ID of the player whose outfit you want to copy" }
            });
        }

        // Get current outfit and send to server
        private void GetOutfit(int playerToGiveOutfit)
		{
            int ped = PlayerPedId();
            IList<dynamic> clothes = new List<dynamic>();
            IList<dynamic> props = new List<dynamic>();

            // Get outfit clothes
            for (int i = 1; i <= 11; i++)
            {
                var pedClothes = new
                {
                    Drawable = GetPedDrawableVariation(ped, i),
                    Texture = GetPedTextureVariation(ped, i),
                    Palette = GetPedPaletteVariation(ped, i)
                };

                clothes.Add(pedClothes);
            }

            // Get outfit props
            for (int i = 0; i <= 7; i++)
            {
                var pedProps = new
                {
                    Index = GetPedPropIndex(ped, i),
                    Texture = GetPedPropTextureIndex(ped, i),
                };

                props.Add(pedProps);
            }

            // Send outfit back to server
            TriggerServerEvent("BadgerCopyOutfit:SendOutfitToServer", clothes, props, playerToGiveOutfit);
        }

        // Set outfit
        private void SetOutfit(IList<dynamic> clothes, IList<dynamic> props)
        {
            int ped = PlayerPedId();

            // Set Clothes
            for (int i = 1; i <= 11; i++)
            {
                SetPedComponentVariation(ped, i, clothes[i - 1].Drawable, clothes[i - 1].Texture, clothes[i - 1].Palette);
            }

            // Set Props
            for (int i = 1; i <= 7; i++)
            {
                SetPedPropIndex(ped, i, props[i].Index, props[i].Texture, false); // Hat
            }
            Screen.ShowNotification("~y~[BadgerCopyOutfit]\n~w~Outfit successfully copied");
		}
    }
}
