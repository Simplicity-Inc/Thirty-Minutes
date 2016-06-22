using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class InputNode : BaseInputNode {

    private InputType inputType;
    public enum InputType {
        Number,
        Randomization
    }

    private string randomFrom = "";
    private string randomTo = "";

    private string inputValue = "";

    public InputNode() {
        windowTitle = "Input Node";
    }

    public override void DrawWindow() {
        base.DrawWindow();

        inputType = (InputType)EditorGUILayout.EnumPopup("Input Type: ", inputType);

        if(inputType == InputType.Number) {
            inputValue = EditorGUILayout.TextField("Value:", inputValue);
        } else if(inputType == InputType.Randomization) {
            randomFrom = EditorGUILayout.TextField("Value:", randomFrom);
            randomTo = EditorGUILayout.TextField("Value:", randomTo);

            if(GUILayout.Button("CalculateRandom")) {
                calculateRandom();
            }
        }
    }

    private void calculateRandom() {
        float rFrom = 0;
        float rTo = 0;

        float.TryParse(randomFrom, out rFrom);
        float.TryParse(randomTo, out rTo);

        int randFrom = (int)( rFrom * 10 );
        int randTo = (int)( rFrom * 10 );

        int selected = UnityEngine.Random.Range(randFrom, randTo + 1);

        float selectedValue = selected / 10;
        inputValue = selectedValue.ToString();
    }

    public override string getResult() {
        return inputValue.ToString();
    }

    public override void DrawCurves() {

    }
}
