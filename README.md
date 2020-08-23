# CoronaSimulation

This is a simulation developed on Unity Engine designed to show the spread of Coronavirus and highlight the importance of social distancing and mask usage. 

## Parameters Explained

* **Number of People** The number of people to be spawned at the beginning. Min 2, max 25. 
* **Social Distancing Usage** Chance of an individual practicing social distancing.
* **Mask Usage** Chance of an individual wearing masks.
* **Mask Effectiveness** Reduction of chance to infect when wearing masks.
* **Infection Rate** Chance of transmitting coronavirus. 
* **Symptomatic Infection Rate** Chance of patient showing symptoms after contracting Coronavirus.
* **Asymptomatic Infection Rate** Chance of patient not showing symptoms after contracting Coronavirus.
* **Symptomatic Infection Fatality Rate** Chance of symptomatic infection eventually leading to death.
* **Asymptomatic Recovery Duration** Duration of time asymptomatic patients need to "self-cure."

## Assumptions

Below are the assumptions made when creating this simulation:

#### Environment

* The ongoing pandemic has forced everyone to claim their own individual office. This office is not to be shared with anyone. 
* There are two meeting rooms, each room housing 6 individuals maximum.

#### Individuals

These are the behaviours individuals can take:

* Claim an office.
* Occupy a seat in a meeting room.
* Visit another individual.

Each action other than claiming an office is as equally likely to be taken as other actions.

#### General Assumptions

* Individuals does not need to use the restroom or exit the building. Individuals may only take actions specified above.
