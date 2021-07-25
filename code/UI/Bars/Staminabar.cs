using AOTD.PlayerRelated;
using Sandbox;

namespace AOTD.UI.Bars
{
	public partial class Staminabar : Basebar
	{
		public override float UpdateValue => DealPlayer.LocalPlayer?.GetStamina() / 100 ?? 0f;
		public override Color BarColor => Color.Orange;

		public override void UpdateLayout()
		{
			if ( !DealPlayer.LocalPlayer?.CanSprint() ?? false )
			{
				Forebar.Style.BackgroundColor = Color.Gray;
			}
		}

		public override string Name => "Stamina";
	}
}
