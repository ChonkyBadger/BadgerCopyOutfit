using CitizenFX.Core;
using System;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;

namespace BadgerServer
{
    public class BadgerServer : BaseScript
    {
        public BadgerServer()
		{
			// Get outfit from client
			EventHandlers["BadgerCopyOutfit:SendOutfitToServer"] += new Action<IList<dynamic>, int>(SendOutfit);

			RegisterCommand("copyOutfit", new Action<int, List<object>, string>((source, args, raw) =>
			{
				if (args.Count > 0)
				{
					if (IsPlayerAceAllowed(source.ToString(), "BadgerCopyOutfit.Command"))
					{
						Player targetPlayer = Players[int.Parse(args[0].ToString())];

						// Get outfit from target player
						TriggerClientEvent(targetPlayer, "BadgerCopyOutfit:GetOutfit", source);
					}
				}
				else
					Debug.WriteLine("BadgerCopyOutfit: No command args");
			}), false);
		}

		// Send outfit to client
		private void SendOutfit(IList<dynamic> outfit, int targetPlayer)
		{
			TriggerClientEvent(Players[targetPlayer], "BadgerCopyOutfit:SetOutfit", outfit);
		}
    }

}
