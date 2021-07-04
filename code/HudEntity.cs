using AOTD.UI;
using Sandbox;
using Sandbox.UI;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace AOTD
{
	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class HudEntity : HudEntity<RootPanel>
	{
		public Panel StaminaBar;
		public HudEntity()
		{
			if ( !IsClient )
				return; 
			
			Load();
		}
		public void Load()
		{
			RootPanel.AddChild<StatsContainer>();
		}
	}

}
