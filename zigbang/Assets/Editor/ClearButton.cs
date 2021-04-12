using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyFramework))]
public class ClearButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MyFramework generator = (MyFramework)target;
        if (GUILayout.Button("Clear Button"))
        {
            generator.Clear();
        }

        if (GUILayout.Button("CreateApartment Button"))
        {
            generator.StartCreateBuilding();
        }
    }
}
