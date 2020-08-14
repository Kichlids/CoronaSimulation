using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private SimManager sim;

    public Slider dayCompletionMeter;

    private void Start() {
        sim = SimManager._sim;
    }

    private void Update() {
        UpdateTimeOfDay();
    }

    public void UpdateTimeOfDay() {
        dayCompletionMeter.value = sim.inGameTime / sim.secondsInDay;
    }
}
