using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5.0f;
    public float speed = 2.0f;

    public TipoEnemigo tipoEnemigo; // Tipo de enemigo

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool enMovimiento;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            if (direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            movement = direction;
            enMovimiento = true;

            // Ejecutar comportamiento según el tipo de enemigo
            EjecutarComportamiento();
        }
        else
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        animator.SetBool("enMovimiento", enMovimiento);
    }

    private void EjecutarComportamiento()
    {
        switch (tipoEnemigo)
        {
            case TipoEnemigo.Realista:
                RealizarDaño(); // Comportamiento para enemigo realista
                break;
            case TipoEnemigo.DañoArea:
                DañoEnArea(); // Comportamiento para enemigo que hace daño en área
                break;
            case TipoEnemigo.Explosivo:
                Explosionar(); // Comportamiento para enemigo explosivo
                break;
            case TipoEnemigo.Todo_menos_Explosivo:
                RealizarDaño();
                DañoEnArea(); // Lógica para realizar daño y daño en área
                break;
            default:
                break;
        }
    }

    private void RealizarDaño()
    {
        Debug.Log("El enemigo realista ha hecho daño al jugador.");
        // Dañar al jugador
        if (player != null) // Verifica que el jugador esté asignado
        {
            player.gameObject.SendMessage("RecibirDaño", 10, SendMessageOptions.DontRequireReceiver); // Enviar daño al jugador
        }
    }

    private void DañoEnArea()
    {
        Debug.Log("El enemigo ha realizado daño en área.");
        // Aquí puedes realizar daño a todos los enemigos cercanos
        Collider2D[] objetosCercanos = Physics2D.OverlapCircleAll(transform.position, 2.0f); // Radio de daño en área

        foreach (Collider2D col in objetosCercanos)
        {
            if (col.CompareTag("Player"))
            {
                col.gameObject.SendMessage("RecibirDaño", 5, SendMessageOptions.DontRequireReceiver); // Dañar al jugador
            }
        }
    }

    private void Explosionar()
    {
        Debug.Log("El enemigo ha explotado.");
        // Daño en área al explotar
        DañoEnArea(); // Realiza daño en área al explotar
        Destroy(gameObject); // Destruir al enemigo al explotar
    }

    // Detectar colisiones con el jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("¡El enemigo ha tocado al jugador!");
            Destroy(gameObject); // Destruir el enemigo
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}


public enum TipoEnemigo
{
    Realista,
    DañoArea,
    Explosivo,
    Todo_menos_Explosivo
}