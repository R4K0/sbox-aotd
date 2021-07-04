using System;
using Sandbox;

namespace AOTD.PlayerRelated
{
	public partial class AOTDWalkController : WalkController
	{
		[Net, Predicted] public TimeSince ExhaustionTime { get; set; } = 15;
		public bool CanSprint()
		{
			return ExhaustionTime > 7f && Stamina > 0f;
		}
		
		public override float GetWishSpeed()
		{
			var ws = Duck.GetWishSpeed();
			if ( ws >= 0 ) return ws;

			if ( IsSprinting )
				return DefaultSpeed;
			
			return WalkSpeed;
		}

		[Net] public bool IsSprinting { get; set; } = false;
		public override void Simulate()
		{
			base.Simulate();
			SprintSimulate();
		}

		private void SprintSimulate()
		{
			IsSprinting = false;

			if (Stamina > 0f && Input.Down(InputButton.Run) && CanSprint() && GroundEntity is not null )
			{
				IsSprinting = true;
				Stamina -= 9f * Time.Delta;

				if (Stamina < 0f)
				{
					ExhaustionTime = 0;
				}
			}

			if ( (!Input.Down(InputButton.Run) || !CanSprint()) && ExhaustionTime >= 2f )
			{
				if ( 99f >= Stamina )
				{				
					Stamina = MathF.Min(100f, Stamina + 3.5f * Time.Delta);
				}
			}

			DebugOverlay.ScreenText(0, $"Stamina:\n{Stamina:n2} | Sprinting: {IsSprinting}\nLast Exhausted:\n{ExhaustionTime.Relative:n2} | Can Sprint: {CanSprint()}");
		}

		[Net, Predicted] public float Stamina { get; set; } = 100f;
	}
}
