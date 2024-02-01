using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cArmasPesadas : cArma
{
    // Start is called before the first frame update
    void Start()
    {
        musMult = 3;
        dañoExplota = true;
        basePara2doMaton = 9;
        bonusAtaque = 0;
        guardiaMod = -2;
        dadosDelAtacanteMod = 0;
        bonusDefensaPropia = 0;
        bonusDefensaAjena = 0;
        bonusDetenerMovimiento = 2;
        bonusIniciativa = 0;
        deRango = false;

        p.CalcularExtraParaMatones();
        p.CalcularGuardia();

        acciones.Add(gameObject.AddComponent<cAccionAtaqueBasico>());
        reacciones.Add(gameObject.AddComponent<cReaccionDefensaBasica>());
        acciones.Add(gameObject.AddComponent<cAccionMovimientoAgresivo>());
        AgregarAccionablesAlPersonaje();

    }
}
