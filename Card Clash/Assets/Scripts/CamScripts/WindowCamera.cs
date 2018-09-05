using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowCamera : MonoBehaviour {

    private Vector3 zero;
    private GameObject mainChar;
    private Vector3 target;
    private Vector3 diff;

	// Use this for initialization
	void Start () {
        mainChar = GameObject.Find("Main Character");
        zero = Vector3.zero;
        target = Vector3.zero;
        diff = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //sets the target to the main character's position Vector
        target = mainChar.transform.position;

        //finds the difference between the target and the camera's position
        diff = target - transform.position;

        //keeps rotation locked
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            0, 0);

        //if the camera is not ontop of the target, smooth over slowly to the target
        if (diff.x != 0 && diff.y != 0)
        {
            transform.position = Vector3.SmoothDamp(transform.position, mainChar.transform.position, ref zero, 0.35f);
        }

        //if the target is 2.0f away, smooth over to the target quicker
        if (diff.x > 2.0f || diff.x < -2.0f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, mainChar.transform.position, ref zero, 0.5f);
        }

        if (diff.y > 2.0f || diff.y < -2.0f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, mainChar.transform.position, ref zero, 0.5f);
        }

        //set the camera's Z position to -12
        transform.position = new Vector3(transform.position.x, transform.position.y, -12);

    }
}
