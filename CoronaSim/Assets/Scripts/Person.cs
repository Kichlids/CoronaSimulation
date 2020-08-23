using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour {
    private SimManager sim;
    private NavMeshAgent agent;

    public bool isUsingMask;
    public bool isSocialDistancing;

    public bool canSetRoute = true;

    // Office
    public bool notInOffice;
    public Office myOffice;

    // Keep track of discussion room and seating this occupied
    public  bool inDiscussionRoom;
    private DiscussionRoom myDiscussionRoom;
    private int drSeatIndex;

    // Person gameobject being visited
    public GameObject personVisiting;
    public bool visingPerson;

    private const float maxTimeUntilAction = 15f;
    private float distDelta = 1.1f;
    private Vector3 targetPos;
    public float timeUntilAction = -1f;
    public float timer = 0f;

    private void Start() {
        sim = SimManager._sim;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;

        sim.people.Add(gameObject);

        Init();

        ClaimEmptyOffice();
    }

    private void Update() {

        if (Vector3.Distance(transform.position, myOffice.seats[0].position) < distDelta) {
            notInOffice = false;
        }
        else {
            notInOffice = true;
        }

        if (!canSetRoute) {
            // Agent on the way to something

            if (Vector3.Distance(transform.position, targetPos) < distDelta) {
                // arrived
                canSetRoute = true;
                timeUntilAction = Random.Range(10f, maxTimeUntilAction);
                timer = 0f;
            }
        }
        else {
            // Agent plan to move in x time
            if (timer > timeUntilAction) {

                // Go back home
                if (notInOffice) {
                    if (inDiscussionRoom) {
                        myDiscussionRoom.seatsStatus[drSeatIndex] = false;
                    }
                    // Go back home
                    Go(myOffice.seats[0].position);
                }
                // Go out
                else {
                    int rand = Random.Range(0, 2);
                    if (rand == 0) {
                        List<GameObject> others = new List<GameObject>();
                        for (int i = 0; i < sim.people.Count; i++) {
                            if (sim.people[i].GetInstanceID() != gameObject.GetInstanceID()) {
                                others.Add(sim.people[i]);
                            }
                        }
                        int rand2 = Random.Range(0, others.Count);
                        personVisiting = others[rand2];

                        
                        Go(others[rand2].transform.position);
                        StartCoroutine(InteractPerson(personVisiting));
                    }
                    else {
                        GoToDiscussionRoomEmpty();
                    }
                }
            }

            timer += Time.deltaTime;
        }
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
            agent.radius = 0.5f;
        }
        else {
            isSocialDistancing = false;
            agent.radius = 0.5f;
        }
    }

    public void Go(Vector3 position) {
        canSetRoute = false;
        targetPos = position;
        agent.SetDestination(position);
    }

    public IEnumerator InteractPerson(GameObject obj) {
        
        while (Vector3.Distance(transform.position, obj.transform.position) > distDelta) {
            yield return null;
        }

        agent.speed = 0;
        obj.GetComponent<NavMeshAgent>().speed = 0;
        visingPerson = true;
        yield return new WaitForSeconds(sim.personInteractionDuration);
        visingPerson = false;
        agent.speed = sim.personSpeed;
        obj.GetComponent<NavMeshAgent>().speed = sim.personSpeed;

        personVisiting = null;
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

    public void GoToDiscussionRoomEmpty() {
        List<GameObject> disc = sim.discussionRooms;

        Vector3 pos = Vector3.zero;

        int rand = Random.Range(0, disc.Count);

        if (!disc[rand].GetComponent<DiscussionRoom>().IsFull()) {

            if (isSocialDistancing) {
                // Use only 0, 2, 4 seats

                for (int i = 0; i < disc[rand].GetComponent<DiscussionRoom>().seats.Count; i += 2) {
                    DiscussionRoom dr = disc[rand].GetComponent<DiscussionRoom>();
                    if (!dr.seatsStatus[i]) {
                        if (dr.ClaimSeat(i)) {
                            pos = dr.seats[i].position;

                            inDiscussionRoom = true;
                            myDiscussionRoom = dr;
                            drSeatIndex = i;
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

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;

        float rad = SimLib.ConvertToInGameLength(sim.socialDistanceLength);
        Gizmos.DrawWireSphere(transform.position - new Vector3(0, 1, 0), rad);
    }

    private void OnDestroy() {
        sim.people.Remove(gameObject);
    }
}