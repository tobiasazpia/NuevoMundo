using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cReaccionNerviosDeAcero : cReaccionDefensa
{
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
        consecuencia = "Por cada Fase desde la fase del Dado de Acción a usar esta reacción, reducí en 1 tu Extra para Matones durante tu próximo Ataque, o +1d en tu próxima tirada de Daño.";
        reglas = "Nervios de Acero: Defensa. ";
        reglas += consecuencia;
        nombre = "Nervios de Acero"; 
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonDTMarcial1");
        Debug.Log("start acero");
        Debug.Log(boton);
        icon = c.GetComponent<cIconos>().NerviosDeAcero;
    }

    override public int DeterminarNumeroDeDados()
    {
        // es igual a DB basica salvo por lo que sumamos al ndados, capaz juntar
        int numeroDeDados = 3 + personaje.atr.ingenio + personaje.tradicionMarcial[3];
        if (personaje.arma.maestria > 4) numeroDeDados += (personaje.arma as cJaieiy).DadosMaestro;

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

            int valorNervios = 0;
            for (int i = 0; i < c.acciones.Count; i++)
            {
                if (c.acciones[i].per.nombre == personaje.nombre && c.acciones[i].fase <= c.faseActual && c.acciones[i].fase > 0)
                {
                    valorNervios = c.faseActual - c.acciones[i].fase;
                    Debug.Log("faseActual " + c.faseActual);
                    Debug.Log("fase " + c.acciones[i].fase);
                    break;
                }
            }
            (personaje.arma as cJaieiy).DadosNervios += valorNervios;

            text = "Tuvo éxito para su próximo ataque tendrá " + (personaje.arma as cJaieiy).DadosNervios + " menos en su extra, o +" + (personaje.arma as cJaieiy).DadosNervios + "d de daño.";
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

