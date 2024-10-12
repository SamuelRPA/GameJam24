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

            // Ejecutar comportamiento seg�n el tipo de enemigo
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
                RealizarDa�o(); // Comportamiento para enemigo realista
                break;
            case TipoEnemigo.Da�oArea:
                Da�oEnArea(); // Comportamiento para enemigo que hace da�o en �rea
                break;
            case TipoEnemigo.Explosivo:
                Explosionar(); // Comportamiento para enemigo explosivo
                break;
            case TipoEnemigo.Todo_menos_Explosivo:
                RealizarDa�o();
                Da�oEnArea(); // L�gica para realizar da�o y da�o en �rea
                break;
            default:
                break;
        }
    }

    private void RealizarDa�o()
    {
        Debug.Log("El enemigo realista ha hecho da�o al jugador.");
        // Da�ar al jugador
        if (player != null) // Verifica que el jugador est� asignado
        {
            player.gameObject.SendMessage("RecibirDa�o", 10, SendMessageOptions.DontRequireReceiver); // Enviar da�o al jugador
        }
    }

    private void Da�oEnArea()
    {
        Debug.Log("El enemigo ha realizado da�o en �rea.");
        // Aqu� puedes realizar da�o a todos los enemigos cercanos
        Collider2D[] objetosCercanos = Physics2D.OverlapCircleAll(transform.position, 2.0f); // Radio de da�o en �rea

        foreach (Collider2D col in objetosCercanos)
        {
            if (col.CompareTag("Player"))
            {
                col.gameObject.SendMessage("RecibirDa�o", 5, SendMessageOptions.DontRequireReceiver); // Da�ar al jugador
            }
        }
    }

    private void Explosionar()
    {
        Debug.Log("El enemigo ha explotado.");
        // Da�o en �rea al explotar
        Da�oEnArea(); // Realiza da�o en �rea al explotar
        Destroy(gameObject); // Destruir al enemigo al explotar
    }

    // Detectar colisiones con el jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisi�n detectada con: " + collision.gameObject.name);
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("�El enemigo ha tocado al jugador!");
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
    Da�oArea,
    Explosivo,
    Todo_menos_Explosivo
}