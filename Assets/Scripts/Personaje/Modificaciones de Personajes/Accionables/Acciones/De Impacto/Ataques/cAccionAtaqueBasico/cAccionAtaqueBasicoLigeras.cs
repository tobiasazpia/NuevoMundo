using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAccionAtaqueBasicoLigeras : cAccionAtaqueBasico
{
    override public int DeterminarNumeroDeDados()
    {
        int numeroDeDados = base.DeterminarNumeroDeDados();
        if (!(personaje.arma as cArmasLigeras).perYaActuo(c.personajeObjetivo))
        {
            numeroDeDados++;
        }
        return numeroDeDados;
    }
}
