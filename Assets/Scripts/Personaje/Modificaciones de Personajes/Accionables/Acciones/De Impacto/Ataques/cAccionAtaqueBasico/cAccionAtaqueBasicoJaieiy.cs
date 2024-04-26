using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAccionAtaqueBasicoJaieiy : cAccionAtaqueBasico
{

    override public int DeterminarNumeroDeDados()
    {
        personaje.totalDadosDelAtacante = personaje.dadosDelAtacantePorPrecavido + personaje.arma.GetDadosDelAtacanteMod();
        int numeroDeDados = 3 + personaje.atr.maña + personaje.tradicionMarcial[0] + personaje.arma.GetBonusAtaque() + personaje.BonusPAtqBporDefB + c.personajeObjetivo.totalDadosDelAtacante - personaje.impuesto;
        if (personaje.arma.maestria > 4) numeroDeDados += (personaje.arma as cJaieiy).DadosMaestro;
        if (c.movAgro) numeroDeDados -= 3;
        if (reroleando)
        {
            Debug.Log("reroll true, sumamos");
            numeroDeDados += dadosExtrasParaReroll;
        }
        else Debug.Log("reroll false, no sumamos");
        return numeroDeDados;
    }

    protected override void TiramosDaño()
    {
        if (!(c.personajeObjetivo is cMatones))
        {
            int numeroDeDados = 3 + personaje.atr.musculo * personaje.arma.GetMusMult() + personaje.BonusPAtqBporDefB + (personaje.arma as cJaieiy).DadosNervios;
            (personaje.arma as cJaieiy).ResetNervios();
            if (c.movAgro) numeroDeDados -= 3;
            tirada tr = cDieMath.TirarDados(numeroDeDados, personaje.arma.GetDañoExpl(), c.personajeObjetivo.tieneIraDivina);
            c.daño = cDieMath.sumaDe3Mayores(tr);
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + ", con sus " + personaje.atr.musculo + " en Musculo y multiplicador de " + personaje.arma.GetMusMult() + ", tira " + numeroDeDados + " dados ¡Haciendo " + UIInterface.IntEnNegrita(c.daño) + " de daño!");
            uiC.perCambio = c.personajeObjetivo.nombre;
            c.personajeObjetivo.Daño += c.daño;
        }
    }
}
