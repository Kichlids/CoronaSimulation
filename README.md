# CoronaSimulation

This is a simulation developed on Unity Engine designed to show the spread of Coronavirus and highlight the importance of social distancing and mask usage. 

## Parameters Explained

* **Number of People** The number of people to be spawned at the beginning. Min 2, max 25. 
* **Social Distancing Usage** Chance of an individual practicing social distancing.
* **Mask Usage** Chance of an individual wearing masks.
* **Mask Effectiveness** Reduction of chance to infect when wearing masks.
* **Infection Rate** Chance of transmitting coronavirus. 
* **Symptomatic Infection Rate** Chance of an individual showing symptoms after contracting Coronavirus.
* **Asymptomatic Infection Rate** Chance of an individual not showing symptoms after contracting Coronavirus.
* **Symptomatic Infection Fatality Rate** Chance of symptomatic infection eventually leading to death.
* **Asymptomatic Recovery Duration** Duration of time asymptomatic individuals need to "self-cure."

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
* Infected individuals are either symptomatic or asymptomatic.
* Asymptomatic individuals automatically recover after some duration of time.
* Individuals practicing social distancing will first occupy even numbered office first, then the odd numbered office. They will also occupy even numbered seats in meeting rooms first in the same way. 
* Individuals wearing masks will always have them on. 
* Individuals does not contract coronavirus from surface areas. 

#### Likely Scenario

Below are the simulation's recommended settings. See Source below.

* Wearing mask reduces infection rate by 65%.
* The infection rate is 50%.
* There is 60% chance to show symptoms after becoming infected.
* There is 40% chance to not show symptoms after becoming infected.
* There is 1.3% symptomatic infection fatality rate.


## Sources

The sources listed below contributed construction of the assumptions to make the simulation as realistic as possible.

* https://www.cdc.gov/coronavirus/2019-ncov/hcp/planning-scenarios.html#table-2=
* https://www.ucdavis.edu/coronavirus/news/your-mask-cuts-own-risk-65-percent/
* https://www.healthaffairs.org/doi/full/10.1377/hlthaff.2020.00455

* Building layout inspired by University of Oklahoma's Engineering Lab 2nd Floor.
