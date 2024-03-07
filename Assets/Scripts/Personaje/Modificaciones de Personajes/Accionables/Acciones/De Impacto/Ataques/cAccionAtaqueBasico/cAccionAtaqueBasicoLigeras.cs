using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAccionAtaqueBasicoLigeras : cAccionAtaqueBasico
{
    override public int DeterminarNumeroDeDados()
    {
        int numeroDeDados = base.DeterminarNumeroDeDados();
        Debug.Log("Ab, per ya actuo?");
        if (!(personaje.arma as cArmasLigeras).perYaActuo(c.personajeObjetivo))
        {
            numeroDeDados++;
            Debug.Log("Bonus aplicado! Dados ahora " + numeroDeDados);
        }
        else Debug.Log("si actuo! no aplicamos bonus");
        return numeroDeDados;
    }
}
