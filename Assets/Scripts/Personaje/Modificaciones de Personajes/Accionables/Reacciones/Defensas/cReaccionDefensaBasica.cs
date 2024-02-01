using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cReaccionDefensaBasica : cReaccionDefensa
{
    public const int DB_DETERMINANDO_DADOS = 0;
    public const int DB_TIRANDO = 1;
    public const int DB_CONSECUENCIAS = 2;

    public int db_state;
    public int dadosATirar;
    public int defensa;
    public bool exito;

    // Start is called before the first frame update
    void Start()
    {
        GetObjets();
        nombre = "Defensa Basica";
    }

    override public void Ejecutar()
    {
        Debug.Log("ejecutra defensa basica");
        switch (db_state)
        {
            case DB_DETERMINANDO_DADOS:
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
        db_state++;
        c.EsperandoOkOn(true);
    }

    protected void DeterminadoDados()
    {
        string armasImprovisadas = "";
        dadosATirar = DeterminarNumeroDeDados();
        if (!c.atacando)
        {
            if (personaje.zonaActual != c.personajeActivo.zonaActual && personaje.arma is cArmasPelea)
            {
                (personaje.arma as cArmasPelea).PerderArmaImprovisada();
                armasImprovisadas = " " + personaje.nombre + " lanza su arma improvisada.";
            }
            uiC.SetText(personaje.nombre + " trata de intervenir con " + dadosATirar + " dados contra la guardia de " + c.personajeActivo.nombre + " de " + c.personajeActivo.GetGuardia() + "." + armasImprovisadas);
        }
        else
        {
            if (personaje.zonaActual != c.personajeActivo.zonaActual && personaje.zonaActual != c.personajeObjetivo.zonaActual && personaje.arma is cArmasPelea)
            {
                (personaje.arma as cArmasPelea).PerderArmaImprovisada();
                armasImprovisadas = " " + personaje.nombre + " lanza su arma improvisada.";
            }
            uiC.SetText(personaje.nombre + " trata de defender con " + dadosATirar + " dados contra el ataque de " + c.personajeActivo.nombre + " de " + c.jugadorAtq +"." + armasImprovisadas);
        }
    }

    virtual protected void Tirando()
    {
        Debug.Log("tirando");
        tirada tr = cDieMath.TirarDados(dadosATirar);
        defensa = cDieMath.sumaDe3Mayores(tr);
        string resultado;
        c.personajeActivo.bonusPAtqBporDefB = 0;
        if (c.atacando)
        {
            if (defensa >= c.jugadorAtq)
            {
                exito = true;
                resultado = "deteniendo";
            }
            else
            {
                exito = false;
                resultado = "no pudiendo detener";
            }
            uiC.SetText(personaje.nombre + " saca " + defensa + ", " + resultado + " el ataque de " + c.jugadorAtq + ".");
        }
        else
        {
            if (defensa >= c.personajeActivo.GetGuardia())
            {
                resultado = "deteniendo";
                exito = true;
            }
            else
            {
                exito = false;
                resultado = "no pudiendo detener";
            }
            uiC.SetText(personaje.nombre + " saca " + defensa + ", " + resultado + " el movimiento de " + c.personajeActivo.nombre +".");
        }
    }

    public void Consecuencias()
    {
        Debug.Log("consecuencias");
        string text = "";
        if (exito)
        {
            int dif = 0;
            if(c.atacando) dif = defensa - c.jugadorAtq;
            else dif = defensa - c.personajeActivo.GetGuardia();
            personaje.bonusPAtqBporDefB += dif;
            text = ("Tuvo exito por " + dif + ", y tirara " + personaje.bonusPAtqBporDefB + " dados adicionales en su proximo Ataque Basico y su Daño.");
            personaje.GastarDado(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
            c.stateID = cCombate.BUSCANDO_ACCION;
            c.personajeActivo.GetAccionPorNumero(c.accionActiva).ResetState();
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
        uiC.ActualizarIniciativa(c.personajes);
        db_state = DB_DETERMINANDO_DADOS - 1;
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
