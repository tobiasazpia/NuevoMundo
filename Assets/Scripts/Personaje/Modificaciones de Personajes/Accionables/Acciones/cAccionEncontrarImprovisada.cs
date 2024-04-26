using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionEncontrarImprovisada : cAcciones
{
    const string ABImproTooltipBase = "Ataque Básico con Arma Improvisada";
    const string MovImproTooltipBase = "Carga con Arma Improvisada";
    const string DBImproTooltipBase = "Defensa Básica con Arma Improvisada";

    protected void Start()
    {
        GetObjets();
        nombre = "Encontrar Improvisada";
        consecuencia = "Conseguís un arma improvisada de un tamaño al azar.";
        reglas = nombre + ": Especial sin tirar. " + consecuencia;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonConseguirImpro");
        icon = c.GetComponent<cIconos>().Encontrar;
    }

    override public void Ejecutar()
    {
        int armaI = Random.Range(0, 3);
        string armaINombre = "";
        switch (armaI)
        {
            case 0:
                armaINombre = "arma improvisada <b>pequeña</b>";
                break;
            case 1:
                armaINombre = "arma improvisada <b>mediana</b>";
                break;
            case 2:
                armaINombre = "arma improvisada <b>grande</b>";
                break;
            default:
                break;
        }
        string text = (UIInterface.NombreDePersonajeEnNegrita(personaje) + " busca un arma improvisada, y encuentra un " + armaINombre + ".");

        uiC.MostrarArmaEnTooltip(ABImproTooltipBase, DBImproTooltipBase, MovImproTooltipBase,armaI);
        (personaje.arma as cArmasPelea).AdquirirArmaImprovisada(armaI);
        c.EsperandoOkOn(true);
        c.stateID = cCombate.BUSCANDO_ACCION;
        c.personajeActivo.GastarDado(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
        uiC.ActualizarIniciativa(c.personajes);
    }

    override public void RevisarLegalidad()
    {
        esLegal = !(personaje.arma as cArmasPelea).armaImprovisadaActiva;
    }
}
