using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using AOTD.PlayerRelated;
using AOTD.PlayerRelated;

namespace AOTD.PlayerRelated
{
	public partial class DealPlayer : Player
	{
		[Net]
		public ClothingManager Clothes { get; set; }
		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			Clothes?.Delete();
			
			Clothes = new ClothingManager()
			{
				Owner = this
			};
			
			Clothes.SetupSlots();
			
			TeamManager.Singleton.ApplyTeamClothingToPlayer( this );
			
			//
			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			//
			Controller = new AOTDWalkController()
			{
				WalkSpeed = 55f
			};

			//
			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			//
			Animator = new StandardPlayerAnimator();

			//
			// Use ThirdPersonCamera (you can make your own Camera for 100% control)
			//
			Camera = new ThirdPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
		}
		
		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			//
			// If you have active children (like a weapon etc) you should call this to 
			// simulate those too.
			//
			SimulateActiveChild( cl, ActiveChild );
		}

		public float GetStamina()
		{
			return (Controller as AOTDWalkController)?.Stamina ?? -1;
		}

		public bool IsSprinting()
		{
			return (Controller as AOTDWalkController)?.IsSprinting ?? false;
		}

		public bool CanSprint()
		{
			return (Controller as AOTDWalkController)?.CanSprint() ?? false;
		}
		
		public override void OnKilled()
		{
			base.OnKilled();

			EnableDrawing = false;
		}
		
		[Net] private Team Team { get; set; } = Team.Civilian;
		
		/// <summary>
		/// Sets the <see cref="AOTD.PlayerRelated.Team"/> of the Player.
		/// Team is automatically networked.
		/// </summary>
		/// <param name="team">Which team to set the player to</param>
		/// <param name="applyClothing">Should we reapply their clothing?</param>
		public void SetTeam( Team team, bool applyClothing = false )
		{
			Team = team;

			if ( applyClothing )
			{
				TeamManager.Singleton?.ApplyTeamClothingToPlayer( this );
			}
		}

		/// <summary>
		/// Gets the team.
		/// </summary>
		/// <returns>The <see cref="AOTD.PlayerRelated.Team"/>.</returns>
		public Team GetTeam()
		{
			return Team;
		}
	}
}
