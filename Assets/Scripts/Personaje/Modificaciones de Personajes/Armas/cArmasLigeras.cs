using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cArmasLigeras : cArma
{
    public List<string> personajesQueActuaron;
    // Start is called before the first frame update
    void Start()
    {
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
        if (!personajesQueActuaron.Contains(perQueActuo.nombre)) personajesQueActuaron.Add(perQueActuo.nombre);
    }

    private void NadieActuo()
    {
        personajesQueActuaron.Clear();
    }

    public bool perYaActuo(cPersonaje per)
    {
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
