using UnityEngine;

namespace Tools
{
    public abstract class Tool : MonoBehaviour
    {
        public string toolName; // Name of the tool for identification

        // Called when the tool is selected
        public virtual void OnSelect()
        {
            Debug.Log($"{toolName} selected.");
        }

        // Called when the tool is deselected
        public virtual void OnDeselect()
        {
            Debug.Log($"{toolName} deselected.");
        }

        // Called when the tool is used
        public abstract void Use();

        // Optional: Called every frame when the tool is active
        public virtual void UpdateTool()
        {
            // Override if needed
        }
    }
}