using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionAtaqueBasico : cAccionAtaque
{
    public const int AB_DETERMINANDO_DADOS = 0;
    public const int AB_TIRANDO = 1;
    public const int AB_DEFENSAS = 2;
    public const int AB_DA�O = 3;
    public const int AB_HERIDAS = 4;
    public const int AB_TERMINADO = 5;
    public const int AB_ERROR = -1;

    public int dadosATirar;

    protected string textoAdicional = "";

    protected void Start()
    {
        GetObjets();
        nombre = "Ataque Basico";
        categoria = cAcciones.AC_CAT_MARCIAL;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonAtacar");
        reroleandoState = AB_TIRANDO;
    }

    override public void RevisarLegalidad()
    {

        esLegal = c.HayEnemigosEnMelee(personaje);
    }

    override public void Ejecutar()
    {
        Debug.Log("state " + acc_state);
        switch (acc_state)
        {
            case AB_DETERMINANDO_DADOS:
                //Debug.Log("ab_state 0");
                uiC.acc = this;
                DeterminadoDados();
                break;
            case AB_TIRANDO:
                //Debug.Log("ab_state 1");
                Tirando();
                break;
            case AB_DEFENSAS:
                //Debug.Log("ab_state 2");
                Defensas();
                break;
            case AB_DA�O:
                //Debug.Log("ab_state 3");
                Da�o();
                break;
            case AB_HERIDAS:
                //Debug.Log("ab_state 4");
                Heridas();
                break;
            case AB_TERMINADO:
                //Debug.Log("ab_state 5");
                Terminado();
                break;
            case AB_ERROR:
                //Debug.Log("ab_state 6");
                break;
            default:
                break;
        }
        acc_state++;
        //Debug.Log("despues de ++ ab_state es:" + ab_state);
        //switch (ab_state)
        //{
        //    case AB_DETERMINANDO_DADOS:
        //        Debug.Log("ab_state 0");
        //        break;
        //    case AB_TIRANDO:
        //        Debug.Log("ab_state 1");
        //        break;
        //    case AB_DEFENSAS:
        //        Debug.Log("ab_state 2");
        //        break;
        //    case AB_DA�O:
        //        Debug.Log("ab_state 3");
        //        break;
        //    case AB_HERIDAS:
        //        Debug.Log("ab_state 4");
        //        break;
        //    case AB_TERMINADO:
        //        Debug.Log("ab_state 5");
        //        break;
        //    case AB_ERROR:
        //        Debug.Log("ab_state -1");
        //        break;
        //    default:
        //        break;
        //}
        //c.EsperandoOkOn(true);
    }

    virtual protected void DeterminadoDados()
    {
        Debug.Log("zona de atacante: " + personaje.GetZonaActual());
        Debug.Log("zona de per activo: " + c.personajeActivo.GetZonaActual());
        Debug.Log("zona de objetivo: " + c.personajeObjetivo.GetZonaActual());
        c.atacando = true;
        intentaronDetenerlo = false;    
        c.jugadorDef = 0;

        string armasImprovisadas = "";
        if(personaje.arma is cArmasPelea)
        {
            if((personaje.arma as cArmasPelea).armaImprovisadaActiva)
            {
                (personaje.arma as cArmasPelea).PerderArmaImprovisada();               
                armasImprovisadas = " Suelta su arma improvisada.";
            }
            armaImpro();
        }

        dadosATirar = DeterminarNumeroDeDados();

        string text = "�" + personaje.nombre + " usa su " + nombre + " contra " + c.personajeObjetivo.nombre + "! Tira " + dadosATirar + " dados contra su guardia de " + c.personajeObjetivo.GetGuardia() + "." + armasImprovisadas;
        if (c.movAgro) {
            uiC.SetText(text);
        }
        else
        {
            if (!reroleando)
            {
                c.personajeActivo.GastarDado(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
                uiC.ActualizarIniciativa(c.personajes);
            }
        }
        reroleando = false;
    }

    virtual protected void armaImpro()
    {
        Debug.Log("arma imrpvo basico");
        (personaje.arma as cArmasPelea).ActualizarDataDeImprovisada(false);
    }

    virtual protected void Tirando()
    {
        mostrarMensaje1 = true;
        tirada tr = cDieMath.TirarDados(dadosATirar);
        c.jugadorAtq = cDieMath.sumaDe3Mayores(tr);
        string resultado;
        textoAdicional = "";
        if (c.jugadorAtq >= c.personajeObjetivo.GetGuardia())
        {
            resultado = SuperamosGuardia();
        }
        else
        {
            resultado = NoSuperamosGuardia();
        }
        uiC.SetText(c.personajeActivo.nombre + " saca " + c.jugadorAtq + ", " + resultado + " el ataque contra la guardia de " + c.personajeObjetivo.GetGuardia() + " de " + c.personajeObjetivo.nombre + "." + textoAdicional);
        if (personaje.drama) uiC.PedirDrama();
    }

    virtual protected string SuperamosGuardia()
    {
        LlenarReaccionesPosibles();
        return "acierta";
    }

    virtual protected string NoSuperamosGuardia()
    {
        acc_state = AB_TERMINADO - 1;
        return "fallando";
    }


    protected void LlenarReaccionesPosibles()
    {
        posiblesReacciones = new List<cPersonaje>();
        List<cPersonaje> posiblesReaccionesSinObjetivo = new List<cPersonaje>();
        // sacamos lo de acelerar reacciones
        if (c.personajeObjetivo.reaccion1Disponible) posiblesReacciones.Add(c.personajeObjetivo); // si el objetivo tiene reacciones disponibles, puede reaccionar, y esta primero
        foreach (var p in c.personajes)
        {
            if (p.equipo != personaje.equipo && p.nombre != c.personajeObjetivo.nombre && p.reaccion1Disponible) //si tien
            {
                if (p.arma.GetDeRango()) //si es rango, y la zona de la que proviene el ataque esta en rango
                {
                    Debug.Log("de rango en llenando reacciones");
                    if (p.arma is cArmasFuego)
                    {
                        if(c.ZonaEsteEnRangoDePersonaje(p, c.personajeActivo.GetZonaActual()) && (p.arma as cArmasFuego).cargada) posiblesReaccionesSinObjetivo.Add(p);
                    }
                    else
                    {
                        if (c.ZonaEsteEnRangoDePersonaje(p, c.personajeActivo.GetZonaActual())) posiblesReaccionesSinObjetivo.Add(p);
                    }
                }
                else if (p.GetZonaActual() == c.personajeActivo.GetZonaActual() || p.GetZonaActual() == c.personajeObjetivo.GetZonaActual())// si es melee, y su zona es igual a la zona del objetivo o del atacante
                {

                    posiblesReaccionesSinObjetivo.Add(p);
                }
            }
        }
        posiblesReaccionesSinObjetivo.Sort(CompararPersonajesPorPrioridad);
        posiblesReacciones.AddRange(posiblesReaccionesSinObjetivo);
        string text = "";
        foreach (var item in posiblesReacciones)
        {
            text += item.nombre + "  ";
        }
        if (posiblesReacciones.Count > 0)
        {
            c.personajeInterversor = posiblesReacciones[0];
            c.posibleReaccion = true;
        }
        else c.posibleReaccion = false;
    }

    private static int CompararPersonajesPorPrioridad(cPersonaje x, cPersonaje y)
    {
        if (x.dadosDeAccion[0] > y.dadosDeAccion[0])
        {
            Debug.Log(y.nombre + "actuaria antes por dado bajo");
            return -1;
        }
        if (x.dadosDeAccion[0] < y.dadosDeAccion[0])
        {
            Debug.Log(x.nombre + "actuaria antes por dado bajo");
            return 1;
        }
        if (x.valorDeIniciativa < y.valorDeIniciativa) {
            return -1;
    }
        if (x.valorDeIniciativa > y.valorDeIniciativa) {
            return 1;
        }
        Debug.Log("empatados full");
        return 0;
    }

    virtual protected void Defensas()
    {
        if (mostrarMensaje1)
        {
            uiC.SetText("�Intentara detenerlo alguien?");
            mostrarMensaje1 = false;
            //mostrarMensaje2 = false;
        }

        if (posiblesReacciones.Count > 0 && !intentaronDetenerlo)
        {
            acc_state--;
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
                uiC.SetText("�Nadie detuvo el ataque, da en blanco!");
                if (c.personajeObjetivo is cMatones) acc_state++; //Saltear Da�o
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

    private void Da�o()
    {
        if (!(c.personajeObjetivo is cMatones))
        {
            int numeroDeDados = 3 + personaje.atr.musculo * personaje.arma.GetMusMult() + personaje.bonusPAtqBporDefB;
            if (c.movAgro) numeroDeDados -= 3;
            tirada tr = cDieMath.TirarDados(numeroDeDados, personaje.arma.GetDa�oExpl());
            c.da�o = cDieMath.sumaDe3Mayores(tr);
            uiC.SetText(personaje.nombre + ", con sus " + personaje.atr.musculo + " en Musculo y multiplicador de " + personaje.arma.GetMusMult() + ", tira " + numeroDeDados + " dados �Haciendo " + c.da�o + " de da�o!");
            personaje.bonusPAtqBporDefB = 0;
            //if (personaje.drama) uiC.PedirDrama();
        }
    }

    private void Heridas()
    {
        c.personajeObjetivo.RecibirGolpe(personaje, c.jugadorAtq, c.jugadorDef);
    }

    private void Terminado()
    {
        // esta logica va a estar en caulquier ataque, no solo en ataque basico...
        // pero banca, me gusta, podemos hacer una clase padre "Ataque" que tenga esta logica
        ResetState();
        c.movPrec = false;
        c.movAgro = false;
        c.atacando = false;
        c.jugadorDef = 0;
        personaje.bonusPAtqBporDefB = 0;
        //capaz hay que separar esto en 2 stages? chequeando muertes y chequeando si termino?
        if (c.personajeObjetivo is cMatones)
        {
            (c.personajeObjetivo as cMatones).CapazNoHayMasMatones();
        }
        else
        {
            c.personajeObjetivo.CapazMori();
        }

        if (!c.MasDeUnEquipoEnPie())
        {
            c.stateID = cCombate.TERMINANDO_COMBATE;
        }
        else
        {
            uiC.SetText("Seguimos adelante.");
            c.stateID = cCombate.BUSCANDO_ACCION;
        }
    }

    override public int DeterminarNumeroDeDados()
    {
        personaje.totalDadosDelAtacante = personaje.dadosDelAtacantePorPrecavido + personaje.arma.GetDadosDelAtacanteMod();
        int numeroDeDados = 3 + personaje.atr.ma�a + personaje.hab.ataqueBasico + personaje.arma.GetBonusAtaque() + personaje.bonusPAtqBporDefB + c.personajeObjetivo.totalDadosDelAtacante;
        if (c.movAgro) numeroDeDados -= 3;
        return numeroDeDados;
    }

    override public void ResetState()
    {
        Debug.Log("ab state reseteado");
        acc_state = AB_DETERMINANDO_DADOS - 1;
    }

    override public void ResetMensaje()
    {
        mostrarMensaje2 = true;
    }
}
