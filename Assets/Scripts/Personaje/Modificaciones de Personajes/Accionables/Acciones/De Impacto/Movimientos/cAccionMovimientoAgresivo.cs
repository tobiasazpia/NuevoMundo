using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionMovimientoAgresivo : cAccionMovimiento
{
    public int movag_state;
    public const int MOVAG_INICIO = 0;
    public const int MOVAG_DEFENSAS = 1;
    public const int MOVAG_TERMINADO = 2;

    public bool ready = true;
    // Start is called before the first frame update
    void Start()
    {
        GetObjets();
        nombre = "Movimiento Agresivo";
        categoria = cAcciones.AC_CAT_MOVIMIENTO;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonMoverAgro");
    }

    override public void RevisarLegalidad()
    {
        esLegal = c.HayEnemigosVivosEnZonasLimitrofes();
    }

    override public void Ejecutar()
    {
        Debug.Log("mov agro state: " + movag_state);
        switch (movag_state)
        {
            case MOVAG_INICIO:
                Inicio();
                break;
            case MOVAG_DEFENSAS:
                Defensas();
                break;
            default:
                // c.stateID = cCombate.BUSCANDO_ACCION;
                break;
        }
        movag_state++;
        //c.EsperandoOkOn(true);
    }

    public void Inicio()
    {
        foreach (var p in c.personajes)
        {
            p.guardando = false;
        }
        LlenarAccionesPosibles();
        string text = (personaje.nombre + " trata de moverse a " + c.zonas[c.zonaObjetiva].nombre + ".");
        c.personajeActivo.GastarDado(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
        uiC.ActualizarIniciativa(c.personajes);
        //no gastamos esta accion porque se va a gastar cuando se haga el ataque basico... es medio raro esto, onda, que pasa si detienen el movimiento?
        //entonces, hay que gastarlo aca y en atauqe basico NO gastarlo si es un mov agro
        mostrarMensaje1 = true;
        c.personajeActivo.dadosDelAtacantePorPrecavido = 0;
    }

    virtual public void Defensas()
    {
        if (mostrarMensaje1)
        {
            uiC.SetText("¿Intentara detenerlo alguien?");
            mostrarMensaje1 = false;
            mostrarMensaje2 = false;
        }
        //movag_state--; //probando sacarlo del if..?
        if (posiblesReacciones.Count > 0 && !intentaronDetenerlo)
        {
            movag_state--;
            c.personajeInterversor = posiblesReacciones[0];
            posiblesReacciones.RemoveAt(0);
            c.stateID = cCombate.PREGUNTANDO_REACCION;
        }
        else
        {
            posiblesReacciones.Clear();
            uiC.DejarDePedirReaccion();
            c.personajeActivo.SetZonaActual(c.zonaObjetiva);
            c.personajeActivo.transform.position = new Vector3(c.personajeActivo.GetZonaActual() * 10 - 10, 0, c.personajeActivo.transform.position.z);
            c.personajeActivo.totalDadosDelAtacante = c.personajeActivo.dadosDelAtacantePorPrecavido + c.personajeActivo.arma.GetDadosDelAtacanteMod();
            ResetState();
            if (c.personajeActivo.ai == null)
            {
                uiC.SetText("Nadie detiene a " + c.personajeActivo.nombre + " y carga contra " + c.zonas[c.zonaObjetiva].nombre + " para atacar. ¿Pero a quien?");
                c.esperandoObjetivo = true;
                ready = false;
            }
            else
            {
                uiC.SetText("Nadie detiene a " + c.personajeActivo.nombre + " y carga contra " + c.zonas[c.zonaObjetiva].nombre + " listo para atacar a " + c.personajeObjetivo.nombre + ".");
                c.atacando = true;
                c.stateID = cCombate.RESOLVIENDO_ACCION;
                c.accionActiva = cPersonaje.AC_ATACAR;
                uiC.RegistrarAccion();
                c.EsperandoOkOn(true);
            }
        }
    }

    private void LlenarAccionesPosibles()
    {
        posiblesReacciones = new List<cPersonaje>();
        List<cPersonaje> posiblesReaccionesSinObjetivo = new List<cPersonaje>();
        foreach (var p in c.personajes)
        {
            if (p.equipo != personaje.equipo && (p.reaccion1Disponible || p.reaccion2Disponible)) //si tien
            {
                if (p.arma.GetDeRango()) //si es rango, y la zona de la que proviene el ataque esta en rango
                {
                    if (p.arma is cArmasFuego)
                    {
                        if (c.ZonaEsteEnRangoDePersonaje(p, c.personajeActivo.GetZonaActual()) && (p.arma as cArmasFuego).cargada) posiblesReaccionesSinObjetivo.Add(p);
                    }
                    else
                    {
                        if (c.ZonaEsteEnRangoDePersonaje(p, c.personajeActivo.GetZonaActual())) posiblesReaccionesSinObjetivo.Add(p);
                    }
                }
                else if (p.GetZonaActual() == c.personajeActivo.GetZonaActual())
                {
                    posiblesReaccionesSinObjetivo.Add(p);
                }
                else if (p.arma is cArmasPelea)
                {
                    if ((p.arma as cArmasPelea).armaImprovisadaActiva)
                    {
                        foreach (var item in c.zonas[p.GetZonaActual()].zonasEnRango)
                        {
                            Debug.Log("zona en rango: " + item);
                            Debug.Log("zona que buscamos: " + (c.personajeActivo.GetZonaActual()));
                            if (item == c.personajeActivo.GetZonaActual())
                            {
                                Debug.Log("return true, deberia tener reaccion disponible");
                                posiblesReaccionesSinObjetivo.Add(p);
                                break;
                            }
                        }
                    }
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
    static int CompararPersonajesPorPrioridad(cPersonaje x, cPersonaje y)
    {
        if (x.dadosDeAccion[0] > y.dadosDeAccion[0])
        {
            return -1;
        }
        if (x.dadosDeAccion[0] < y.dadosDeAccion[0])
        {
            Debug.Log(x.nombre + "actuaria antes por dado bajo");
            return 1;
        }
        if (x.valorDeIniciativa < y.valorDeIniciativa)
        {
            return -1;
        }
        if (x.valorDeIniciativa > y.valorDeIniciativa)
        {
            return 1;
        }
        Debug.Log("empatados full");
        return 0;
    }

    override public void ResetState()
    {
        movag_state = MOVAG_INICIO - 1;
    }
}
