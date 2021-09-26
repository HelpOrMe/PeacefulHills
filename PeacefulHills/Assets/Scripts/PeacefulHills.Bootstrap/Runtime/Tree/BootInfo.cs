using System;
using System.Collections.Generic;
using System.Linq;

namespace PeacefulHills.Bootstrap.Tree
{
    public readonly struct BootInfo
    {
        public readonly Type Type; 
        public readonly IBoot Operand;
        public readonly IBootControl[] Controls; 
        
        public BootInfo(Type type) : this()
        {
            if (!typeof(IBoot).IsAssignableFrom(type))
            {
                throw new ArgumentException($"Unable to build BootstrapType from '{type.FullName}'.");
            }

            Type = type;
            Operand = (IBoot) Activator.CreateInstance(type);
            Controls = GatherControls(type);
        }

        private IBootControl[] GatherControls(Type type)
        {
            var controls = new List<IBootControl>();
            
            foreach (object attribute in type.GetCustomAttributes(inherit: true))
            {
                if (attribute is IBootControl attrControl)
                {
                    controls.Add(attrControl);
                }
            }

            if (Operand is IBootControl control)
            {
                controls.Add(control);
            }

            return controls.ToArray();
        }
        
        public bool HasControl<TControl>() where TControl : IBootControl
        {
            return Controls.Any(c => c is TControl);
        }

        public TControl GetControl<TControl>() where TControl : IBootControl
        {
            return (TControl) Controls.FirstOrDefault(c => c is TControl);
        }
        
        public bool TryGetControl<TControl>(out TControl control) where TControl : IBootControl
        {
            control = (TControl) Controls.FirstOrDefault(c => c is TControl);
            return control != null;
        }

        public void RequestControl<TControl>(Action<TControl> controlHandle) where TControl : IBootControl
        {
            var control = (TControl) Controls.FirstOrDefault(c => c is TControl);

            if (control != null)
            {
                controlHandle(control);
            }
        }
    }
}