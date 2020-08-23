using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SimManager : MonoBehaviour {
    [Header("Environment Variables")]
    public float secondsInDay;
    public int inGameDaysPassed;
    public float inGameTime;
    public int maxNumPeople;
    public int numPeople;
    public GameObject personPrefab;
    public Transform spawnPoint;
    [Space]
    public GameObject officesParent;
    public GameObject discussionRoomParent;
    public List<GameObject> offices;
    public List<GameObject> discussionRooms;
    public List<GameObject> people;
    [Space]
    public Color nonInfected;
    public Color asympatomatic;
    public Color symptomatic;
    [Space]
    public float personSpeed;
    public float personInteractionDuration = 2f;

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

    private bool beginSim = false;

    [Header("Stats")]
    public int numUninfected;
    public int numSymptomatic;
    public int numAsymptomatic;
    public int numRecovered;


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
        inGameDaysPassed = 0;
        maxNumPeople = offices.Count;
    }

    private void Update() {
        if (beginSim) {
            IncrementGameTime();
            GetInfectionStats();
        }
    }

    private void IncrementGameTime() {

        inGameTime += Time.deltaTime;

        if (inGameTime > secondsInDay) {
            inGameDaysPassed++;
            inGameTime = 0;
            Debug.Log("Day complete");
        }
    }

    private void GetInfectionStats() {
        int non = 0, symp = 0, asymp = 0;

        foreach(GameObject person in people) {
            Covid covid = person.GetComponent<Covid>();
            if (covid.status == InfectionStatus.NotInfected) {
                non++;
            }
            else if (covid.status == InfectionStatus.Symptomatic) {
                symp++;
            }
            else {
                asymp++;
            }
        }

        numUninfected = non;
        numSymptomatic = symp;
        numAsymptomatic = asymp;
    }
    public IEnumerator BeginSim() {
        beginSim = true;
        for (int i = 0; i < numPeople; i++) {
            GameObject inst = Instantiate(personPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
            inst.transform.parent = spawnPoint;
            if (i == 0) {
                inst.GetComponent<Covid>().status = InfectionStatus.Symptomatic;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}

