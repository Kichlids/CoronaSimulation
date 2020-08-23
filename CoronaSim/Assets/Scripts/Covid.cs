using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InfectionStatus { NotInfected, Asymptomatic, Symptomatic };

public class Covid : MonoBehaviour
{
    private SimManager sim;

    public float InfectionCd = 2f;  // See personInteractionDuration in SimManager
    public float infectionTimer = 0;

    public float asymptomaticRecoveryTimer = 0;

    public InfectionStatus status;

    public List<GameObject> nearbyPeople;

    private void Start() {
        sim = SimManager._sim;

        nearbyPeople = new List<GameObject>();

        InfectionCd = sim.personInteractionDuration;
    }

    public void Init(InfectionStatus _status) {
        status = _status;
    }

    private void Update() {

        DisplayInfectionStatus();

        // Infected
        if (status != InfectionStatus.NotInfected) {

            if (infectionTimer > InfectionCd) {
                infectionTimer = 0;

                nearbyPeople = SimLib.FindNearbyPeople(gameObject, SimLib.ConvertToInGameLength(sim.socialDistanceLength));

                // Attempt to infect the noninfected
                foreach (GameObject p in nearbyPeople) {
                    if (p.GetComponent<Covid>().status == InfectionStatus.NotInfected) {
                        float perc = Random.Range(0f, 1f);

                        // Mask modifier
                        float maskMod = 1f;
                        if (p.GetComponent<Person>().isUsingMask)
                            maskMod = 1 - sim.maskInfectionReductionRate;

                        if (perc <= sim.infectionRate * maskMod) {
                            Infect(p);
                        }
                    }
                }
            }

            // Recover if asymptomatic
            if (status == InfectionStatus.Asymptomatic) {
                if (asymptomaticRecoveryTimer > sim.asymptomaticRecoveryTime * sim.secondsInDay) {
                    status = InfectionStatus.NotInfected;
                    sim.numRecovered++;
                    Debug.Log("Recovered");
                }
                asymptomaticRecoveryTimer += Time.deltaTime;
            }

            infectionTimer += Time.deltaTime;
        }
        // Not infected
        else {
            infectionTimer = 0;
            asymptomaticRecoveryTimer = 0;
        }
    }

    private void Infect(GameObject subject) {
        float perc = Random.Range(0f, 1f);
        if (perc <= sim.symptomaticInfectionRate) {
            subject.GetComponent<Covid>().status = InfectionStatus.Symptomatic;
        }
        else {
            subject.GetComponent<Covid>().status = InfectionStatus.Asymptomatic;
        }
    }

    private void DisplayInfectionStatus() {
        if (status == InfectionStatus.NotInfected) {
            gameObject.GetComponent<Renderer>().material.color = sim.nonInfected;
        }
        else if (status == InfectionStatus.Asymptomatic) {
            gameObject.GetComponent<Renderer>().material.color = sim.asympatomatic;
        }
        else {
            gameObject.GetComponent<Renderer>().material.color = sim.symptomatic;
        }
    }
}
