namespace aotd.System
{
	public interface IInventory
	{
		void AddItem( InventoryItem item );
		void RemoveItem( InventoryItem item );
		void RemoveItemCount( InventoryItem item, int count = 2 );
		bool HasItem( InventoryItem item );
		bool HasItemCount( InventoryItem item, int count = 2 );
		void SwapSlots( int x, int y, int anotherX, int anotherY );
		void SwapSlots( InventorySlot slot, InventorySlot anotherSlot );
		InventorySlot GetSlot( int x, int y );
		
		(int, int) GetSize();
		int GetTotalSize()
		{
			var (x, y) = GetSize();

			return x * y;
		}
		void SetSize( int x, int y );
	}
}
