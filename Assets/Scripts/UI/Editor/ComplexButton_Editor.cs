namespace UnityEngine.UI
{
    [UnityEditor.CustomEditor(typeof(ComplexButton))]
    public class ComplexButton_Editor : UnityEditor.UI.ButtonEditor 
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawDefaultInspector();
        }
    }
}

