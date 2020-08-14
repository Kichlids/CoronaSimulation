using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class SimManager : MonoBehaviour
{
    public float secondsInDay;
    public float inGameTime;

    public GameObject officesParent;
    public GameObject discussionRoomParent;
    public List<Room> offices;
    public List<Room> discussionRooms;

    public int maxNumPeople;
    public int numPeople;
    public bool wearMask;
    public bool socialDistance;
    public float socialDistanceLength;


    public static SimManager _sim;

    private void Awake() {
        if (_sim != null && _sim != this) {
            Destroy(this.gameObject);
        }
        else {
            _sim = this;
        }
    }

    private void Start() {
        inGameTime = 0f;

        offices = new List<Room>();
        discussionRooms = new List<Room>();

        Room[] temp = officesParent.GetComponentsInChildren<Room>();
        foreach (Room t in temp) {
            if (t.gameObject.GetInstanceID() != officesParent.GetInstanceID()) {
                offices.Add(t);
            }
        }

        Room[] temp2 = discussionRoomParent.GetComponentsInChildren<Room>();
        foreach (Room t in temp2) {
            if (t.gameObject.GetInstanceID() != discussionRoomParent.GetInstanceID()) {
                discussionRooms.Add(t);
            }
        }



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
