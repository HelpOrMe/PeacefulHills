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

        public BootInfo(Type type, IBoot operand, IBootControl[] controls)
        {
            Type = type;
            Operand = operand;
            Controls = controls;
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

        public IEnumerable<Type> FindControlTypes()
        {
            foreach (IBootControl control in Controls)
            {
                foreach (Type controlInterface in control.GetType().GetInterfaces())
                {
                    if (typeof(IBootControl).IsAssignableFrom(controlInterface))
                    {
                        yield return controlInterface;
                    }
                }
            }
        }

        public bool HasControl<TControl>() where TControl : IBootControl
        {
            return Controls.Any(c => c is TControl);
        }

        public TControl GetControl<TControl>() where TControl : IBootControl
        {
            return (TControl) Controls.FirstOrDefault(c => c is TControl);
        }

        public void RequestControl<TControl>(Action<TControl> controlHandle) where TControl : IBootControl
        {
            if (TryGetControl(out TControl control))
            {
                controlHandle(control);
            }
        }

        public bool TryGetControl<TControl>(out TControl control) where TControl : IBootControl
        {
            control = (TControl) Controls.FirstOrDefault(c => c is TControl);
            return control != null;
        }

        public void RequestGenericControl<TControl>(Action<IBootControl> controlHandle) where TControl : IBootControl
        {
            if (TryGetGenericControl<TControl>(out IBootControl control))
            {
                controlHandle(control);
            }
        }

        public bool TryGetGenericControl<TControl>(out IBootControl control) where TControl : IBootControl
        {
            Type type = typeof(TControl).GetGenericTypeDefinition();
            
            control = Controls.FirstOrDefault(c => c
                .GetType()
                .GetInterfaces()
                .Any(i => i.IsGenericType 
                          && i.GetGenericTypeDefinition() == type));
            
            return control != null;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(BootInfo other)
        {
            return Type.Name == other.Type.Name;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Operand != null ? Operand.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Controls != null ? Controls.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}