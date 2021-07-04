using System;
using Sandbox;
using Sandbox.UI;

namespace AOTD.UI.Bars
{
	public abstract class Basebar : Panel
	{
		private Label Label;
		public virtual float UpdateValue => 0f;
		public virtual Color BarColor => "#8b0000";

		public virtual string Name => "";

		public Basebar() => CreateLayout();

		public Panel Forebar;

		[Event.Hotload]
		private void CreateLayout()
		{
			DeleteChildren( true );

			StyleSheet.Load( "UI/Bars/Bars.scss" );
			AddClass( "ValueBar" );

			Forebar = AddChild<Panel>( "Forebar" );

			if ( !Name.Equals( "" ) )
			{
				Label = AddChild<Label>();

				Label.SetText( Name );
			}

			Tick();
		}

		public virtual void UpdateLayout()
		{
		}

		private float LerpedValue = 0f;
		public override void Tick()
		{
			base.Tick();

			Label?.SetText( $"{Name} ({(Math.Abs( UpdateValue )):P0})" );


			LerpedValue = LerpedValue.LerpTo( UpdateValue, Time.Delta * 3 );
			
			Forebar.Style.BackgroundColor = BarColor;
			Forebar.Style.Width = Length.Fraction( LerpedValue );
			
			// This will call the UpdateLayout - In base class it does nothing, but deriving elements might want to override stuff.
			// For an example, see Staminabar
			UpdateLayout();
			
			Style.Dirty();
			Forebar.Style.Dirty();
		}
	}
}
