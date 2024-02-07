using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cReaccionDefensaBasicaLigeras : cReaccionDefensaBasica
{
    override public int DeterminarNumeroDeDados()
    {
        Debug.Log(personaje.nombre + " ve si puede usar bonus de yaActuo contra " + c.personajeObjetivo.nombre + " en su " + nombre);
        int numeroDeDados = base.DeterminarNumeroDeDados();
        if (!(personaje.arma as cArmasLigeras).perYaActuo(c.personajeObjetivo))
        {
            numeroDeDados++;
            Debug.Log("Bonus aplicado! Dados ahora " + numeroDeDados);
        }
        else Debug.Log("Bonus no aplicado");
        return numeroDeDados;
    }
}
