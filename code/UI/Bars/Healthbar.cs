using Sandbox;

namespace AOTD.UI.Bars
{
	public partial class Healthbar : Basebar
	{
		public override float UpdateValue => Local.Client?.Pawn?.Health / 100 ?? 0;
		public override string Name => "Health";
	}
}
