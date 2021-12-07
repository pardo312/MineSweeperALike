using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public static class HierarchyIcons
{
    const string IgnoreIcons = "GameObject Icon, Prefab Icon";

    static HierarchyIcons()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var content = EditorGUIUtility.ObjectContent(EditorUtility.InstanceIDToObject(instanceID), null);

        if (!content.text.Contains("----") && content.image != null )
            GUI.DrawTexture(new Rect(selectionRect.xMin , selectionRect.yMin, 16, 16), content.image);

    }
}
#endif 
