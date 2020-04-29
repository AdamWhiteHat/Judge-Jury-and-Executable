using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePropertiesBaselineGUI
{
    public class Toggle
    {
        public bool IsActive { get; private set; }

        private Action ActivationBehavior = null;
        private Action DeactivationBehavior = null;

        public Toggle(Action activationBehavior, Action deactivationBehavior)
        {
            if (activationBehavior == null) throw new ArgumentNullException(nameof(activationBehavior));
            if (deactivationBehavior == null) throw new ArgumentNullException(nameof(deactivationBehavior));

            IsActive = false;
            ActivationBehavior = activationBehavior;
            DeactivationBehavior = deactivationBehavior;
        }

        public void SetState(bool value)
        {
            bool previousState = IsActive;
            IsActive = value;

            if (previousState != value)
            {
                OnToggle();
            }
        }

        private void OnToggle()
        {
            if (IsActive)
            {
                ActivationBehavior.Invoke();
            }
            else
            {
                DeactivationBehavior.Invoke();
            }
        }
    }
}
