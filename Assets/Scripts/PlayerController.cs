using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 movementInputDirection;
    bool touchControlEnabled = false; // set true as soon as player touches screen

    bool touchStart = false;
    Vector3 touchStartPos; // screen position of players first screen touch

    [SerializeField] float maxtouchDrag = 5;
    [SerializeField] float mintouchDrag = 0.5f;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(touchControlEnabled == false)
        {
            if (Input.touchCount > 0)
            {
                touchControlEnabled = true;
            }

            MKControls();
        }
        else
        {
            TouchControls();
        }
    }

    void TouchControls()
    {
        if (Input.touchCount > 0)
        {
            if (touchStart == false)
            {
                touchStart = true;
                touchStartPos = Input.touches[0].position;
                movementInputDirection = Vector3.zero;
            }
            else
            {
                Vector3 touchStartWorld = cam.ScreenToWorldPoint(touchStartPos);
                Vector3 currentTouchToWorld = cam.ScreenToWorldPoint(Input.touches[0].position);
                float dist = Vector3.Distance(touchStartWorld, currentTouchToWorld);
                Vector3 touchOffset = currentTouchToWorld - touchStartWorld;

                if (dist > mintouchDrag) // add a bit of a deadzone to touch movement
                {
                    float mag = touchOffset.magnitude;
                    if(mag > maxtouchDrag)
                    {
                        mag = maxtouchDrag;
                    }
                    movementInputDirection = touchOffset;
                    movementInputDirection = movementInputDirection.normalized * mag;
                }
                else
                {
                    movementInputDirection = Vector3.zero;
                }
            }
        }
        else
        {
            touchStart = false;
            movementInputDirection = Vector3.zero;
        }
    }
    void MKControls()
    {
        movementInputDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    }
}
