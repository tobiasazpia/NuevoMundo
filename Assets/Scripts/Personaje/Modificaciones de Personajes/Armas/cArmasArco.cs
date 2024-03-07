using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cArmasArco : cArma
{
    public static new string Descripcion = "Resalta atacando desde lejos, protegiendo aliados y controlando enemigos.";
    public static new string Reglas = "Multiplicador de Musculo: 0, Base para Matones adicionales: 9. Rango: puede atacar y defender otras zonas, pero tiene -1d al tener enemigos en su misma zona. +2d a defender a otros y a detener movimiento. -1d si defiende de o ataca a un enemigo en su misma zona.";
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
