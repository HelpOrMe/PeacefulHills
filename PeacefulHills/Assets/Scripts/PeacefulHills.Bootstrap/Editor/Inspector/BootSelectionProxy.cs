using System;
using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Bootstrap.Editor
{
    /// <summary>
    /// Special scriptable object that stores information about the selected bootstrap
    /// in the <see cref="BootstrapWindow"/> and draws information about it when it is open in the inspector.
    /// </summary>
    [Serializable]
    public class BootSelectionProxy : ScriptableObject, ISerializationCallbackReceiver
    {        
        public Boot Boot { get; private set; }

        [SerializeField] private string bootTypeName;
        
        public static void Select(Boot boot)
        {
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Select Boot");
            CreateInstance(boot).Select();
        }
        
        public static BootSelectionProxy CreateInstance(Boot boot)
        {
            var proxy = CreateInstance<BootSelectionProxy>();
            proxy.hideFlags = HideFlags.HideAndDontSave;
            proxy.Initialize(boot);

            Undo.RegisterCreatedObjectUndo(proxy, "Create EntitySelectionProxy");
            return proxy;
        }

        private void Initialize(Boot boot)
        {
            Boot = boot;
        }

        public void Select()
        {
            // Don not reselect yourself
            if (Selection.activeObject == this)
                return;

            // Don not reselect the same boot
            if (Selection.activeObject is BootSelectionProxy selectionProxy && selectionProxy.Boot == Boot)
                return;

            Selection.activeObject = this;
        }

        public void OnBeforeSerialize()
        {
            bootTypeName = Boot?.GetType().AssemblyQualifiedName;
        }

        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(bootTypeName))
            {
                var type = Type.GetType(bootTypeName);
                if (type != null)
                {
                    Boot = (Boot)Activator.CreateInstance(Type.GetType(bootTypeName)!);
                }
            }
        }
    }
}