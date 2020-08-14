using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int numSeats;
    public GameObject seatParent;
    public List<Transform> seats;

    public bool[] seatsStatus;
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

        isOccupied = false;
        covidInfected = false;
    }
}
