using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cArmasArco : cArma
{
    // Start is called before the first frame update
    void Start()
    {
        musMult = 0;
        dañoExplota = true;
        basePara2doMaton = 9;
        bonusAtaque = 0;
        guardiaMod = 0;
        dadosDelAtacanteMod = 0;
        bonusDefensaPropia = 0;
        bonusDefensaAjena = 2;
        bonusDetenerMovimiento = 2;
        bonusIniciativa = 0;
        deRango = true;
        p.CalcularExtraParaMatones();
        p.CalcularGuardia();
        acciones.Add(gameObject.AddComponent<cAccionAtaqueBasicoArco>());
        reacciones.Add(gameObject.AddComponent<cReaccionDefensaBasicaArco>());
        AgregarAccionablesAlPersonaje();
    }
}
