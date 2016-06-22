using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class OutputNode : BaseNode {

    private string result = "";

    private BaseInputNode inputNode;
    private Rect inputNodeRect;

    public OutputNode() {
        windowTitle = "Output Node";
        hasInputs = true;
    }

    public override void DrawWindow() {
        // Draw the original class
        base.DrawWindow();
        // Keep track of events
        Event e = Event.current;
        // Use this to store input name
        string input1Title = "None";
        // If we do have a connection then set the title
        if(inputNode) { input1Title = inputNode.getResult(); }
        // Draw the Title
        GUILayout.Label("Input 1: " + input1Title);
        // Store the last frame
        if(e.type == EventType.Repaint) { inputNodeRect = GUILayoutUtility.GetLastRect(); }
        // Draw the results
        GUILayout.Label("Result: " + result);
    }

    public override void DrawCurves() {
        if(inputNode) {
            Rect rect = windowRect;
            rect.x += inputNodeRect.x;
            rect.y += inputNodeRect.y + inputNodeRect.height / 2;
            rect.width = 1;
            rect.height = 1;

            NodeEditor.DrawNodeCurve(inputNode.windowRect, rect);
        }
    }

    public override void NodeDeleted(BaseNode node) {
        if(node.Equals(inputNode)) {
            inputNode = null;
        }
    }

    public override BaseInputNode ClickedOnInput(Vector2 pos) {
        BaseInputNode retVal = null;

        pos.x -= windowRect.x;
        pos.y -= windowRect.y;

        if(inputNodeRect.Contains(pos)) {
            retVal = inputNode;
            inputNode = null;
        }

        return retVal;
    }

    public override void SetInput(BaseInputNode input, Vector2 clickPos) {
        clickPos.x -= windowRect.x;
        clickPos.y -= windowRect.y;

        if(inputNodeRect.Contains(clickPos)) { inputNode = input; }
    }
}
