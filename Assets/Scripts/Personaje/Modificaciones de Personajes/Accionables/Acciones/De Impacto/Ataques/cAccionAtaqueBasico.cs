using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionAtaqueBasico : cAccionAtaque
{
    public const int AB_DETERMINANDO_DADOS = 0;
    public const int AB_TIRANDO = 1;
    public const int AB_DEFENSAS = 2;
    public const int AB_DAÑO = 3;
    public const int AB_HERIDAS = 4;
    public const int AB_TERMINADO = 5;
    public const int AB_ERROR = -1;

    public int dadosATirar;
    public bool ataqueExitoso;

    protected string textoAdicional = "";

    protected void Start()
    {
        GetObjets();
        nombre = "Ataque Básico";
        consecuencia = "Tratás de dañar al oponente.";
        reglas = nombre + ": Ataque. " + consecuencia;
        categoria = cAcciones.AC_CAT_MARCIAL;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonAtacar");
        icon = c.GetComponent<cIconos>().AB;
        if (personaje.tieneTradicionMarcial) reroleandoState = AB_DETERMINANDO_DADOS;
        else reroleandoState = AB_TIRANDO;
    }

    override public void RevisarLegalidad()
    {
        esLegal = c.HayEnemigosEnMelee(personaje);
    }

    override public void Ejecutar()
    {
        switch (acc_state)
        {
            case AB_DETERMINANDO_DADOS:
                //Debug.Log("ab_state 0");
                uiC.acc = this;
                DeterminadoDados();
                break;
            case AB_TIRANDO:
                Tirando();
                break;
            case AB_DEFENSAS:
                //Debug.Log("ab_state 2");
                Defensas();
                break;
            case AB_DAÑO:
                //Debug.Log("ab_state 3");
                TiramosDaño();
                break;
            case AB_HERIDAS:
                //Debug.Log("ab_state 4");
                TiramosHeridas();
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
        //    case AB_DAÑO:
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
        string text = "¡" + UIInterface.NombreDePersonajeEnNegrita(personaje) + " usa su " + nombre + " contra " + UIInterface.NombreDePersonajeEnNegrita(c.personajeObjetivo) + "! Tira " + dadosATirar + " dados contra su guardia de " + UIInterface.IntEnNegrita(c.personajeObjetivo.GetGuardia()) + "." + armasImprovisadas;
        if (c.movAgro) {
            uiC.SetText(text);
        }
        else
        {
            c.personajeActivo.GastarDado(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
            uiC.ActualizarIniciativa(c.personajes);
        }
    }

    virtual protected void armaImpro()
    {
        (personaje.arma as cArmasPelea).ActualizarDataDeImprovisada(false);
    }

    virtual protected void Tirando()
    {
        mostrarMensaje1 = true;
        tirada tr;
        if (personaje.arma is cLaVoluntadDelCreador)
        {
            if ((personaje.arma as cLaVoluntadDelCreador).maestria > 2) tr = cDieMath.TirarDados10v11(dadosATirar);
            else tr = cDieMath.TirarDados(dadosATirar);
        }
        else tr = cDieMath.TirarDados(dadosATirar);
        c.jugadorAtq = Mathf.Min(30,cDieMath.sumaDe3Mayores(tr));
        string resultado;
        string atq;
        textoAdicional = "";
        ataqueExitoso = c.jugadorAtq >= c.personajeObjetivo.GetGuardia();
        if (ataqueExitoso)
        {
            resultado = SuperamosGuardia();
            atq = UIInterface.IntExitoso(c.jugadorAtq);
        }
        else
        {
            resultado = NoSuperamosGuardia();
            atq = UIInterface.IntFallido(c.jugadorAtq);
        }
        uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " saca " + atq + ", " + resultado + " el ataque contra la guardia de " + UIInterface.IntEnNegrita(c.personajeObjetivo.GetGuardia()) + " de " + UIInterface.NombreDePersonajeEnNegrita(c.personajeObjetivo) + "." + textoAdicional);
        if (personaje.Drama && !ataqueExitoso) uiC.PedirDrama();
    }

    virtual protected string SuperamosGuardia()
    {
        LlenarReaccionesPosibles();
        return "acertando";
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
            uiC.SetText("¿Intentara detenerlo alguien?");
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
                uiC.SetText("¡Nadie detuvo el ataque, da en blanco!");
                if (c.personajeObjetivo is cMatones) acc_state++; //Saltear Daño
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

    protected virtual void TiramosDaño()
    {
        if (!(c.personajeObjetivo is cMatones))
        {
            int numeroDeDados = 3 + personaje.atr.musculo * personaje.arma.GetMusMult() + personaje.BonusPAtqBporDefB;
            if (c.movAgro) numeroDeDados -= 3;
            tirada tr = cDieMath.TirarDados(numeroDeDados, personaje.arma.GetDañoExpl(), c.personajeObjetivo.tieneIraDivina);
            c.daño = cDieMath.sumaDe3Mayores(tr);
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + ", con sus " + personaje.atr.musculo + " en Musculo y multiplicador de " + personaje.arma.GetMusMult() + ", tira " + numeroDeDados + " dados ¡Haciendo " + UIInterface.IntEnNegrita(c.daño) + " de daño!");
            uiC.perCambio = c.personajeObjetivo.nombre;
            c.personajeObjetivo.Daño += c.daño;
            //if (personaje.drama) uiC.PedirDrama();
        }
    }

    private void TiramosHeridas()
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
        uiC.perCambio = personaje.nombre;
        personaje.BonusPAtqBporDefB = 0;
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
        int numeroDeDados = 3 + personaje.atr.maña + personaje.tradicionMarcial[0] + personaje.arma.GetBonusAtaque() + personaje.BonusPAtqBporDefB + c.personajeObjetivo.totalDadosDelAtacante - personaje.impuesto;
        if (c.movAgro) numeroDeDados -= 3;
        return numeroDeDados;
    }

    override public void ResetState()
    {
        acc_state = AB_DETERMINANDO_DADOS - 1;
        reroleando = false;
        Debug.Log("reroll false");
    }

    override public void ResetMensaje()
    {
        mostrarMensaje2 = true;
    }
}
