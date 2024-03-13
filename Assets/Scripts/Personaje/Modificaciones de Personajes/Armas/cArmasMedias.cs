using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cArmasMedias : cArma
{
    public static new string Descripcion = "Arma más balanceada, puede hacer un poco de todo.";
    public static new string Reglas = "Multiplicador de Musculo: 2, Base para Matones adicionales: 9.";

    // Start is called before the first frame update
    void Start()
    {

        nombre = "Armas Medias";
        musMult = 2;
        dañoExplota = true;
        basePara2doMaton = 9;
        bonusAtaque = 0;
        guardiaMod = 0;
        dadosDelAtacanteMod = 0;
        bonusDefensaPropia = 0;
        bonusDefensaAjena = 0;
        bonusDetenerMovimiento = 0;
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
