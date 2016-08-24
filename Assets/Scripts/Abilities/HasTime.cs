using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class HasTime : MonoBehaviour {

    public Image clock;

    public int timeSlices;

    private const int MAX_SLICES = 12;

    void Awake() {
        timeSlices = UnityEngine.Random.Range(6, 12);
    }

    void Update() {
        clock.fillAmount = Mathf.Lerp(clock.fillAmount, timeSlices / 12.0f, .2f);
    }
}
