using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cReaccionDefensaBasicaJaieiy : cReaccionDefensaBasica
{
    override public int DeterminarNumeroDeDados()
    {
        int numeroDeDados = base.DeterminarNumeroDeDados();
        if (personaje.arma.maestria > 4) numeroDeDados += (personaje.arma as cJaieiy).DadosMaestro;
        if (reroleando)
        {
            numeroDeDados += dadosExtrasParaReroll;
        }
        return numeroDeDados;
    }
}
