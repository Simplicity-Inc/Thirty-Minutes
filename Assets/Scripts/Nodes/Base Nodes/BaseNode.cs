using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// This is the base node from which all nodes are formed
/// </summary>
public abstract class BaseNode : ScriptableObject {

    /// <summary>
    /// Size of the node's window in the editor
    /// </summary>
    public Rect windowRect;
    /// <summary>
    /// A bool for if the node can have connects to it
    /// </summary>
    public bool hasInputs = false;
    /// <summary>
    /// The title that displays at the node's window
    /// </summary>
    public string windowTitle = "";
    /// <summary>
    /// Draws the node window in the editor
    /// </summary>
    public virtual void DrawWindow() { windowTitle = EditorGUILayout.TextField("Title", windowTitle); }
    /// <summary>
    /// An abstract function for which the nodes draw connections to eachother
    /// </summary>
    public abstract void DrawCurves();
    /// <summary>
    /// Makes the connects to between nodes based on the input and were you add it
    /// </summary>
    /// <param name="input">The node that is being connected</param>
    /// <param name="clickPos">Where you are clicking in global space of the editor</param>
    public virtual void SetInput(BaseInputNode input, Vector2 clickPos) { }
    /// <summary>
    /// Deletes the node that is passed in
    /// </summary>
    /// <param name="node">The node that will be deleted</param>
    public virtual void NodeDeleted(BaseNode node) { }
    /// <summary>
    /// Detects if you clicked on the Input spot in the editor
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>Return null from the BaseNode class</returns>
    public virtual BaseInputNode ClickedOnInput(Vector2 pos) { return null; }

    //public abstract void Tick(float deltaTime);
}
