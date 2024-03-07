using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Si se llamo esta accion es porque el arma esta cargada, no ca a ser una posibilidad atacar si no

public class cAccionAtaqueBasicoFuego : cAccionAtaqueBasico
{
    override public int DeterminarNumeroDeDados()
    {
        int numeroDeDados = base.DeterminarNumeroDeDados();
        if (c.HayEnemigosEnMelee(personaje))
        {
            numeroDeDados -= 1;
        }
        return numeroDeDados;
    }

    override public void RevisarLegalidad()
    {
        esLegal = c.HayEnemigosVivosEnRango() && (personaje.arma as cArmasFuego).cargada;
    }

    override protected void Defensas()
    {
        if (mostrarMensaje1)
        {
            uiC.SetText("¿Intentara detenerlo alguien?");
            mostrarMensaje1 = false;
            //mostrarMensaje2 = false;
        }

        if (posiblesReacciones.Count > 0 && !intentaronDetenerlo)
        {
            acc_state--; // Seguimos preguntando
            c.personajeInterversor = posiblesReacciones[0];
            posiblesReacciones.RemoveAt(0);
            c.stateID = cCombate.PREGUNTANDO_REACCION;
        }
        else
        {
            posiblesReacciones.Clear();
            uiC.DejarDePedirReaccion();
            //if (mostrarMensaje2)
            //{
                uiC.SetText("¡Nadie detuvo el ataque, da en blanco! " + UIInterface.NombreDePersonajeEnNegrita(personaje) + " aprovecha para recargar su arma.");
                acc_state++; //Saltear Daño siempre (unica diferencia con ataque basico normal)
                (personaje.arma as cArmasFuego).cargada = true;
                mostrarMensaje1 = true;
            //}
            //else
            //{
            //    Debug.Log("no mostarmos m");
            //    ab_state--; //quedarse un cacho para mostrar mensaje
            //    mostrarMensaje2 = true;
            //}
        }
    }

    override protected void DeterminadoDados()
    {
        c.atacando = true;
        intentaronDetenerlo = false;
        c.jugadorDef = 0;
        dadosATirar = DeterminarNumeroDeDados();
        string text = "¡" + UIInterface.NombreDePersonajeEnNegrita(personaje) + " usa su " + nombre + " contra " + UIInterface.NombreDePersonajeEnNegrita(c.personajeObjetivo) + "! Tira " + dadosATirar + " dados contra su guardia de " + UIInterface.IntEnNegrita(c.personajeObjetivo.GetGuardia()) + ", descargando su arma.";
        (personaje.arma as cArmasFuego).cargada = false;
        c.personajeActivo.GastarDado(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
        uiC.ActualizarIniciativa(c.personajes);
    }

    override protected string NoSuperamosGuardia()
    {
        textoAdicional = " Su arma queda descargada.";
        acc_state = AB_TERMINADO - 1;
        return "fallando";
    }
}
