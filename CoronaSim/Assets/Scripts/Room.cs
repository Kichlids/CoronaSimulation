using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int numSeats;
    public GameObject seatParent;
    public List<Transform> seats;

    public bool[] seatsStatus;  // True if taken, false if empty
    public bool isOccupied;
    public bool covidInfected;

    public void Start() {

        seats = new List<Transform>();

        // Populate seats
        Transform[] temp = seatParent.GetComponentsInChildren<Transform>();
        foreach (Transform t in temp) {
            if (t.gameObject.GetInstanceID() != seatParent.GetInstanceID()) {
                seats.Add(t);
            }
        }

        numSeats = seats.Count;
        seatsStatus = new bool[numSeats];

        //isOccupied = false;
        covidInfected = false;
    }



    public bool ClaimSeat(int index) {
        if (seatsStatus[index]==true) {
            return false;
        }
        else {
            seatsStatus[index] = true;

            isOccupied = IsFull();
            return true;
        }
    }

    public bool IsFull() {
        for (int i = 0; i < seatsStatus.Length; i++) {
            if (!seatsStatus[i])
                return false;
        }
        return true;
    }
}
