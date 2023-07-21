using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] float strafeSpeed = 5;
    [SerializeField] float forwardSpeed = 5;

    PlayerController controller;
    // Start is called before the first frame update
    void Start()
    {
        // get components
        rb = GetComponent<Rigidbody2D>();
        controller = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(controller.movementInputDirection);
    }

    public void Move(Vector3 _velocity)
    {
        Vector3 localVelocity = transform.InverseTransformDirection(_velocity);
        Vector2 velocity2d = new Vector2(localVelocity.x * strafeSpeed, localVelocity.y * forwardSpeed);

        rb.MovePosition(rb.position + velocity2d * Time.deltaTime);
    }
}
