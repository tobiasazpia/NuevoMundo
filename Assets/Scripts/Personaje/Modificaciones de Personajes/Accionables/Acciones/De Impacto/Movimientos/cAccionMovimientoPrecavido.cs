using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionMovimientoPrecavido : cAccionMovimiento
{
    public int mov_state;
    public const int MOV_INICIO = 0;
    public const int MOV_DEFENSAS = 1;
    public const int MOV_TERMINADO = 2;

    public bool mostrarMensaje1;
    public bool mostrarMensaje2;

    public List<cPersonaje> posiblesReacciones;

    // Start is called before the first frame update
    void Start()
    {
        GetObjets();
        nombre = "Movimiento Precavido";
        categoria = cAcciones.AC_CAT_MOVIMIENTO;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonMoverPrec");
        esLegal = true;
    }

    override public void Ejecutar()
    {
        switch (mov_state)
        {
            case MOV_INICIO:
                Inicio();
                break;
            case MOV_DEFENSAS:
                Defensas();
                break;
            case MOV_TERMINADO:
                Terminando();
                break;
            default:
                break;
        }
        mov_state++;
        c.EsperandoOkOn(true);
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
        mostrarMensaje1 = true;
    }

    public void Defensas()
    {
        Debug.Log("mov prec defensas");
        if (mostrarMensaje1)
        {
            uiC.SetText("¿Intentara detenerlo alguien?");
            mostrarMensaje1 = false;
            mostrarMensaje2 = false;
        }

        if (posiblesReacciones.Count > 0 && !intentaronDetenerlo)
        {
            mov_state--;
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
                uiC.SetText("Nadie detiene a " + c.personajeActivo.nombre + " y se mueve a " + c.zonas[c.zonaObjetiva].nombre + ". Todos tiran -1d al atacar a " + c.personajeActivo.nombre + " hasta que vuelva a actuar.");
                c.personajeActivo.zonaActual = c.zonaObjetiva;
                c.personajeActivo.transform.position = new Vector3(c.personajeActivo.zonaActual * 10 - 10, 0, c.personajeActivo.transform.position.z);
                c.personajeActivo.dadosDelAtacantePorPrecavido = -1;
                c.personajeActivo.totalDadosDelAtacante = c.personajeActivo.dadosDelAtacantePorPrecavido + c.personajeActivo.arma.GetDadosDelAtacanteMod();
                mov_state = MOV_INICIO - 1;
                c.EsperandoOkOn(true);
                c.stateID = cCombate.BUSCANDO_ACCION;
            }
            else
            {
                mov_state--; //quedarse un cacho para mostrar mensaje
                mostrarMensaje2 = true;
            }
        }
    }

    public void Terminando()
    {

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
                        if (c.ZonaEsteEnRangoDePersonaje(p, c.personajeActivo.zonaActual) && (p.arma as cArmasFuego).cargada) posiblesReaccionesSinObjetivo.Add(p);
                    }
                    else
                    {
                        if (c.ZonaEsteEnRangoDePersonaje(p, c.personajeActivo.zonaActual)) posiblesReaccionesSinObjetivo.Add(p);
                    }
                }
                else if (p.zonaActual == c.personajeActivo.zonaActual)
                {

                    posiblesReaccionesSinObjetivo.Add(p);
                }
                else if (p.arma is cArmasPelea)
                {
                    if ((p.arma as cArmasPelea).armaImprovisadaActiva)
                    {
                        foreach (var item in c.zonas[p.zonaActual].zonasEnRango)
                        {
                            Debug.Log("zona en rango: " + item);
                            Debug.Log("zona que buscamos: " + (c.personajeActivo.zonaActual));
                            if (item == c.personajeActivo.zonaActual)
                            {
                                Debug.Log("return true, deberia tener reaccion disponible");
                                posiblesReaccionesSinObjetivo.Add(p);
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
        mov_state = MOV_INICIO - 1;
    }
}
