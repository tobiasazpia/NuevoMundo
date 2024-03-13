using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cArmasLigeras : cArma
{
    public static new string Descripcion = "Autosuficiente y veloz, le gusta actuar antes que los demas.";
    public static new string Reglas = "Multiplicador de Musculo: 1, Base para Matones adicionales: 9. +2d a la Inicitiva, +1 al Defenderse a si mismo, -1d a defender a otros y a detener movimiento. +1d al actuar contra quien todavia lo haya hecho, o al reaccionar contra quien este actuando por primera vez esta ronda.";

    public List<string> personajesQueActuaron;
    // Start is called before the first frame update
    void Start()
    {
        nombre = "Armas Ligeras";
        musMult = 1;
        dañoExplota = true;
        basePara2doMaton = 9;
        bonusAtaque = 0;
        guardiaMod = 0;
        dadosDelAtacanteMod = 0;
        bonusDefensaPropia = 1;
        bonusDefensaAjena = -1;
        bonusDetenerMovimiento = -1;
        bonusIniciativa = 2;
        deRango = false;

        personajesQueActuaron = new List<string>();
        

        p.CalcularExtraParaMatones();
        p.CalcularGuardia();

        acciones.Add(gameObject.AddComponent<cAccionAtaqueBasicoLigeras>());
        reacciones.Add(gameObject.AddComponent<cReaccionDefensaBasicaLigeras>());
        acciones.Add(gameObject.AddComponent<cAccionMovimientoAgresivo>());
        AgregarAccionablesAlPersonaje();

        cEventManager.PersonajeActuoEvent += PersonajeActuo;
        cEventManager.FinDeRondaEvent += NadieActuo;
        //falta agregar bonus por actuar temprano
    }

    private void PersonajeActuo(cPersonaje perQueActuo)
    {
        if (!personajesQueActuaron.Contains(perQueActuo.nombre))
        {
            personajesQueActuaron.Add(perQueActuo.nombre);
            Debug.Log(perQueActuo.nombre + " actuo.");
        }
        Debug.Log("personajes que ya actuaron:");
        foreach (var item in personajesQueActuaron)
        {
            Debug.Log(item);
        }
    }

    private void NadieActuo()
    {
        Debug.Log("Vaciamos pers que ya actuaron");
        personajesQueActuaron.Clear();
    }

    public bool perYaActuo(cPersonaje per)
    {
        Debug.Log("personajes que ya actuaron:");
        foreach (var item in personajesQueActuaron)
        {
            Debug.Log(item);
        }
        return personajesQueActuaron.Contains(per.nombre);
    }

    private void OnDisable()
    {
        cEventManager.PersonajeActuoEvent -= PersonajeActuo;
        cEventManager.FinDeRondaEvent -= NadieActuo;
    }
}
//+1d en todas las tiradas de Ataque, Acción y Defensa
//contra enemigos que esten actuando por primera vez esta ronda, o que todavia no lo hayan hecho
