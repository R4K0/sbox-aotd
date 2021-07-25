using AOTD.PlayerRelated;
using AOTD.UI.Bars;
using Sandbox;
using Sandbox.UI;

namespace AOTD.UI
{
	[UseTemplate("/UI/StatsContainer.html")]
	public class StatsContainer : Panel
	{
		public Label JobLabel { get; set; }
		public override void Tick()
		{
			base.Tick();

			var desiredString = Local.Client?.Pawn is DealPlayer player ? player.GetTeam().ToString() : "Error";
			
			JobLabel?.SetText( $"💼 {desiredString}" );
		}
	}
}
