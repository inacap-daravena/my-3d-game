using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    Animator animator;
    Rigidbody rb;
    Camera cam;

    public float velocidad = 5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MoverPersonaje();
    }

    void MoverPersonaje()
    {
        // Tomamos los inputs del jugador
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        // Calculamos la fuerza donde el usuario quiere ir.
        Vector3 fuerzaObjetivo = new Vector3 (inputX, 0,inputZ) * velocidad;

        // Calculamos la diferencia entre la fuerza objetivo y la velocidad del jugador
        Vector3 diferenciaFuerza = fuerzaObjetivo - new Vector3(rb.velocity.x, 0, rb.velocity.z);

        // Calcula la magnitud de nuestra fuerza para utilizarla en nuestro Animator
        float magnitud = new Vector2(inputX, inputZ).sqrMagnitude;

        // Finalmente aplicamos la fuerza
        rb.AddForce (diferenciaFuerza);
        animator.SetFloat("Speed", magnitud);

        GirarPersonaje(inputX, inputZ);
    }

    void GirarPersonaje(float x, float z)
    {
        // Movimiento relativa a la cámara
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        // Vector3(0.04, 0.01, 0.8) -> Vector3(0.05, 0, 0.8)
        forward.y = 0f;
        right.y = 0f;

        // Vector3(0.04, 0.01, 0.8) -> Vector3(0, 0, 1)
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * z + right * x;

        if (desiredMoveDirection != Vector3.zero) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.1f);
        }
    }
}
