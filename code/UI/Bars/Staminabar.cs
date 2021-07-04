using Sandbox;

namespace AOTD.UI.Bars
{
	public class Staminabar : Basebar
	{
		public override float UpdateValue => (Local.Client?.Pawn as DealPlayer)?.GetStamina() / 100 ?? 0f;
		public override Color BarColor => Color.Orange;

		public override void UpdateLayout()
		{
			if ( !(Local.Client?.Pawn as DealPlayer)?.CanSprint() ?? false )
			{
				Forebar.Style.BackgroundColor = Color.Gray;
			}
		}

		public override string Name => "Stamina";
	}
}
