using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cReaccionDefensaBasicaVoluntadDelCreador : cReaccionDefensaBasica
{
    override public int DeterminarNumeroDeDados()
    {
        int numeroDeDados = base.DeterminarNumeroDeDados();
        numeroDeDados += (personaje.arma as cLaVoluntadDelCreador).DadosDeSedDeSangre - personaje.impuesto;
        if (reroleando)
        {
            numeroDeDados += dadosExtrasParaReroll;
        }
        return numeroDeDados;
    }
}
