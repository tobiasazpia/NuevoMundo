using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAccionAtaqueBasicoVoluntadDelCreador : cAccionAtaqueBasico
{
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    protected override void TiramosDaño()
    {
        bool veterano = (personaje.arma as cLaVoluntadDelCreador).maestria > 2;
        const int bonusDeVoluntad = 1;
        if (!(c.personajeObjetivo is cMatones))
        {
            int numeroDeDados = 3 + personaje.atr.musculo * personaje.arma.GetMusMult() + personaje.BonusPAtqBporDefB + bonusDeVoluntad + (personaje.arma as cLaVoluntadDelCreador).DadosDeSedDeSangre - personaje.impuesto;
            if (c.movAgro) numeroDeDados -= 3;
            tirada tr;
            if (veterano) tr = cDieMath.TirarDadosDobleExplosion(numeroDeDados, c.personajeObjetivo.tieneIraDivina);
            else tr = cDieMath.TirarDados(numeroDeDados,personaje.arma.GetDañoExpl(),c.personajeObjetivo.tieneIraDivina);
            c.daño = cDieMath.sumaDe3Mayores(tr);
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + ", con sus " + personaje.atr.musculo + " en Musculo y multiplicador de " + personaje.arma.GetMusMult() + ", tira " + numeroDeDados + " dados ¡Haciendo " + UIInterface.IntEnNegrita(c.daño) + " de daño!");
            uiC.perCambio = c.personajeObjetivo.nombre;
            c.personajeObjetivo.Daño += c.daño;
            //if (personaje.drama) uiC.PedirDrama();
        }
    }

    override public int DeterminarNumeroDeDados()
    {
        personaje.totalDadosDelAtacante = personaje.dadosDelAtacantePorPrecavido + personaje.arma.GetDadosDelAtacanteMod();
        int numeroDeDados = 3 + personaje.atr.maña + personaje.tradicionMarcial[0] + personaje.arma.GetBonusAtaque() + personaje.BonusPAtqBporDefB + c.personajeObjetivo.totalDadosDelAtacante + (personaje.arma as cLaVoluntadDelCreador).DadosDeSedDeSangre;
        if (c.movAgro) numeroDeDados -= 3;
        if (reroleando)
        {
            Debug.Log("reroll true, sumamos");
            numeroDeDados += dadosExtrasParaReroll;
        }else Debug.Log("reroll false, no sumamos");
        return numeroDeDados;
    }
}
