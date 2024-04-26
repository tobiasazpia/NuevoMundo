using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionAtaqueBasicoImprovisadas : cAccionAtaqueBasico
{
    new void Start()
    {
        GetObjets();
        nombre = "Ataque Básico Improvisado";
        consecuencia = "Tratás de dañar al oponente con tu Arma Improvisada.";
        reglas = nombre + ": Ataque. " + consecuencia;
        categoria = cAcciones.AC_CAT_MARCIAL;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonAtacarImpro");
        icon = c.GetComponent<cIconos>().Improvisada;
    }

    override public int DeterminarNumeroDeDados()
    {
        armaImpro();
        return base.DeterminarNumeroDeDados();
    }

    override protected void armaImpro()
    {
        (personaje.arma as cArmasPelea).ActualizarDataDeImprovisada(true);
    }

    override public void RevisarLegalidad()
    {
        esLegal = c.HayEnemigosVivosEnRango() && (personaje.arma as cArmasPelea).armaImprovisadaActiva;
    }

    override protected void Tirando()
    {
        Debug.Log("tirando ara arma imrpov");
        Debug.Log("Mus ult: " + personaje.arma.musMult);
        Debug.Log("bonus a: " + personaje.arma.bonusAtaque);
        Debug.Log("explota: " + personaje.arma.dañoExplota);
        mostrarMensaje1 = true;
        tirada tr = cDieMath.TirarDados(dadosATirar);
        c.jugadorAtq = cDieMath.sumaDe3Mayores(tr);
        string resultado;
        string pierdeArma = "";
        ataqueExitoso = c.jugadorAtq >= c.personajeObjetivo.GetGuardia();
        string atq;
        if (ataqueExitoso)
        {
            atq = UIInterface.IntExitoso(c.jugadorAtq);
            resultado = "acierta";
            LlenarReaccionesPosibles();
            pierdeArma = " Su arma improvisada se destruye en el impacto.";
            (personaje.arma as cArmasPelea).PerderArmaImprovisada();
        }
        else
        {
            atq = UIInterface.IntFallido(c.jugadorAtq);
            resultado = "fallando";
            if (personaje.GetZonaActual() != c.personajeObjetivo.GetZonaActual())
            {
                pierdeArma = " Pierde su arma improvisada por usarla a rango.";
                (personaje.arma as cArmasPelea).PerderArmaImprovisada();
            }
            acc_state = AB_TERMINADO - 1;
        }
        uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(c.personajeActivo) + " saca " + atq + ", " + resultado + " el ataque contra la guardia de " + UIInterface.IntEnNegrita(c.personajeObjetivo.GetGuardia()) + " de " + UIInterface.NombreDePersonajeEnNegrita(c.personajeObjetivo) + "." + pierdeArma);
        if (personaje.Drama && !ataqueExitoso) uiC.PedirDrama();
    }

    override protected void DeterminadoDados()
    {
        c.atacando = true;
        intentaronDetenerlo = false;
        c.jugadorDef = 0;

        dadosATirar = DeterminarNumeroDeDados();

        string text = "¡" + UIInterface.NombreDePersonajeEnNegrita(personaje) + " usa su " + nombre + " contra " + UIInterface.NombreDePersonajeEnNegrita(c.personajeObjetivo) + "! Tira " + dadosATirar + " dados contra su guardia de " + UIInterface.IntEnNegrita(c.personajeObjetivo.GetGuardia()) + ".";
        if (c.movAgro)
        {
            uiC.SetText(text);
        }
        else
        {
            c.personajeActivo.GastarDado(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
            uiC.ActualizarIniciativa(c.personajes);
        }
    }
}
