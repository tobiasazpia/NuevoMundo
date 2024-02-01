using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cReaccionDefensaBasicaArco : cReaccionDefensaBasica
{
    override public int DeterminarNumeroDeDados()
    {
        int numeroDeDados = base.DeterminarNumeroDeDados();
        if (c.HayEnemigosEnMelee(personaje))
        {
            numeroDeDados -= 1;
            if (personaje.zonaActual == c.personajeActivo.zonaActual && c.personajeObjetivo.nombre == personaje.nombre)
            {
                numeroDeDados -= 1;
            }
        }
        return numeroDeDados;
    }
}
