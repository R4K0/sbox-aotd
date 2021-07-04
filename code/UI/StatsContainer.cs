using AOTD.UI.Bars;
using Sandbox;
using Sandbox.UI;

namespace AOTD.UI
{
	public class StatsContainer : Panel
	{
		public StatsContainer() => CreateLayout();

		[Event.Hotload]
		private void CreateLayout()
		{
			DeleteChildren( true );
			
			StyleSheet.Load( "UI/StatsContainer.scss" );

			AddChild<Healthbar>();
			AddChild<Staminabar>();
		}
	}
}
