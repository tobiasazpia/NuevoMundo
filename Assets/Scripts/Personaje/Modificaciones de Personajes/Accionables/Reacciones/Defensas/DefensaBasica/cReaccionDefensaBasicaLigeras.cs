using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cReaccionDefensaBasicaLigeras : cReaccionDefensaBasica
{
    override public int DeterminarNumeroDeDados()
    {
        Debug.Log("DB, per ya actuo?");
        int numeroDeDados = base.DeterminarNumeroDeDados();
        if (!(personaje.arma as cArmasLigeras).perYaActuo(c.personajeActivo))
        {
            numeroDeDados++;
            Debug.Log("Bonus aplicado! Dados ahora " + numeroDeDados);
        }
        else Debug.Log("si actuo! no aplicamos bonus");
        return numeroDeDados;
    }
}
