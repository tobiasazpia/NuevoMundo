using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cArmasPesadas : cArma
{
    public static new string Descripcion = "Destaca en el frente del combate, pero depende de su Músculo y es fácil de atacar.";
    public static new string Reglas = "Multiplicador de Musculo: 3, Base para Matones adicionales: 9. +2d a detener movimiento. -2 a tu Guardia.";
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
