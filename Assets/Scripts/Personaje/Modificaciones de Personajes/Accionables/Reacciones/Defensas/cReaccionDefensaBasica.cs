using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cReaccionDefensaBasica : cReaccionDefensa
{
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
        nombre = "Defensa Basica";
    }

    protected override void Tirando()
    {
        base.Tirando();
        c.personajeActivo.BonusPAtqBporDefB = 0;      //what? que es esto
    }

    public override void Consecuencias()
    {
        string text = "";
        string curar = "";
        if (exito)
        {
            
            int dif = 0;
            if (c.atacando) { 
                dif = defensa - c.jugadorAtq;
                if(c.personajeObjetivo.Daño > 0)
                {
                    c.personajeObjetivo.Daño = 0;
                    curar = " " + UIInterface.NombreDePersonajeEnNegrita(c.personajeObjetivo) + " puede tomar un respiro y se recupera de su daño.";
                }
            }
            else dif = defensa - c.personajeActivo.GetGuardia();
            uiC.perCambio = personaje.nombre;
            personaje.BonusPAtqBporDefB += dif;
            text = ("Tuvo éxito por " + dif + ", y tirará " + UIInterface.IntExitoso(personaje.BonusPAtqBporDefB) + " dados adicionales en su proximo Ataque Básico y su Daño.") + curar;
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
            //el problema de esto es que vuleve al ataque basico, que va a seguir chequeando defensas. el chequeo que usa AB es una lista de reacciones posibles que va vaciando... se podria vaciar desde aca? o como le decimos que entre en "Nadie defnedio"? capaz tambien podramos cambiar su state aca, y que esa seccion en defensas sea solo para decir que nadie intento defender. de todas formas
            c.personajeActivo.GetAccionPorNumero(c.accionActiva).intentaronDetenerlo = true;
            c.personajeActivo.GetAccionPorNumero(c.accionActiva).ResetMensaje();
        }
        cEventManager.StartPersonajeActuoEvent(personaje);
        uiC.ActualizarIniciativa(c.personajes);
        acc_state = DB_DETERMINANDO_DADOS - 1;
    }

    override public int DeterminarNumeroDeDados()
    {
        int numeroDeDados = 3 + personaje.atr.ingenio + personaje.hab.defensaBasica;

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
}