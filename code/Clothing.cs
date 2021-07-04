using System;
using System.Collections.Generic;
using Sandbox;

namespace AOTD
{
	public enum ClothingType
	{
		Head,
		Chest,
		Legs,
		Feet
	}

	public partial class ClothingManager : Entity
	{
		[Net] public IList<ClothingSlot> _ClothingSlots { get; set; }
		public List<ClothingSlot> ClothingSlots => ( List<ClothingSlot> ) _ClothingSlots;
		
		public virtual void SetupSlots()
		{
			_ClothingSlots ??= new List<ClothingSlot>();
			_ClothingSlots.Clear();

			for ( var i = 0; i < (int) ClothingType.Feet; i++ )
			{
				_ClothingSlots.Add( new ClothingSlot()
				{
					Type = (ClothingType) i
				} );
			}
		}
		private ClothingSlot GetSlotOfType( ClothingType type )
		{
			return ClothingSlots.Find( slot => slot.Type == type );
		}

		public bool IsSlotPopulated( ClothingType type )
		{
			return GetSlotOfType( type )?.IsPopulated() ?? false;
		}

		public void Wear( ClothingType type, string model )
		{
			if ( !IsServer )
				return;
			
			var slot = GetSlotOfType( type );

			if ( slot is not null )
			{
				slot.Entity?.Delete();
				
				var modelEntity = new ModelEntity();
				modelEntity.SetModel( model );
				modelEntity.SetParent( Owner, true );

				slot.Entity = modelEntity;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		
			Log.Info( "Cleaning up a Clothing Manager!" );
			if ( _ClothingSlots != null )
			{
				foreach ( var clothingSlot in _ClothingSlots )
				{
					clothingSlot.Entity?.Delete();
				}
			}
		}
	}
	
	public partial class ClothingSlot : Entity
	{
		[Net] public ClothingType Type { get; set; }
		[Net] public ModelEntity Entity { get; set; }

		public bool IsPopulated()
		{
			return Entity.IsValid();
		}
	}
}
