using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithMouse : MonoBehaviour {

    Vector3 originaPos;
    float BoundariesOffset = 0.5f;
    float movedownY = 0.0f;
    float sensitivityY = 0.08f;

    float movedownX = 0.0f;
    float sensitivityX =0.08f;
    // Use this for initialization
    void Start () {
        originaPos = transform.position;

    }

    void DoMoveY()
    {
        movedownY += Input.GetAxis("Mouse Y") * sensitivityY;
        if (Input.GetAxis("Mouse Y") > 0)
        {
            transform.Translate(Vector3.back * movedownY);
        }
        if (Input.GetAxis("Mouse Y") < 0)
        {
            transform.Translate(Vector3.back * movedownY);
        }
      //  print(movedownY);
        movedownY = 0.0f;
    }
    void DoMoveX()
    {
        movedownX += Input.GetAxis("Mouse X") * sensitivityX;
        if (Input.GetAxis("Mouse X") > 0)
        {
            transform.Translate(Vector3.left * movedownX);
        }
        if (Input.GetAxis("Mouse X") < 0)
        {
            transform.Translate(Vector3.left * movedownX);
        }
       // print(movedownX);
        movedownX = 0.0f;
    }

    void DoMoveZ()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) { transform.Translate(Vector3.up * Time.deltaTime); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { transform.Translate(Vector3.down * Time.deltaTime); }
    }


    void lockBoundaries() {

        if (transform.position.x > originaPos.x + BoundariesOffset) { transform.position = new Vector3(originaPos.x + BoundariesOffset, transform.position.y, transform.position.z) ; }
        if (transform.position.x < originaPos.x - BoundariesOffset) { transform.position = new Vector3(originaPos.x - BoundariesOffset, transform.position.y, transform.position.z); }
        if (transform.position.z > originaPos.z + BoundariesOffset) { transform.position = new Vector3(transform.position.x, transform.position.y, originaPos.z + BoundariesOffset); }
        if (transform.position.z < originaPos.z - BoundariesOffset) { transform.position = new Vector3(transform.position.x, transform.position.y, originaPos.z - BoundariesOffset); }
    }
    // Update is called once per frame
    void Update () {

        if (GameSettings.Instance.IsTestModeON)
        {
            lockBoundaries();
            DoMoveX();
            DoMoveY();
            DoMoveZ();

            if (Input.GetKeyDown(KeyCode.KeypadEnter)) { transform.position = originaPos; }
        }
	}
}



