using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cReaccionDefensaBasica : cReaccionDefensa
{
    public const int DB_DETERMINANDO_DADOS = 0;
    public const int DB_TIRANDO = 1;
    public const int DB_CONSECUENCIAS = 2;

    public int dadosATirar;
    public int defensa;
    public bool exito;

    // Start is called before the first frame update
    void Start()
    {
        GetObjets();
        nombre = "Defensa Basica";
        reroleandoState = DB_TIRANDO;
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
        c.EsperandoOkOn(true);
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
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " trata de intervenir con " + dadosATirar + " dados contra la guardia de " + UIInterface.NombreDePersonajeEnNegrita(c.personajeActivo) + " de " + c.personajeActivo.GetGuardia() + "." + armasImprovisadas);
        }
        else
        {
            if (personaje.GetZonaActual() != c.personajeActivo.GetZonaActual() && personaje.GetZonaActual() != c.personajeObjetivo.GetZonaActual() && personaje.arma is cArmasPelea)
            {
                (personaje.arma as cArmasPelea).PerderArmaImprovisada();
                armasImprovisadas = " " + UIInterface.NombreDePersonajeEnNegrita(personaje) + " lanza su arma improvisada.";
            }
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " trata de defender con " + dadosATirar + " dados contra el ataque de " + UIInterface.NombreDePersonajeEnNegrita(c.personajeActivo) + " de " + UIInterface.IntEnNegrita(c.jugadorAtq) +"." + armasImprovisadas);
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
        c.personajeActivo.BonusPAtqBporDefB = 0;
        if (c.atacando)
        {
            exito = defensa >= c.jugadorAtq;
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
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " saca " + def + ", " + resultado + " el ataque de " +  UIInterface.IntEnNegrita(c.jugadorAtq) + ".");
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
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " saca " + def + ", " + resultado + " el movimiento de " + UIInterface.NombreDePersonajeEnNegrita(c.personajeActivo) +".");
        }
        if (personaje.Drama && !exito) uiC.PedirDrama();
    }

    public void Consecuencias()
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
