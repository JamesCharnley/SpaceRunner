using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{

    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth = 0;
    [SerializeField] float maxSheild = 100;
    [SerializeField] float currentShield = 0;

    Rigidbody2D rb;
    [SerializeField] float strafeSpeed = 5;
    [SerializeField] float forwardSpeed = 5;

    [SerializeField] float damagePerMass = 3;

    PlayerController controller;

    Vector3 queuedExternalImpulseForce = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        // get components
        rb = GetComponent<Rigidbody2D>();
        controller = FindObjectOfType<PlayerController>();

        // set variables
        currentHealth = maxHealth;
        currentShield = maxSheild;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(controller.movementInputDirection);

        if(queuedExternalImpulseForce != Vector3.zero)
        {
            rb.AddForce(queuedExternalImpulseForce, ForceMode2D.Impulse);
            queuedExternalImpulseForce = Vector3.zero;
        }
    }

    public void Move(Vector3 _velocity)
    {
        Vector3 localVelocity = transform.InverseTransformDirection(_velocity);
        Vector2 velocity2d = new Vector2(localVelocity.x * strafeSpeed, localVelocity.y * forwardSpeed);

       // rb.MovePosition(rb.position + velocity2d * Time.deltaTime);
        rb.AddForce(velocity2d, ForceMode2D.Force);
    }

    public void DoDamage(float _amount)
    {
        if(currentShield > 0)
        {
            if(currentShield - _amount < 0)
            {
                _amount -= currentShield;
                currentShield = 0;
            }
        }

        currentHealth -= _amount;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Obstacle obs = collision.gameObject.GetComponent<Obstacle>();
        if(obs != null)
        {
            Rigidbody2D otherRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if(otherRB != null)
            {
                float dam = damagePerMass * otherRB.mass;
                DoDamage(dam);

                Vector3 force = Vector3.Project(rb.position - otherRB.position, otherRB.velocity);
                queuedExternalImpulseForce += force;
            }
        }
    }
}
