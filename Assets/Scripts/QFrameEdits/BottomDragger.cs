using UnityEngine;
using System.Collections;

public class BottomDragger : DraggerAbstract {

    public override void abstractUpdate() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            //turn left for bottom
            MovingPotential(new Vector3(-movement, 0, 0));

        } else if (Input.GetKey(KeyCode.RightArrow)) {
            // turn right for bottom 
            MovingPotential(new Vector3(movement, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            OnKeyDown();
        }
    }
}
