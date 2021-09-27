using System;
using PeacefulHills.Bootstrap.Tree;
using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Bootstrap.Editor
{
    [Serializable]
    public class BootSelectionProxy : ScriptableObject, ISerializationCallbackReceiver
    {        
        public BootInfo Boot { get; private set; }

        [SerializeField] private string bootTypeName;
        
        public static void Select(BootInfo boot)
        {
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Select Boot");
            CreateInstance(boot).Select();
        }
        
        public static BootSelectionProxy CreateInstance(BootInfo boot)
        {
            var proxy = CreateInstance<BootSelectionProxy>();
            proxy.hideFlags = HideFlags.HideAndDontSave;
            proxy.Initialize(boot);

            Undo.RegisterCreatedObjectUndo(proxy, "Create EntitySelectionProxy");
            return proxy;
        }

        private void Initialize(BootInfo boot)
        {
            Boot = boot;
        }

        public void Select()
        {
            // Don not reselect yourself
            if (Selection.activeObject == this)
                return;

            // Don not reselect the same boot
            if (Selection.activeObject is BootSelectionProxy selectionProxy && selectionProxy.Boot.Type == Boot.Type)
                return;

            Selection.activeObject = this;
        }

        public void OnBeforeSerialize()
        {
            bootTypeName = Boot.Type?.AssemblyQualifiedName;
        }

        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(bootTypeName))
            {
                Boot = new BootInfo(Type.GetType(bootTypeName));
            }
        }
    }
}