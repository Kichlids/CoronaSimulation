using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Office : Room {

    private new void Start() {
        base.Start();
    }

    public bool ClaimOffice() {
        if (isOccupied) {
            return false;
        }

        isOccupied = true;
        return true;
    }
}
