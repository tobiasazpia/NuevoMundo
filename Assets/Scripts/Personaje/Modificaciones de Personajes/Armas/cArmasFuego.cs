using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cArmasFuego : cArma
{
    public bool cargada;

    // Start is called before the first frame update
    void Start()
    {
        musMult = 0;
        dañoExplota = false;
        basePara2doMaton = 6;
        bonusAtaque = 0;
        guardiaMod = 0;
        dadosDelAtacanteMod = 0;
        bonusDefensaPropia = 0;
        bonusDefensaAjena = 0;
        bonusDetenerMovimiento = 0;
        bonusIniciativa = 0;
        deRango = true;

        p.CalcularExtraParaMatones();
        p.CalcularGuardia();

        acciones.Add(gameObject.AddComponent<cAccionAtaqueBasicoFuego>());
        acciones.Add(gameObject.AddComponent<cAccionRecargar>());
        reacciones.Add(gameObject.AddComponent<cReaccionDefensaBasicaFuego>());
        AgregarAccionablesAlPersonaje();
    }

    override public bool AccionesFase0() 
    {
        if (!cargada)
        {
            cargada = true;
            p.uiC.SetText(p.nombre + " carga su arma entre rondas.");
            return false;
        }
        return true;
    }
}
