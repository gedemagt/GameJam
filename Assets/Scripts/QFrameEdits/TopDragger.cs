using UnityEngine;
using System.Collections;

public class TopDragger : DraggerAbstract {

    public override void abstractUpdate() {
        if (Input.GetKey(KeyCode.A)) {
            //turn right for bottom
            MovingPotential(new Vector3(movement, 0, 0));
        } else if (Input.GetKey(KeyCode.D)) {
            MovingPotential(new Vector3(-movement, 0, 0));
            // turn left for bottom
        }

        if (Input.GetKey(KeyCode.S)) {
            OnKeyDown();
            
        }
    }
}
