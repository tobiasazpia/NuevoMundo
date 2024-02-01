using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAccionAtaqueBasicoArco : cAccionAtaqueBasico
{
    override public int DeterminarNumeroDeDados()
    {
        int numeroDeDados = base.DeterminarNumeroDeDados();
        if (c.HayEnemigosEnMelee(personaje))
        {
            numeroDeDados -= 1;
            if (personaje.zonaActual == c.personajeObjetivo.zonaActual)
            {
                numeroDeDados -= 1;
            }
        }
        return numeroDeDados;
    }

    override public void RevisarLegalidad()
    {
        esLegal = c.HayEnemigosVivosEnRango();
    }
}
