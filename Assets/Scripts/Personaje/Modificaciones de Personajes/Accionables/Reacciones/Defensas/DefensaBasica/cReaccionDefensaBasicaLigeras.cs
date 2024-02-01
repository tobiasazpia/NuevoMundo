using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cReaccionDefensaBasicaLigeras : cReaccionDefensaBasica
{
    override public int DeterminarNumeroDeDados()
    {
        Debug.Log(personaje.nombre + " usa bonus de yaActuo contra " + c.personajeObjetivo.nombre + " en su " + nombre);
        int numeroDeDados = base.DeterminarNumeroDeDados();
        Debug.Log("Dados antes del bonus " + numeroDeDados);
        if (!(personaje.arma as cArmasLigeras).perYaActuo(c.personajeObjetivo))
        {
            numeroDeDados++;
            Debug.Log("Bonus aplicado! Dados ahora " + numeroDeDados);
        }
        return numeroDeDados;
    }
}
