using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour {
    private SimManager sim;
    private NavMeshAgent agent;

    public Office myOffice;

    public bool isUsingMask;
    public bool isSocialDistancing;


    public bool away;
    public Vector3 targetPos;
    public bool canSetRoute = true;
    private float diff = 1.1f;
    private const float maxTimeUntilAction = 30f;
    public float timeUntilAction = -1f;
    public float timer = 0f;

    private void Start() {
        sim = SimManager._sim;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;

        Init();

        ClaimEmptyOffice();
    }

    private void Update() {

        if (Vector3.Distance(transform.position, myOffice.seats[0].position) < diff) {
            away = false;
        }
        else {
            away = true;
        }

        if (!canSetRoute) {
            // Agent on the way to something
            //print(Vector3.Distance(transform.position, targetPos));
            if (Vector3.Distance(transform.position, targetPos) < diff) {
                // arrived
                canSetRoute = true;
                timeUntilAction = Random.Range(10f, maxTimeUntilAction);
                timer = 0f;
            }
        }
        else {
            // Agent plan to move in x time
            if (timer > timeUntilAction) {

                if (away) {
                    // Go back home
                    Go(myOffice.seats[0].position);
                }
                else {
                    GoToDiscussionRoomEmpty();
                }
            }



            timer += Time.deltaTime;
        }
    }

    public void GoToDiscussionRoomEmpty() {
        List<GameObject> disc = sim.discussionRooms;

        Vector3 pos = Vector3.zero;

        int rand = Random.Range(0, disc.Count);
        print("Disc room: " + rand);

        if (!disc[rand].GetComponent<DiscussionRoom>().IsFull()) {
            
            if (isSocialDistancing) {
                // Use only 0, 2, 4 seats

                for (int i = 0; i < disc[rand].GetComponent<DiscussionRoom>().seats.Count; i += 2) {
                    DiscussionRoom dr = disc[rand].GetComponent<DiscussionRoom>();
                    if (!dr.seatsStatus[i]) {
                        if (dr.ClaimSeat(i)) {
                            print("index: " + i);
                            pos = dr.seats[i].position;
                            break;
                        }
                    }
                }
            }
            else {
                // Use all

                for (int i = 0; i < disc[rand].GetComponent<DiscussionRoom>().seats.Count; i++) {
                    DiscussionRoom dr = disc[rand].GetComponent<DiscussionRoom>();
                    if (!dr.seatsStatus[i]) {
                        if (dr.ClaimSeat(i)) {
                            print("index: " + i);
                            pos = dr.seats[i].position;
                            break;
                        }
                    }
                }
            }
        }

        if (pos == Vector3.zero) {
            // dont
        }
        else {
            Go(pos);
        }
    }

    public void Go(Vector3 position) {
        canSetRoute = false;
        targetPos = position;
        agent.SetDestination(position);
    }


    public void Init() {
        SetSocialDistancingStance();
        SetMaskStance();
    }

    private void SetMaskStance() {
        float perc = Random.Range(0f, 1f);
        if (perc <= sim.maskPercentage) {
            isUsingMask = true;
        }
        else {
            isUsingMask = false;
        }
    }

    private void SetSocialDistancingStance() {
        float perc = Random.Range(0f, 1f);
        if (perc <= sim.socialDistancingPercentage) {
            isSocialDistancing = true;
            agent.radius = SimLib.ConvertToInGameLength(sim.socialDistanceLength);
        }
        else {
            isSocialDistancing = false;
            agent.radius = 0.5f;
        }
    }

    private void ClaimEmptyOffice() {

        myOffice = null;
        List<GameObject> offices = sim.offices;


        if (isSocialDistancing) {
            // Attempt to leave one space in between
            for (int i = 0; i < offices.Count; i++) {

                if (!offices[i].GetComponent<Office>().isOccupied) {
                    myOffice = offices[i].GetComponent<Office>();
                    break;
                }
            }
            // Couldn't find one empty both sides so check one by one
            if (myOffice == null) {
                for (int i = 0; i < offices.Count; i++) {
                    if (!offices[i].GetComponent<Office>().isOccupied) {
                        myOffice = offices[i].GetComponent<Office>();
                        break;
                    }
                }
            }
        }
        else {

            // Randomly occupy one
            List<GameObject> emptyOffices = new List<GameObject>();

            foreach (GameObject o in offices) {
                if (!o.GetComponent<Room>().isOccupied) {
                    emptyOffices.Add(o);
                }
            }

            if (emptyOffices.Count > 0) {
                int rand = Random.Range(0, emptyOffices.Count);
                myOffice = emptyOffices[rand].GetComponent<Office>();
            }
        }

        if (myOffice == null) {
            print("non found");
            Destroy(gameObject);
            enabled = false;
            return;
        }

        if (!myOffice.ClaimOffice()) {
            ClaimEmptyOffice();
        }

        Go(myOffice.seats[0].position);
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;

        float rad = SimLib.ConvertToInGameLength(sim.socialDistanceLength);
        Gizmos.DrawWireSphere(transform.position, rad);
    }
}