using System.Collections.Generic;
using AOTD.PlayerRelated;
using Sandbox;

namespace aotd.System
{
	/// <summary>
	/// This entity acts as our spawn point. It's an entity, not a model entity, because it doesn't have a physical manifestation.
	/// </summary>
	[Library("aotd_spawnpoint", Title = "Team Spawnpoint", Description = "This is where your players will spawn", Group = "Art Of The Deal")]
	[Hammer.EditorModel("models/editor/playerstart.vmdl")]
	public partial class Spawnpoint : Entity
	{
		public static List<Spawnpoint> Spawnpoints = new();
		
		/// <summary>
		/// Which team does this spawn point belong to?
		/// </summary>
		[Net]
		[Property(Title = "Team Ownership")]
		public Team OwningTeam { get; set; }

		/// <summary>
		/// The priority in which the spawn points are going to be used. If one is obstructed, then the next one will be tested.
		/// </summary>
		[Property( Title = "Priority" )]
		public int Priority { get; set; } = 1;

		public Output PostSpawnOutput = new();

		public Spawnpoint()
		{
			Transmit = TransmitType.Always;
		}

		/// <summary>
		/// Checks whenever the spawnpoint is clear for this player
		/// </summary>
		/// <param name="player">The player we're trying to test for</param>
		/// <returns>True if obstructed, false if not</returns>
		public bool IsObstructed( DealPlayer player )
		{
			var traceResult = Trace.Ray( Position, Position )
				.Size( player.CollisionBounds )
				.HitLayer( CollisionLayer.All, false )
				.HitLayer( CollisionLayer.WORLD_GEOMETRY, false )
				.HitLayer( CollisionLayer.Player )
				.Ignore( this )
				.Ignore( player )
				.Run();

			return traceResult.Hit;
		}

		public virtual void PostSpawn( Entity player )
		{
			FireOutput( "PostSpawnOutput", player );
		}
		
		protected override void OnDestroy()
		{
			Spawnpoints.Remove( this );
		}

		public override void Spawn()
		{
			AddToGlobal();
		}
		
		public override void ClientSpawn()
		{
			AddToGlobal();
		}
		
		private void AddToGlobal()
		{
			Spawnpoints.Add( this );
		}
	}
}
