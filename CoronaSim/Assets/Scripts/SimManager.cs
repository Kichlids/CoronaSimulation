using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SimManager : MonoBehaviour {
    [Header("Environment Variables")]
    public float secondsInDay;
    public float inGameTime;
    public int maxNumPeople;
    public int numPeople;
    [Space]
    public GameObject officesParent;
    public GameObject discussionRoomParent;
    public List<GameObject> offices;
    public List<GameObject> discussionRooms;

    [Header("Covid Variables")]
    [Range(0, 1)]
    public float infectionRate;
    [Range(0, 1)]
    public float symptomaticInfectionRate;
    [Range(0, 1)]
    public float asymptomaticInfectionRate;
    [Range(0, 1)]
    public float symptomaticFatalityRate;
    public int asymptomaticRecoveryTime;

    [Header("Prevention Variables")]
    [Range(0, 1)]
    public float maskInfectionReductionRate;
    [Range(0, 1)]
    public float maskPercentage;
    [Range(0, 1)]
    public float socialDistancingPercentage;
    public float socialDistanceLength = 3;




    public static SimManager _sim;

    private void Awake() {
        if (_sim != null && _sim != this) {
            Destroy(this.gameObject);
        }
        else {
            _sim = this;
        }

        // Populate offices
        offices = new List<GameObject>();
        Room[] temp = officesParent.GetComponentsInChildren<Room>();
        foreach (Room t in temp) {
            if (t.gameObject.GetInstanceID() != officesParent.GetInstanceID()) {
                offices.Add(t.gameObject);
            }
        }

        // Populate discussion rooms
        discussionRooms = new List<GameObject>();
        Room[] temp2 = discussionRoomParent.GetComponentsInChildren<Room>();
        foreach (Room t in temp2) {
            if (t.gameObject.GetInstanceID() != discussionRoomParent.GetInstanceID()) {
                discussionRooms.Add(t.gameObject);
            }
        }
    }

    private void Start() {
        inGameTime = 0f;

       

        maxNumPeople = offices.Count;
    }

    private void Update() {
        IncrementGameTime();
    }

    private void IncrementGameTime() {

        inGameTime += Time.deltaTime;

        if (inGameTime > secondsInDay) {
            inGameTime = secondsInDay;
            Debug.Log("Day complete");
        }
    }
}

