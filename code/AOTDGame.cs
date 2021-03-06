
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AOTD.PlayerRelated;
using aotd.System;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace AOTD
{

	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// 
	/// Your game needs to be registered (using [Library] here) with the same name 
	/// as your game addon. If it isn't then we won't be able to find it.
	/// </summary>
	public partial class AOTDGame : Sandbox.Game
	{
		public static AOTDGame GetGame()
		{
			return Current as AOTDGame;
		}

		public AOTDGame()
		{
			if ( IsServer )
			{
				// Create a HUD entity. This entity is globally networked
				// and when it is created clientside it creates the actual
				// UI panels. You don't have to create your HUD via an entity,
				// this just feels like a nice neat way to do it.
				new HudEntity();
				new TeamManager();
			}

			if ( IsClient )
			{
				Log.Info( "My Gamemode Has Created Clientside!" );
			}
		}
		
		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new DealPlayer();
			client.Pawn = player;

			player.Respawn();
		}

		public override void MoveToSpawnpoint( Entity pawn )
		{
			// If pawn is a DealPlayer, proceed with custom logic
			if ( pawn is DealPlayer player )
			{
				var sorted = Spawnpoint.Spawnpoints.OrderBy( spawnpoint => spawnpoint.Priority )
					.Where( spawnpoint => spawnpoint.OwningTeam == player.GetTeam() )
					.ToList();
				
				// Tries to find the first unobstructed spawn point, if it can't find it - it'll pick the spawn by priority.
				var found = sorted.FirstOrDefault( spawnpoint => !spawnpoint.IsObstructed( player ) ) ?? sorted.FirstOrDefault();

				if ( found is not null )
				{
					player.Transform = found.Transform;
					found.PostSpawn( pawn );

					// If the spawn was found, then don't proceed to default behavior.
					return;
				}
			}
			
			// If not found or pawn is not a DealPlayer, proceed with default behavior
			base.MoveToSpawnpoint( pawn );
		}
	}

}
