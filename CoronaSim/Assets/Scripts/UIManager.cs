using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private SimManager sim;

    public Slider numPeopleSlr;
    public TextMeshProUGUI numPeopleTxt;
    [Space]
    public Slider infectionRateSlr;
    public TextMeshProUGUI infectionRateTxt;
    [Space]
    public Slider symptomaticInfectionRateSlr;
    public TextMeshProUGUI symptomaticInfectionRateTxt;
    [Space]
    public Slider asymptomaticInfectionRateSlr;
    public TextMeshProUGUI asymptomaticInfectionRateTxt;
    [Space]
    public Slider symptomaticIfrRateSlr;
    public TextMeshProUGUI symptomaticIfrTxt;
    [Space]
    public Slider asymptomaticRecoveryDurationSlr;
    public TextMeshProUGUI asymptomaticRecoveryDurationTxt;
    [Space]
    public Slider maskInfectionReductionRateSlr;
    public TextMeshProUGUI maskInfectionReductionRateTxt;
    [Space]
    public Slider maskUsagePercentageSlr;
    public TextMeshProUGUI maskUsagePercentageTxt;
    [Space]
    public Slider socialDistancingUsagePercentageSlr;
    public TextMeshProUGUI socialDistancingUsagePercentageTxt;
    

    public Slider dayCompletionMeter;

    private void Start() {
        sim = SimManager._sim;

        //SlidersInit();
        
    }

    private void Update() {
        UpdateTimeOfDay();
    }

    private void SlidersInit() {
        // Number of people present
        numPeopleSlr.minValue = 1;
        numPeopleSlr.maxValue = sim.maxNumPeople;
        numPeopleSlr.wholeNumbers = true;
        numPeopleSlr.value = numPeopleSlr.minValue;
        numPeopleTxt.text = numPeopleSlr.value.ToString();

        // Infection rate
        infectionRateSlr.minValue = 0;
        infectionRateSlr.maxValue = 1;
        infectionRateSlr.wholeNumbers = false;
        infectionRateSlr.value = infectionRateSlr.minValue;
        infectionRateTxt.text = infectionRateSlr.value.ToString();

        // Symptomatic infection rate
        symptomaticInfectionRateSlr.minValue = 0;
        symptomaticInfectionRateSlr.maxValue = 1;
        symptomaticInfectionRateSlr.wholeNumbers = false;
        symptomaticInfectionRateSlr.value = symptomaticInfectionRateSlr.minValue;
        symptomaticInfectionRateTxt.text = symptomaticInfectionRateSlr.value.ToString();

        // Asymptomatic infection rate
        asymptomaticInfectionRateSlr.minValue = 0;
        asymptomaticInfectionRateSlr.maxValue = 1;
        asymptomaticInfectionRateSlr.wholeNumbers = false;
        asymptomaticInfectionRateSlr.value = asymptomaticInfectionRateSlr.minValue;
        asymptomaticInfectionRateTxt.text = asymptomaticInfectionRateSlr.value.ToString();

        // Symptomatic IFR
        symptomaticIfrRateSlr.minValue = 0;
        symptomaticIfrRateSlr.maxValue = 1;
        symptomaticIfrRateSlr.wholeNumbers = false;
        symptomaticIfrRateSlr.value = symptomaticIfrRateSlr.minValue;
        symptomaticIfrTxt.text = symptomaticIfrRateSlr.value.ToString();

        // Asymptomatic Patient Recovery Time
        asymptomaticRecoveryDurationSlr.minValue = 3;
        asymptomaticRecoveryDurationSlr.maxValue = 12;
        asymptomaticRecoveryDurationSlr.wholeNumbers = true;
        asymptomaticRecoveryDurationSlr.value = asymptomaticRecoveryDurationSlr.minValue;
        asymptomaticRecoveryDurationTxt.text = asymptomaticRecoveryDurationSlr.value.ToString();

        // Mask Effectiveness
        maskInfectionReductionRateSlr.minValue = 0;
        maskInfectionReductionRateSlr.maxValue = 1;
        maskInfectionReductionRateSlr.wholeNumbers = false;
        maskInfectionReductionRateSlr.value = maskInfectionReductionRateSlr.minValue;
        maskInfectionReductionRateTxt.text = maskInfectionReductionRateSlr.value.ToString();

        // Mask Usage
        maskUsagePercentageSlr.minValue = 0;
        maskUsagePercentageSlr.maxValue = 1;
        maskUsagePercentageSlr.wholeNumbers = false;
        maskUsagePercentageSlr.value = maskUsagePercentageSlr.minValue;
        maskUsagePercentageTxt.text = maskInfectionReductionRateSlr.value.ToString();

        // Social Distancing
        socialDistancingUsagePercentageSlr.minValue = 0;
        socialDistancingUsagePercentageSlr.maxValue = 1;
        socialDistancingUsagePercentageSlr.wholeNumbers = false;
        socialDistancingUsagePercentageSlr.value = socialDistancingUsagePercentageSlr.minValue;
        socialDistancingUsagePercentageTxt.text = socialDistancingUsagePercentageSlr.value.ToString();

    }

    public void UpdateTimeOfDay() {
        dayCompletionMeter.value = sim.inGameTime / sim.secondsInDay;
    }

    public void OnNumberPeopleChanged() {
        sim.numPeople = Mathf.RoundToInt(numPeopleSlr.value);
        numPeopleTxt.text = numPeopleSlr.value.ToString();
    }

    public void OnSymptomaticInfectionRateChanged() {
        sim.symptomaticInfectionRate = Mathf.Round(symptomaticInfectionRateSlr.value * 1000) / 1000;
        symptomaticInfectionRateTxt.text = sim.symptomaticInfectionRate.ToString();

        asymptomaticInfectionRateSlr.value = 1 - symptomaticInfectionRateSlr.value;
    }

    public void OnAsymptomaticInfectionRateChanged() {
        sim.asymptomaticInfectionRate = Mathf.Round(asymptomaticInfectionRateSlr.value * 1000) / 1000;
        asymptomaticInfectionRateTxt.text = sim.asymptomaticInfectionRate.ToString();

        symptomaticInfectionRateSlr.value = 1 - asymptomaticInfectionRateSlr.value;
    }

    public void OnSymptomaticIfrChanged() {
        sim.symptomaticFatalityRate = Mathf.Round(symptomaticIfrRateSlr.value * 10000) / 10000;
        symptomaticIfrTxt.text = sim.symptomaticFatalityRate.ToString();
    }

    public void OnAsymptomaticRecoveryDurationChanged() {
        sim.asymptomaticRecoveryTime = Mathf.RoundToInt(asymptomaticRecoveryDurationSlr.value);
        asymptomaticRecoveryDurationTxt.text = sim.asymptomaticRecoveryTime.ToString();
    }

    public void OnMaskEffectivenessChanged() {
        sim.maskInfectionReductionRate = Mathf.Round(maskInfectionReductionRateSlr.value * 1000) / 1000;
        maskInfectionReductionRateTxt.text = sim.maskInfectionReductionRate.ToString();
    }

    public void OnMaskUsageChanged() {
        sim.maskPercentage = Mathf.Round(maskUsagePercentageSlr.value * 1000) / 1000;
        maskUsagePercentageTxt.text = sim.maskPercentage.ToString();
    }

    public void OnSocialDistanceUsageChanged() {
        sim.socialDistancingPercentage = Mathf.Round(socialDistancingUsagePercentageSlr.value * 1000) / 1000;
        socialDistancingUsagePercentageTxt.text = sim.socialDistancingPercentage.ToString();
    }

    public void OnInfectionRateChanged() {
        sim.infectionRate = Mathf.Round(infectionRateSlr.value * 1000) / 1000;
        infectionRateTxt.text = sim.infectionRate.ToString();
    }
}
