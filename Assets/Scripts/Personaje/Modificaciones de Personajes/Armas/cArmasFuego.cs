using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cArmasFuego : cArma
{
    public static new string Descripcion = "Mayor capacidad de daño, que se mete en problemas si no tiene tiempo para recarar.";
    public static new string Reglas = "Multiplicador de Musculo: 0, Base para Matones adicionales: 6. Rango: puede atacar y defender otras zonas, pero tiene -1d al tener enemigos en su misma zona. No tira Daño, si acierta el Ataque contra un Personaje le genera una Herida, más 1 adicional por cada 30 de Daño que ya tuviese. Si no tiene éxito al atacar, defender a otros o detener movimiento, el arma queda descargada y no puede volver a realizar esas acciones hasta usar una acción para cargarla, o hasta que empieze la proxima ronda donde se carga automaticamente.";

    public bool cargada;

    // Start is called before the first frame update
    void Start()
    {
        nombre = "Armas de Fuego";

        musMult = 0;
        dañoExplota = false;
        basePara2doMaton = 6;
        bonusAtaque = 0;
        guardiaMod = -2;
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
