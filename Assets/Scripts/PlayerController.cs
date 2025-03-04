using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración")]
    float horizontal, vertical;
    public float moveSpeed = 2.5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 7f;
    public bool isGrounded;
    [SerializeField] private Rigidbody playerRigidbody;

    private Vector3 moveDirection;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void HandleInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {   Debug.Log("Jump");
            Jump();
        }
    }

    private void Jump()
    {
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }
    private void Move()
    {
        // Movimiento basado en Rigidbody
        Vector3 velocity = moveDirection * moveSpeed;
        playerRigidbody.velocity = new Vector3(velocity.x, playerRigidbody.velocity.y, velocity.z);
    }

    private void Rotate()
    {
        // Rotación hacia la dirección de movimiento
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
}

