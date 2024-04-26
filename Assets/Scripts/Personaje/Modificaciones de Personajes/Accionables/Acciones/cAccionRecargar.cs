using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionRecargar : cAcciones
{
    // Start is called before the first frame update
    void Start()
    {
        GetObjets();
        nombre = "Recargar";
        consecuencia = "Tu arma de fuego pasa a estar cargada, y podes volver a Atcar, Defender a otros, y Detener Movimiento.";
        reglas = nombre + ": Especial sin tirar. " + consecuencia;

        categoria = cAcciones.AC_CAT_MARCIAL;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonRecargar");
        icon = c.GetComponent<cIconos>().Recargar;
    }

    override public void Ejecutar()
    {
        (personaje.arma as cArmasFuego).cargada = true;
        string text = UIInterface.NombreDePersonajeEnNegrita(personaje) + " se toma un segundo para recargar su arma.";
        c.personajeActivo.GastarDado(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
        uiC.ActualizarIniciativa(c.personajes);
        c.EsperandoOkOn(true);
        c.stateID = cCombate.BUSCANDO_ACCION;
    }

    override public void RevisarLegalidad()
    {
        if (personaje.arma is cArmasFuego)
        {
            esLegal = !(personaje.arma as cArmasFuego).cargada;
        }
        else esLegal = false;
    }
}
