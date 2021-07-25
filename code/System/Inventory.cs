using System.Collections;
using System.Collections.Generic;
using Sandbox;
using NotImplementedException = System.NotImplementedException;

namespace aotd.System
{
	public abstract partial class InventoryItem : NetworkComponent
	{
		public virtual string UniqueID => "";
		public virtual void OnUse() {}
		public virtual void CanUse( Client player ) {}
	}
	public partial class InventorySlot : NetworkComponent
	{
		[Net] public Inventory OwningInventory { get; set; }

		[Net] public InventoryItem Item { get; set; }

		[Net] public int Count { get; set; }
	}
	public partial class Inventory : Entity, IEnumerable<InventorySlot>
	{
		[Net, Local] private List<InventorySlot> Items { get; set; }
		public IEnumerator<InventorySlot> GetEnumerator()
		{
			return Items.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
