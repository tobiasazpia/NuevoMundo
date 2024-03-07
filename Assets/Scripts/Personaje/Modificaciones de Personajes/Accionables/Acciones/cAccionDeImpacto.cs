using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAccionDeImpacto : cAcciones
{
    //Las Acciones de Impacto son todas las Acciones a las que otros pueden reaccionar y que despiertan a personajes guardando
    // incluye acciones especiales, moviminetos y ataques, pero no guardar, recargar o encontrar arma improvisada
    public List<cPersonaje> posiblesReacciones;
    public bool mostrarMensaje1;
    public bool mostrarMensaje2;

    virtual protected int Defensas(int state)
    {
        if (mostrarMensaje1)
        {
            uiC.SetText("¿Intentará detenerlo alguien?");
            mostrarMensaje1 = false;
            mostrarMensaje2 = false;
        }

        if (posiblesReacciones.Count > 0 && !intentaronDetenerlo)
        {
            state--;
            c.personajeInterversor = posiblesReacciones[0];
            posiblesReacciones.RemoveAt(0);
            c.stateID = cCombate.PREGUNTANDO_REACCION;
        }
        else
        {
            posiblesReacciones.Clear();
            uiC.DejarDePedirReaccion();
            if (mostrarMensaje2)
            {
                MensajeDeExito();
                if (c.personajeObjetivo is cMatones) state++; //Saltear Daño
                mostrarMensaje1 = true;
            }
            else
            {
                Debug.Log("no mostarmos m");
                state--; //quedarse un cacho para mostrar mensaje
                mostrarMensaje2 = true;
            }
        }
        return state;
    }

    virtual protected void MensajeDeExito()
    {
        uiC.SetText("Error: No Override");
    }
    //protected void LlenarReaccionesPosibles()
    //{
    //    posiblesReacciones = new List<cPersonaje>();
    //    List<cPersonaje> posiblesReaccionesSinObjetivo = new List<cPersonaje>();
    //    // sacamos lo de acelerar reacciones
    //    if (c.personajeObjetivo.reaccion1Disponible) posiblesReacciones.Add(c.personajeObjetivo); // si el objetivo tiene reacciones disponibles, puede reaccionar, y esta primero
    //    foreach (var p in c.personajes)
    //    {
    //        if (p.equipo != personaje.equipo && p.nombre != c.personajeObjetivo.nombre && p.reaccion1Disponible) //si tien
    //        {
    //            if (p.arma.GetDeRango()) //si es rango, y la zona de la que proviene el ataque esta en rango
    //            {
    //                Debug.Log("de rango en llenando reacciones");
    //                if (p.arma is cArmasFuego)
    //                {
    //                    if (c.ZonaEsteEnRangoDePersonaje(p, c.personajeActivo.GetZonaActual()) && (p.arma as cArmasFuego).cargada) posiblesReaccionesSinObjetivo.Add(p);
    //                }
    //                else
    //                {
    //                    if (c.ZonaEsteEnRangoDePersonaje(p, c.personajeActivo.GetZonaActual())) posiblesReaccionesSinObjetivo.Add(p);
    //                }
    //            }
    //            else if (p.GetZonaActual() == c.personajeActivo.GetZonaActual() || p.GetZonaActual() == c.personajeObjetivo.GetZonaActual())// si es melee, y su zona es igual a la zona del objetivo o del atacante
    //            {

    //                posiblesReaccionesSinObjetivo.Add(p);
    //            }
    //        }
    //    }
    //    posiblesReaccionesSinObjetivo.Sort(CompararPersonajesPorPrioridad);
    //    posiblesReacciones.AddRange(posiblesReaccionesSinObjetivo);
    //    string text = "";
    //    foreach (var item in posiblesReacciones)
    //    {
    //        text += item.nombre + "  ";
    //    }
    //    Debug.Log("posibles reacciones sorteadas: " + text);
    //    if (posiblesReacciones.Count > 0)
    //    {
    //        c.personajeInterversor = posiblesReacciones[0];
    //        c.posibleReaccion = true;
    //    }
    //    else c.posibleReaccion = false;
    //}
}
