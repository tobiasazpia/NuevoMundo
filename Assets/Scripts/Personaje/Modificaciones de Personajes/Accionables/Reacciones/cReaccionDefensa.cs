using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cReaccionDefensa : cReacciones
{
    public const int DB_DETERMINANDO_DADOS = 0;
    public const int DB_TIRANDO = 1;
    public const int DB_CONSECUENCIAS = 2;

    public int dadosATirar;
    public int defensa;
    public bool exito;
    public bool pidiendoDrama = false;

    override public void SetUp()
    {
        GetObjets();
        reroleandoState = DB_TIRANDO;
        if (personaje.tieneTradicionMarcial) reroleandoState = DB_DETERMINANDO_DADOS;
        else reroleandoState = DB_TIRANDO;
    }

    override public void Ejecutar()
    {
        switch (acc_state)
        {
            case DB_DETERMINANDO_DADOS:
                uiC.acc = this;
                DeterminadoDados();
                break;
            case DB_TIRANDO:
                Tirando();
                break;
            case DB_CONSECUENCIAS:
                Consecuencias();
                break;
            default:
                break;
        }
        acc_state++;
        if(!pidiendoDrama) c.EsperandoOkOn(true);
    }

    protected void DeterminadoDados()
    {
        string armasImprovisadas = "";
        dadosATirar = DeterminarNumeroDeDados();
        if (!c.atacando)
        {
            if (personaje.GetZonaActual() != c.personajeActivo.GetZonaActual() && personaje.arma is cArmasPelea)
            {
                (personaje.arma as cArmasPelea).PerderArmaImprovisada();
                armasImprovisadas = " " + personaje.nombre + " lanza su arma improvisada.";
            }
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " trata de intervenir con " + nombre + ", tirando " + dadosATirar + " dados contra la guardia de " + UIInterface.NombreDePersonajeEnNegrita(c.personajeActivo) + " de " + c.personajeActivo.GetGuardia() + "." + armasImprovisadas);
        }
        else
        {
            if (personaje.GetZonaActual() != c.personajeActivo.GetZonaActual() && personaje.GetZonaActual() != c.personajeObjetivo.GetZonaActual() && personaje.arma is cArmasPelea)
            {
                (personaje.arma as cArmasPelea).PerderArmaImprovisada();
                armasImprovisadas = " " + UIInterface.NombreDePersonajeEnNegrita(personaje) + " lanza su arma improvisada.";
            }
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " trata de defender con " + nombre + " tirando " + dadosATirar + " dados contra el ataque de " + UIInterface.NombreDePersonajeEnNegrita(c.personajeActivo) + " de " + UIInterface.IntEnNegrita(c.jugadorAtq) + "." + armasImprovisadas);
        }
    }

    virtual protected void Tirando()
    {
        tirada tr = cDieMath.TirarDados(dadosATirar);
        defensa = cDieMath.sumaDe3Mayores(tr);
        c.jugadorDef = defensa;
        string def;
        string resultado;
        uiC.perCambio = c.personajeActivo.nombre;
        if (c.atacando)
        {
            exito = defensa >= c.jugadorAtq;
            //To Do - Jaieiy: decirle al ataque si tuvo exito o no, para que sepa que bonus de Jairiy tiene que aplicar
            //sabes el p activo, del cueal sabes su lista de acciones
            //combate tambien
            // si no, la otra seria no interactaur ocn el ataque, y en toda defensa al determinar el exito, chequear si la escuela del otro es Jaieiy, y si lo es, updatear sus bonus segun corresponda.
            // Esto requeriria tambien updetearlos cuando un ataque falla de una y cuando nadie decide trata de defender
            if (exito)
            {
                resultado = "deteniendo";
                def = UIInterface.IntExitoso(defensa);
            }
            else
            {
                resultado = "no pudiendo detener";
                def = UIInterface.IntFallido(defensa);
            }
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " saca " + def + ", " + resultado + " el ataque de " + UIInterface.IntEnNegrita(c.jugadorAtq) + ".");
        }
        else
        {
            exito = defensa >= c.personajeActivo.GetGuardia();
            if (exito)
            {
                resultado = "deteniendo";
                def = UIInterface.IntExitoso(defensa);
            }
            else
            {
                resultado = "no pudiendo detener";
                def = UIInterface.IntFallido(defensa);
            }
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " saca " + def + ", " + resultado + " el movimiento de " + UIInterface.NombreDePersonajeEnNegrita(c.personajeActivo) + ".");
        }
        if (personaje.Drama && !exito) { uiC.PedirDrama(); pidiendoDrama = true; }
    }

    public virtual void Consecuencias()
    {
        string text = "";
        string curar = "";
        if (exito)
        {
            personaje.c.effect.clip = personaje.c.effectDefensa;
            personaje.c.effect.Play();


            int dif = 0;
            if (c.atacando)
            {
                dif = defensa - c.jugadorAtq;
                if (c.personajeObjetivo.Da�o > 0)
                {
                    c.personajeObjetivo.Da�o = 0;
                    curar = " " + UIInterface.NombreDePersonajeEnNegrita(c.personajeObjetivo) + " puede tomar un respiro y se recupera de su da�o.";
                }
            }
            else dif = defensa - c.personajeActivo.GetGuardia();
            uiC.perCambio = personaje.nombre;
            personaje.BonusPAtqBporDefB += dif;
            text = ("Tuvo �xito por " + dif + ", y tirar� " + UIInterface.IntExitoso(personaje.BonusPAtqBporDefB) + " dados adicionales en su proximo Ataque B�sico y su Da�o.") + curar;
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

    protected void PlaySoundEffect()
    {
        personaje.c.effect.clip = personaje.c.effectDefensa;
        personaje.c.effect.Play();
    }
}