using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace AOTD.PlayerRelated
{
	public partial class ClothingItem : Entity
	{
		[Net] public ClothingType Type { get; set; }
		[Net] public string Model { get; set; }
		[Net] public Color Tint { get; set; }
		[Net] public List<TerryBodygroup> HiddenBodygroups { get; set; }

		public ClothingItem( ClothingType type, string model, Color tint = default, params TerryBodygroup[] hiddenBodygroups )
		{
			Type = type;
			Model = model;
			Tint = tint;
			HiddenBodygroups = hiddenBodygroups.ToList();
		}
	}

	public enum TerryBodygroup
	{
		Head,
		Chest,
		Legs,
		Hands,
		Feet
	}

	public enum ClothingType
	{
		Head,
		Chest,
		Legs,
		Feet
	}

	public partial class ClothingManager : Entity, IEnumerable<ClothingSlot>
	{
		[Net] public IList<ClothingSlot> _ClothingSlots { get; set; }
		public List<ClothingSlot> ClothingSlots => (List<ClothingSlot>)_ClothingSlots;

		public virtual void SetupSlots()
		{
			_ClothingSlots ??= new List<ClothingSlot>();
			_ClothingSlots.Clear();

			for ( var i = 0; i < (int)ClothingType.Feet; i++ )
			{
				_ClothingSlots.Add( new ClothingSlot() {Type = (ClothingType)i} );
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

		public void Wear( ClothingItem item )
		{
			if ( !IsServer )
				return;

			var slot = GetSlotOfType( item.Type );

			if ( slot is null )
			{
				return;
			}

			slot.Entity?.Delete();

			var modelEntity = new ModelEntity();
			modelEntity.SetModel( item.Model );
			modelEntity.SetParent( Owner, true );

			slot.Entity = modelEntity;
			slot.Item = item;

			var hideBodygroups = item.HiddenBodygroups;
			if ( hideBodygroups is not null && Owner is DealPlayer and ModelEntity ownerModel )
			{
				foreach (var bodygroupIndex in hideBodygroups)
				{
					ownerModel.SetBodyGroup( (int) bodygroupIndex, 1 );
				}
			}
		}

		public void Undress( ClothingType type )
		{
			var slotOfType = GetSlotOfType( type );

			if ( slotOfType is null || !slotOfType.IsPopulated() )
			{
				return;
			}

			var hideBodygroups = slotOfType.Item?.HiddenBodygroups;

			if ( hideBodygroups is not null && Owner is DealPlayer and ModelEntity ownerModel  )
			{
				foreach (var terryBodygroup in hideBodygroups)
				{
					ownerModel.SetBodyGroup( (int) terryBodygroup, 0 );
				}
			}

			slotOfType.Entity?.Delete();
			slotOfType.Item = null;
		}
		
		protected override void OnDestroy()
		{
			base.OnDestroy();

			Log.Info( "Cleaning up a Clothing Manager!" );
			ClearSlots();
		}

		public void ClearSlots()
		{
			foreach (var clothingSlot in this)
			{
				Undress(  clothingSlot.Type );
			}
		}

		public IEnumerator<ClothingSlot> GetEnumerator()
		{
			return _ClothingSlots.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _ClothingSlots.GetEnumerator();
		}
	}

	public partial class ClothingSlot : Entity
	{
		[Net] public ClothingItem Item { get; set; }
		[Net] public ModelEntity Entity { get; set; }
		[Net] public ClothingType Type { get; set; } 

		public bool IsPopulated()
		{
			return Item is not null;
		}
	}
}
