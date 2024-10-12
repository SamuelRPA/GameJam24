using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int vida = 100; // Vida inicial del jugador

    public void RecibirDa�o(int cantidad)
    {
        vida -= cantidad;
        Debug.Log("El jugador ha recibido da�o: " + cantidad);

        if (vida <= 0)
        {
            Muerte();
        }
    }

    // M�todo que se llama cuando el jugador muere
    void Muerte()
    {
        Debug.Log("El jugador ha muerto.");
        
    }
}
