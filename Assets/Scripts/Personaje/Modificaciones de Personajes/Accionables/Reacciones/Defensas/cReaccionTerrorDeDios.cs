using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cReaccionTerrorDeDios : cReaccionDefensa
{
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
        consecuencia = "Hasta el final de la ronda, los dados del objetivo no explotan en sus tiradas de Heridas o se derriba un Matón adicional contra él.";
        reglas = "Terror de Dios: Defensa. ";
        reglas += consecuencia;
        nombre = "Terror de Dios";
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonDTMarcial1");
        icon = c.GetComponent<cIconos>().TerrorDeDios;
    }

    override public int DeterminarNumeroDeDados()
    {
        // es igual a DB basica salvo por lo que sumamos al ndados, capaz juntar
        int numeroDeDados = 3 + personaje.atr.ingenio + personaje.tradicionMarcial[3];
        numeroDeDados += (personaje.arma as cLaVoluntadDelCreador).DadosDeSedDeSangre;
        if (reroleando)
        {
            numeroDeDados += dadosExtrasParaReroll;
        }

        if (!c.atacando)
        {
            numeroDeDados += personaje.arma.GetBonusDetenerMovimiento();
        }
        else
        {
            if (personaje.nombre == c.personajeObjetivo.nombre)
            {
                numeroDeDados += personaje.arma.GetBonusDefensaPropia();
            }
            else
            {
                numeroDeDados += personaje.arma.GetBonusDefensaAjena();
            }
        }
        return numeroDeDados;
    }

    public override void Consecuencias()
    {
        string text;
        string curar = "";
        if (exito)
        {
            PlaySoundEffect();

            if (c.atacando) // esto de curar pasa en todas las defensas
            {
                if (c.personajeObjetivo.Daño > 0)
                {
                    c.personajeObjetivo.Daño = 0;
                    curar = " " + UIInterface.NombreDePersonajeEnNegrita(c.personajeObjetivo) + " puede tomar un respiro y se recupera de su daño.";
                }
            }
            // Lo que hace terro de dios es aplicar un debuff, llamemoslo TERROR
            // TERROR dura hasta el final de la ronda
            // un personaje con TERROR no explota sus dados en sus tiradas de Heridas.
            // un maton con TERROR siempre pierde un maton mas del que deberia
            c.personajeActivo.tieneTerror = true;
            text = "Tuvo éxito y el atacante ";
            if (c.personajeActivo is cMatones) text += " perderá un Maton adicional por cualquier Ataque exitoso";
            else text += " no explotará en sus tiradas de Heridas";
            text += " hasta el final de la ronda.";
            text += curar;
            personaje.GastarDado(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
            c.stateID = cCombate.BUSCANDO_ACCION;
            cEventManager.StartPersonajeActuoEvent(c.personajeActivo);
            c.personajeActivo.GetAccionPorNumero(c.accionActiva).ResetState();
            c.accionActiva = -1;
        }
        else
        {
            c.jugadorDef = defensa;
            if (!c.atacando)
            {
                c.jugadorAtq = c.personajeActivo.GetGuardia();
            }
            personaje.fallamosDefPor = c.jugadorAtq - c.jugadorDef;
            text = "";
            personaje.RetrocederDadoDeAccion(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
            c.stateID = cCombate.RESOLVIENDO_ACCION;
            c.personajeActivo.GetAccionPorNumero(c.accionActiva).intentaronDetenerlo = true;
            c.personajeActivo.GetAccionPorNumero(c.accionActiva).ResetMensaje();
        }
        cEventManager.StartPersonajeActuoEvent(personaje);
        uiC.ActualizarIniciativa(c.personajes);
        acc_state = DB_DETERMINANDO_DADOS - 1;
    }
}
