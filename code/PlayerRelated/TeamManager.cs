using System;
using System.Collections.Generic;
using AOTD.PlayerRelated;
using Sandbox;

namespace AOTD.PlayerRelated
{
	/// <summary>
	/// The teams of the gamemode.
	///
	/// Civilian is the default one.
	/// </summary>
	public enum Team
	{
		Unemployed,
		Rednikov,
		MonogoIndurstries
	}

	public class TeamManager : Entity
	{
		public static TeamManager Singleton;
		private Dictionary<Team, List<ClothingItem>> TeamClothing = new();

		public TeamManager()
		{
			if ( IsServer )
				Singleton?.Delete();

			Singleton = this;

			OnCreate();
		}

		[Event.Hotload]
		private void OnCreate()
		{
			TeamClothing ??= new Dictionary<Team, List<ClothingItem>>();

			TeamClothing.Clear();
			foreach ( Team enumeration in Enum.GetValues( typeof(Team) ) )
			{
				TeamClothing.Add( enumeration, new List<ClothingItem>() );
			}

			var smartTrousers =
				new ClothingItem( ClothingType.Legs, "models/citizen_clothes/trousers/trousers.smart.vmdl" );

			AddTeamClothing( Team.Unemployed,
				new[]
				{
					new ClothingItem( ClothingType.Legs, "models/citizen_clothes/trousers/trousers.jeans.vmdl" ),
					new ClothingItem( ClothingType.Chest, "models/citizen_clothes/jacket/jacket.red.vmdl", Color.White, TerryBodygroup.Chest ),
					new ClothingItem( ClothingType.Feet, "models/citizen_clothes/shoes/shoes_securityboots.vmdl" ),
					new ClothingItem( ClothingType.Head, "models/citizen_clothes/hat/hat_woolly.vmdl" )
				} );

			AddTeamClothing( Team.MonogoIndurstries, new[]
			{
				smartTrousers
			} );

			AddTeamClothing( Team.Rednikov, new[]
			{
				smartTrousers
			} );
		}

		/// <summary>
		/// Adds <see cref="ClothingItem"/>s to the Team definition, so that players spawn with Team-related clothes.
		/// </summary>
		/// <param name="team">Which team do those clothes belong to</param>
		/// <param name="clothingItem">A IEnumerable of <see cref="ClothingItem"/>s</param>
		///
		/// <example>
		/// <code>
		/// AddTeamClothing( Team.Civilian, new [] {
		///		new ClothingItem( ClothingType.Legs, "examplemodel" );
		/// } )
		/// </code>
		/// </example>
		public void AddTeamClothing( Team team, IEnumerable<ClothingItem> clothingItem )
		{
			TeamClothing.TryGetValue( team, out var clothingItems );

			clothingItems?.AddRange( clothingItem );
		}

		public void ApplyTeamClothingToPlayer( DealPlayer player )
		{
			var clothingItems = GetTeamClothes( player.GetTeam() );

			player.Clothes.ClearSlots();
			foreach ( var clothingItem in clothingItems )
			{
				player.Clothes.Wear( clothingItem );
			}
		}

		public List<ClothingItem> GetTeamClothes( Team team )
		{
			return TeamClothing.TryGetValue( team, out var foundTeam ) ? foundTeam : null;
		}
	}
}
