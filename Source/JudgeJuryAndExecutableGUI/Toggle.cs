using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JudgeJuryAndExecutableGUI
{
	public enum ToggleState
	{
		Ready,
		Active,
		Inactive
	}

	public class Toggle
	{
		public ToggleState CurrentState { get; private set; } = ToggleState.Inactive;

		private Action ActivationBehavior = null;
		private Action DeactivationBehavior = null;
		private Action ResetBehavior = null;

		public Toggle(Action activationBehavior, Action deactivationBehavior, Action resetBehavior)
		{
			if (activationBehavior == null) throw new ArgumentNullException(nameof(activationBehavior));
			if (deactivationBehavior == null) throw new ArgumentNullException(nameof(deactivationBehavior));

			ActivationBehavior = activationBehavior;
			DeactivationBehavior = deactivationBehavior;
			ResetBehavior = resetBehavior;

			CurrentState = ToggleState.Ready;
		}

		public void SetState(ToggleState to)
		{
			if (CurrentState == ToggleState.Ready)
			{
				if (to == ToggleState.Active)
				{
					CurrentState = ToggleState.Active;
					ActivationBehavior();
				}
			}
			else if (CurrentState == ToggleState.Active)
			{
				if (to == ToggleState.Inactive)
				{
					CurrentState = ToggleState.Inactive;
					DeactivationBehavior();
				}
				else if (to == ToggleState.Ready)
				{
					CurrentState = ToggleState.Ready;
					ResetBehavior();
				}
			}
			else if (CurrentState == ToggleState.Inactive)
			{
				if (to == ToggleState.Ready)
				{
					CurrentState = ToggleState.Ready;
					ResetBehavior();
				}
			}
			else
			{
				throw new Exception($"Unhandled {nameof(ToggleState)}: {Enum.GetName(typeof(ToggleState), CurrentState)}");
			}
		}
	}
}
