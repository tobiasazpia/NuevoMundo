using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct sAccion
{
    public cPersonaje per;
    public int fase;
}

public class cCombate : MonoBehaviour
{
    /////Constantes
    public const int INICIANDO_COMBATE = 0; // estado inicial, a INICIANDO_RONDA
    public const int INICIANDO_RONDA = 1; // A TIRANDO_INICIATIVA
    public const int TIRANDO_INICIATIVA = 2; // A BUSCANDO_ACCION
    public const int BUSCANDO_ACCION = 3; // A PREGUNTANDO_ACCION o TERMINANDO_RONDA
    public const int PREGUNTANDO_ACCION = 4; // A RESOLVIENDO_ACCION
    public const int RESOLVIENDO_ACCION = 5; // A PREGUNTANDO REACCION, BUSCANDO_ACCION o TERMINANDO_COMBATE
    public const int PREGUNTANDO_REACCION = 6; // A RESOLVIENDO ACCION
    public const int RESOLVIENDO_REACCION = 7;
    public const int TERMINANDO_RONDA = 8; // A INICIANDO_RONDA
    public const int TERMINANDO_COMBATE = 9; // estado final
    /////Prefabs
    public GameObject personajePrefab;
    public GameObject matonPrefab;
    public GameObject zonaPrefab;
    /////Materiales
    public Material materialEquipo1;
    public Material materialEquipo2;
    public Material materialEquipo3;
    public Material materialEquipo4;
    public Material materialEquipo5;
    public Material materialEquipo6;
    /////Variables
    //////Variables por Combate
    public int stateID;
    public bool esperandoOK;
    public List<cZona> zonas;
    public List<cPersonaje> personajes;
    public List<cPersonaje> ultimosEnActuar;
    public int rondaActual;
    //////Variables por Ronda
    public int faseActual;
    int equipoVictorioso;
    bool esperarA;
    public bool play = false;
    public bool listos = false;
    public bool esRoguelike = false;
    /////Constantes
    public const int AT_TIRAR = 0; // estado inicial, a INICIANDO_RONDA
    public const int AT_MOSTRARTIRADA = 1; // A TIRANDO_INICIATIVA
    public const int AT_DEFENSAS = 2; // A BUSCANDO_ACCION
    public const int AT_DAÑO = 3; // A BUSCANDO_ACCION
    public const int AT_HERIDAS = 4; // A BUSCANDO_ACCION
    int atqState;

    public GameObject roguelikeUpgrade;
    public cRoguelikeUpgrade rU;
    public cRoguelikeManager rM;

    public delegate void combate();
    public static event combate eventTerminoCombate;

    public PlayerInput py;
    public bool pause = false;

    //public AudioSource music;

    //State ID
    public const int EMPEZAR = 0;
    public const int SIN_RONDA = 1;
    public const int TIRAR_INICIATIVA = 2;
    public const int SIN_FASE = 3;
    public const int PREGUNTA_ACTUAMOS = 4;
    public const int A_DECIDIR = 5;
    public const int A_ACTUAR = 6;
    public const int CHEQUEANDO_REACCION = 7;
    public const int CHEQUEANDO_REACCION_INDIVIDUAL = 8;
    public const int A_DEFENDER = 9;
    public const int A_EMBOCAR = 11;
    public const int A_HERIDAS = 12;
    public const int TERMINAR_COMBATE = 13;
    public const int SIN_COMBATE = 14;

    // Otros GameObjects
    cScoreCard scoreCard;
    public cPlanillaDeCombate ui;
    public UICombate uiC;

    //Simulacion
    public bool auto;               // Estamos simulando, no nos interesan los pasos individuales, solo los resultados finales
    public bool time = false;

    //Input
    public bool esperandoObjetivo;         // Epserando eleccion de personaje
    public bool esperandoZona;             // Esperando eleccion de zona
    public bool esperandoAccion;           // Esperando eleccion de accion
    public bool esperandoReaccion;         // Esperando Si o No para ver si reacciona
    public bool esperandoCarga;            // Esperando Si o No para ver si carga

    public bool hayJugadores = false;
    public bool hayMatones = false;
    public bool jugadorPuedeReaccionar = false;
    public bool atacando; // AKA, no estamos deteniendo un movimiento
    public int jugadorAtq;
    public int jugadorDef;
    public int iniciativaIndex = 0;
    public bool mostrandoMensaje = false;

    public bool posibleReaccion;

    float tiempoDesdeEvento;
    const float ESPERA = 2.0f;

    //Varibles para la accion actual
    public cPersonaje personajeActivo;
    public cPersonaje personajeObjetivo;
    public cPersonaje personajeInterversor;
    public List<cPersonaje> enemigosEnMelee;
    public List<cPersonaje> enemigosEnRango;
    public List<int> zonasLimtrofesConEnemigos;
    public bool movAgro;
    public bool movPrec;
    public int daño;
    public int accionActiva;
    public int reaccionActiva;
    public int zonaObjetiva;

    //Game State
    public List<sAccion> accionesActivas;
    public List<sAccion> accionesReactivas;
    public List<sAccion> acciones;

    public bool perSeleccionado;

    private void OnEnable()
    {
        scoreCard = GetComponentInParent<cScoreCard>();
        GameObject.Find("Main Camera").GetComponent<cPlanillaDeCombate>().c = this;
        ui = GameObject.Find("Main Camera").GetComponent<cPlanillaDeCombate>();
        uiC.SetText("Nuevo Combate");

        personajes = new List<cPersonaje>();
        enemigosEnMelee = new List<cPersonaje>();
        enemigosEnRango = new List<cPersonaje>();
        zonasLimtrofesConEnemigos = new List<int>();
        zonas = new List<cZona>();

        //music.loop = true;
        //music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            if (esperandoOK)
            {
                if (py.actions["OK"].WasPressedThisFrame())
                {
                    AvanzarCombate();
                }
            }

            if (py.actions["Pause"].WasPressedThisFrame())
            {
                pause = true;
                uiC.Pause();
            }
        }
        else
        {
            if (py.actions["Pause"].WasPressedThisFrame())
            {
                uiC.Reanudar();
            }
        }
    }

    public void Reanudar()
    {
        pause = false;
    }

    public void AvanzarCombate()
    {
        EsperandoOkOn(false);
        switch (stateID)
        {
            //El combate hace todo el set up, y cuando esta listo para empezar, entra en iniciando ronda
            case INICIANDO_RONDA: // A BuscandoProximaPosibleAccion
                IniciarRonda();
                break;
            case TIRANDO_INICIATIVA: // A TirarIniciativa
                TirarIniciativa();
                break;
            case BUSCANDO_ACCION: // A PreguntandoAccion o si no hay, Terminando Ronda
                BuscarAccion();
                break;
            case PREGUNTANDO_ACCION: // ResolviendoAccion
                PreguntarAccion();
                break;
            case RESOLVIENDO_ACCION: // A Preguntando reaccion si alguien puede reaccionar, sino A BuscandoProximaPosibleAccion o TerminandoCombate si solo hay un equipo vivo
                ResolverAccion();
                break;
            case PREGUNTANDO_REACCION: // A BuscandoProximaPosibleAccion o TerminandoCombate
                PreguntarReaccion();
                break;
            case RESOLVIENDO_REACCION: // A Preguntando reaccion si alguien puede reaccionar, sino A BuscandoProximaPosibleAccion o TerminandoCombate si solo hay un equipo vivo
                ResolverReaccion();
                break;
            case TERMINANDO_RONDA: // A Iniciar Ronda
                TerminarRonda();
                break;
            case TERMINANDO_COMBATE: // estado final
                TerminarCombate();
                break;
            default:
                LimpiarCombate();
                if (esRoguelike)
                {
                    if (equipoVictorioso == 1)
                    {
                        UIInterface.GoUpgrade(); //ganamos, vamos a upgrade
                        rU.LetsUpgrade();
                    }
                    else
                    {
                        uiC.iniciativa.Clear();
                        UIInterface.GoRoguelikeEnd(); //perdimos, vamos a rogulike ini
                        rM.uiRE.fillText(rM.nivel);
                        rM.nivel = 1;
                        rM.party.Clear();
                    }
                }
                else UIInterface.GoEscarmuza();
                break;
        }
    }

    public void LimpiarCombate()
    {
        foreach (var item in personajes)
        {
            for (int i = 0; i < item.dadosDeAccion.Length; i++)
            {
                item.dadosDeAccion[i] = 11;
            }
        }
        uiC.ActualizarIniciativa(personajes);
        foreach (var item in personajes)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in zonas)
        {
            Destroy(item.gameObject);
        }
        personajes.Clear();
        acciones.Clear();
        if (accionesActivas != null) accionesActivas.Clear();
        if (accionesReactivas != null) accionesReactivas.Clear();
        zonas.Clear();
        uiC.ResetRonda();
        List<cPersonaje> emptyL = new List<cPersonaje>();
        uiC.SetNombres(emptyL);
        pause = false;
    }


    public void IniciarRonda()
    {
        //Prepara toda la data para empezar la ronda, y nos manda a BuscarAccion
        uiC.SetRonda(++rondaActual);
        faseActual = 0;
        acciones = new List<sAccion>();

        stateID = TIRANDO_INICIATIVA;
        esperarA = true;
        uiC.SetText("¡Nueva ronda!");

        EsperandoOkOn(true);
    }

    public void TirarIniciativa()
    {
        if (esperarA) // Primera vez en esta funcion, solo se muestra el mensaje
        {
            uiC.SetText("Tirando Iniciativa");
            esperarA = false;
            EsperandoOkOn(true);
            return;
        }

        bool nadieEstabaDisponible = true;
        foreach (var per in personajes) // a partir de la 2da vez en esta funcion
        {
            if (per.vivo && per.valorDeIniciativa == 0) // Hay alguien a quien le falta tirar iniciativa
            {
                nadieEstabaDisponible = per.arma.AccionesFase0();
                if (!nadieEstabaDisponible) break;

                //Tiramos la iniciativa de un personaje, y salimos para mostrarla               
                per.Iniciativa();
                for (int j = 0; j < per.dadosDeAccion.Length; j++)
                {
                    sAccion ac;
                    ac.per = per;
                    ac.fase = per.dadosDeAccion[j];
                    acciones.Add(ac);
                }

                uiC.ActualizarIniciativa(personajes);
                uiC.SetText("Tirando Iniciativa: " + per.nombre);
                nadieEstabaDisponible = false;
                break;
            }
        }
        EsperandoOkOn(true);

        if (nadieEstabaDisponible)
        {
            //Ya tiramos la iniciativa de todos los personajes, empezamos a buscar acciones
            stateID = BUSCANDO_ACCION;
            uiC.SetText("Listos para empezar la ronda.");
            EsperandoOkOn(true);
        }
    }

    private void ResetAccionActiva()
    {
        accionActiva = cPersonaje.AC_SINASIGNAR;
        personajeActivo = null;
        atacando = false;
        movAgro = false;
        movPrec = false;
    }

    public void BuscarAccion()
    {
        //Avanzar por las fases hasta encontrar acciones que puedan ser ejecutadas
        //Si encuentra nos manda a PreguntarAccion
        //Si no nos manda a TerminarRonda
        bool encontramosAccion = false;
        ResetAccionActiva();
        foreach (var per in personajes)
        {
            if (per.vivo && !per.guardando)
            {
                foreach (var dado in per.dadosDeAccion)
                {
                    if (dado <= faseActual)
                    {
                        encontramosAccion = true; //hay por lo menos una accion legal para hacer
                        stateID = PREGUNTANDO_ACCION;
                        if (personajeActivo == null)
                        {
                            personajeActivo = per;  //si no habiamos encontrado otra accion legal, esta es ahora la candidata
                        }
                        else // si si habiamos encontrado, hay que ver quien actua primero 
                        {
                            if (personajeActivo.valorDeIniciativa == per.valorDeIniciativa) // si tienen la misma iniciativa hay que desempatar
                                                                                            //actua primero el que hace mas tiempo que no actua. para eso, llevamos una lista de los ultimos personajes en actuar
                            {
                                Debug.Log("empate de iniciativa para actuar! quien ira primero?");
                                bool perEncontrado = false;
                                bool activoEncontrado = false;
                                foreach (var item in ultimosEnActuar)
                                {
                                    if (item.nombre == personajeActivo.nombre)
                                    {
                                        activoEncontrado = true;
                                        Debug.Log("activo ya actuo");
                                    }
                                    else if (item.nombre == per.nombre)
                                    {
                                        perEncontrado = true;
                                        Debug.Log("candidato ya actuo");
                                    }
                                }
                                //si los dos ya actuaron este combate
                                if (perEncontrado && activoEncontrado)
                                {
                                    for (int i = 0; i < ultimosEnActuar.Count; i++)
                                    {
                                        if (ultimosEnActuar[i].nombre == per.nombre || ultimosEnActuar[i].nombre == personajeActivo.nombre)
                                        {
                                            personajeActivo = ultimosEnActuar[i]; // estamos asumiendo que cuando se agraga alguien a la lista va al final, por lo tanto si esta mas cerca del inicio, hace mas timepo que no actua
                                            break;
                                        }
                                    }
                                    Debug.Log("ya acturaon los dos, va " + personajeActivo.nombre + "porque fue la primera accion que encontramos subiendo por la lista"); //esto es un poco retrasado: deberiamos ir por 
                                }
                                else if (activoEncontrado) // activo ya actuo pero per no, le toca a per
                                {
                                    Debug.Log("activo ya actuo pero per no, le toca a per");
                                    personajeActivo = per;
                                }
                                else if (Random.Range(0, 2) == 1) //"nignuno de los dos actuo, pero randomisamos que cambie le jugador activo
                                {
                                    Debug.Log("nignuno de los dos actuo, pero randomisamos que cambie le jugador activo");
                                    personajeActivo = per; //en el juego irl favorecemos a los jugadores, y entre ellos deciden (supongo que para pvp no hay desempate)
                                }
                            }
                            else if (personajeActivo.valorDeIniciativa < per.valorDeIniciativa) // Si el nuevo candidato tiene mas iniciativa, el es el nuevo activo
                            {
                                personajeActivo = per;
                            }
                        }
                        break;
                    }
                }
            }
        }
        if (encontramosAccion)
        {
            AvanzarCombate();
        }
        else
        {
            faseActual++;
            if (faseActual > 10)
            {
                stateID = TERMINANDO_RONDA;
                AvanzarCombate();
            }
            else
            {
                EsperandoOkOn(true);
                uiC.IrAFase(faseActual);
                accionesActivas = new List<sAccion>();
                accionesReactivas = new List<sAccion>();
                for (int i = 0; i < acciones.Count; i++)
                {
                    if (acciones[i].fase <= faseActual)
                    {
                        accionesActivas.Add(acciones[i]);
                        accionesReactivas.Add(acciones[i]);
                        acciones[i].per.reaccion1Disponible = true;
                        // esto dice que tenemos una reaccion disponible nada mas, no que tenesmos una reaccion legal que hacer ante un ataque
                    }
                }
                if (faseActual == 10)
                {
                    foreach (var item in personajes)
                    {
                        Debug.Log("guard false 7");
                        item.guardando = false;
                    }
                }
            }

        }
    }

    public void PreguntarAccion()
    {
        //Pregunta a la IA o jugador como van a usar su accion o si la van a guardar
        //Si la van a usar nos manda a ResolverAccion
        //Si no nos manda a BuscarAccion
        BuscarEnemigosEnRango();

        if (personajeActivo.ai is null) // es personaje jugable
        {
            switch (accionActiva)
            {
                case cPersonaje.AC_SINASIGNAR:
                    PedirAccion();
                    break;
                case cPersonaje.AC_MARCIAL:
                    PedirMarcial();
                    break;
                case cPersonaje.AC_ARCANA:
                    PedirArcana();
                    break;
                case cPersonaje.AC_MOVER:
                    PedirMovimiento();
                    break;
                case cPersonaje.AC_GUARDAR:
                    break;
                case cPersonaje.AC_RECARGAR:
                    break;
                case cPersonaje.AC_ATACAR:
                    PedirObjetivoDeAtaque();
                    Debug.Log("atacando normal");
                    break;
                case cPersonaje.AC_ATACARIMPRO:
                    Debug.Log("atacando impro");
                    PedirObjetivoDeAtaque();
                    break;
                case cPersonaje.AC_MOVAGRE:
                    movPrec = false;
                    movAgro = true;
                    PedirObjetivoDeMovimiento();
                    break;
                case cPersonaje.AC_MOVPREC:
                    movPrec = true;
                    movAgro = false;
                    PedirObjetivoDeMovimiento();
                    break;
                case cPersonaje.AC_ATRAS:
                    PedirAtras();
                    break;
                case cPersonaje.AC_MOVIMPRO:
                    movPrec = false;
                    movAgro = true;
                    PedirObjetivoDeMovimiento();
                    break;
                default:
                    break;
            }
        }
        else // es ai
        {
            uiC.SetText("¿Que va a hacer " + personajeActivo.nombre + "?");
            BuscarZonasLimitrofesConEnemigos();
            accionActiva = personajeActivo.ai.ElegirAccion(enemigosEnRango, zonasLimtrofesConEnemigos, zonas[personajeActivo.zonaActual].zonasLimitrofes, faseActual);
            if (accionActiva != cPersonaje.AC_GUARDAR) uiC.RegistrarAccion();
            stateID = RESOLVIENDO_ACCION;
            EsperandoOkOn(true);
        }
    }

    public void ResolverAccion()
    {
        personajeActivo.Accionar(GetNombreDeAccion(accionActiva));
        if (stateID != RESOLVIENDO_ACCION && stateID != PREGUNTANDO_REACCION) cEventManager.StartPersonajeActuoEvent(personajeActivo);
    }

    public string GetNombreDeAccion(int numero)
    {
        switch (numero)
        {
            case cPersonaje.AC_ATACAR:
                return "Ataque Basico";
            case cPersonaje.AC_MOVPREC:
                return "Movimiento Precavido";
            case cPersonaje.AC_MOVAGRE:
                return "Movimiento Agresivo";
            case cPersonaje.AC_GUARDAR:
                return "Guardar";
            case cPersonaje.AC_ENCONTRAR:
                return "Encontrar Improvisada";
            case cPersonaje.AC_RECARGAR:
                return "Recargar";
            case cPersonaje.AC_ATACARIMPRO:
                return "Ataque Basico Improvisado";
            case cPersonaje.AC_MOVIMPRO:
                return "Movimiento Agresivo Improvisado";
            default:
                return "ERROR";
        }
    }

    public void ResolverReaccion()
    {
        personajeInterversor.Reaccionar(GetNombreDeReaccion(reaccionActiva));
    }

    public string GetNombreDeReaccion(int numero)
    {
        switch (numero)
        {
            case cPersonaje.DB_DefensaBasica:
                return "Defensa Basica";
            case cPersonaje.DB_DefensaBasicaImpro:
                return "Defensa Basica Improvisada";
            default:
                return "ERROR";
        }
    }

    private void PreguntarReaccion()
    {
        if (personajeInterversor.ai == null) // Es un jugador, le la ui se encarga
        {
            uiC.PedirReaccion(personajeInterversor);
        }
        else // es una ai, le preguntamos como reacciona
        {
            if (personajeInterversor.ai.Reaccion(jugadorAtq))
            {
                uiC.SetText("¡" + personajeInterversor.nombre + " va a intervenir!");
                stateID = cCombate.RESOLVIENDO_REACCION;
            }
            else
            {
                uiC.SetText(personajeInterversor.nombre + " no intervendra.");
                stateID = cCombate.RESOLVIENDO_ACCION;
            }
            EsperandoOkOn(true);
        }
    }

    public void TerminarRonda()
    {
        //Prepara toda la data para terminar la ronda, y nos manda a EmpezarRonda
        cEventManager.StartFinDeRondaEvent();
        acciones.Clear();
        stateID = INICIANDO_RONDA;
        uiC.SetText("Se termino la ronda, pero a este combate todavia le falta mucho para llegar a su fin.");
        EsperandoOkOn(true);
    }

    public void TerminarCombate()
    {
        cEventManager.StartFinDeRondaEvent();
        equipoVictorioso = -1;
        foreach (var per in personajes)
        {
            if (per.vivo)
            {
                equipoVictorioso = per.equipo;
                break;
            }
        }
        string text = "Solo queda un equipo vivo, el equipo " + equipoVictorioso.ToString() + "! ";
        List<string> nombres = new List<string>();
        foreach (var per in personajes)
        {
            if (per.equipo == equipoVictorioso) nombres.Add(per.nombre);
        }
        for (int i = 0; i < nombres.Count; i++)
        {
            text += nombres[i];
            if (i + 2 == nombres.Count) text += " y ";
            else if (nombres.Count > 1 && i < (nombres.Count - 1)) text += ", ";
        }
        if (nombres.Count > 1) text += " emergen victoriosos.";
        else text += " emerge victorioso.";
        uiC.SetText(text);
        stateID++;
        EsperandoOkOn(true);
    }

    bool ChequearZonaValida(int z)
    {
        foreach (var item in zonas[personajeActivo.zonaActual].zonasLimitrofes)
        {
            if (z == item) return true;
        }
        return false;
    }


    //chequear si hay enemigos vivos en rango
    public bool HayEnemigosVivosEnRango()
    {
        return (enemigosEnRango.Count > 0);
    }

    public bool HayEnemigosVivosEnZonasLimitrofes()
    {
        foreach (var pers in personajes)
        {
            foreach (var zona in zonas[personajeActivo.zonaActual].zonasLimitrofes)
            {
                if (zona == pers.zonaActual && pers.equipo != personajeActivo.equipo) return true;
            }
        }
        return false;
    }

    void BuscarEnemigosEnMelee()
    {
        enemigosEnMelee.Clear();
        foreach (var p in personajes)
        {
            if (p.vivo && p.equipo != personajeActivo.equipo && p.zonaActual == personajeActivo.zonaActual)
            {
                enemigosEnMelee.Add(p);
            }
        }
    }

    public bool HayEnemigosEnMelee(cPersonaje per)
    {
        foreach (var p in personajes)
        {
            if (p.vivo && p.equipo != per.equipo && p.zonaActual == per.zonaActual)
            {
                return true;
            }
        }
        return false;
    }

    void BuscarEnemigosEnRango()
    {
        bool rango = false;
        if (personajeActivo.arma is cArmasPelea)
        {
            if ((personajeActivo.arma as cArmasPelea).armaImprovisadaActiva) rango = true;
        }
        else rango = personajeActivo.arma.GetDeRango();
        enemigosEnRango.Clear();
        BuscarEnemigosEnMelee();
        foreach (var item in enemigosEnMelee)
        {
            enemigosEnRango.Add(item);
        }
        if (rango)
        {
            foreach (var p in personajes)
            {
                if (p.vivo && p.equipo != personajeActivo.equipo)
                {
                    foreach (var z in zonas[personajeActivo.zonaActual].zonasEnRango)
                    {
                        if (p.zonaActual == z)
                        {
                            enemigosEnRango.Add(p);
                            break;
                        }
                    }
                }
            }
        }
    }

    void BuscarZonasLimitrofesConEnemigos()
    {
        zonasLimtrofesConEnemigos.Clear();
        foreach (var z in zonas[personajeActivo.zonaActual].zonasLimitrofes)
        {
            foreach (var p in personajes)
            {
                if (p.vivo && p.equipo != personajeActivo.equipo && p.zonaActual == z)
                {
                    zonasLimtrofesConEnemigos.Add(z);
                    break;
                }
            }
        }
    }

    public void ContinuarFase()
    {
        if (personajeActivo != null) personajeActivo.transform.position = new Vector3(personajeActivo.transform.position.x, 0, personajeActivo.transform.position.z);
        //Realizar la accion activa del jugador con mas Iniciativa
        sAccion accionCandidata;
        if (accionesActivas.Count > 0)
        {
            accionCandidata = accionesActivas[0];
            for (int i = 1; i < accionesActivas.Count; i++)
            {
                if (accionesActivas[i].per.valorDeIniciativa > accionCandidata.per.valorDeIniciativa)
                {
                    accionCandidata = accionesActivas[i];
                }
            }
            personajeActivo = accionCandidata.per;
            if (!personajeActivo.guardando) personajeActivo.transform.position = new Vector3(personajeActivo.transform.position.x, 2, personajeActivo.transform.position.z);
            stateID = A_DECIDIR;
        }
        else
        {
            faseActual++;
            uiC.IrAFase(faseActual);
            if (faseActual == 10)
            {
                foreach (var p in personajes)
                {
                    Debug.Log("guard false 6");
                    p.guardando = false;
                }
            }
            ContinuarRonda();
        }
    }

    void PedirAccion()
    {
        //No se que hacen estos
        auto = false;
        esperandoAccion = true;
        esperandoObjetivo = false;
        esperandoZona = false;
        esperandoCarga = false;

        //ui
        uiC.PedirAccion(personajeActivo);
    }

    void PedirMarcial()
    {
        uiC.PedirMarcial(personajeActivo);
    }

    void PedirArcana()
    {
        uiC.PedirArcana(personajeActivo);
    }


    void PedirMovimiento()
    {
        //ui
        uiC.PedirMovimiento(personajeActivo);
    }

    void PedirObjetivoDeAtaque()
    {
        uiC.SetText(personajeActivo.nombre + " va a atacar! A quien?");
        esperandoObjetivo = true;
    }

    void PedirObjetivoDeMovimiento()
    {
        uiC.SetText(personajeActivo.nombre + ": a donde vamos?");
        //Ver que zonas son legales
        if (movPrec)
        {
            for (int i = 0; i < zonas.Count; i++)
            {
                zonas[i].objetivoValidoParaJugadorActivo = ChequearZonaValida(i) || zonas[i].index == personajeActivo.zonaActual; //por ahora es legal hacer un movmiento hacia el lugar donde estas parado. se me ocurre agregar una accion tipo "mantener" que sea pasar, pero tienen -2d al atacarte
            }
        }
        else
        {
            foreach (var zona in zonas)
            {
                zona.objetivoValidoParaJugadorActivo = false;
            }
            BuscarZonasLimitrofesConEnemigos();
            foreach (var i in zonasLimtrofesConEnemigos)
            {
                zonas[i].objetivoValidoParaJugadorActivo = true;
            }
        }
        esperandoZona = true;
        uiC.esperandoZona = true;
    }

    void PedirAtras()
    {

    }

    void ContinuarRonda()
    {
        //Correr 10 fases 
        if (faseActual < 11)
        {
            stateID = SIN_FASE;
        }
        else
        {
            FinDeRonda();
        }
    }

    void FinDeRonda()
    {
        bool seguimos = MasDeUnEquipoEnPie();
        if (!seguimos)
        {
            //if (!scoreCard.set)
            //{
            //    scoreCard.SetScorecard();
            //}
            int equipoVictorioso = -1;
            for (int i = 0; i < personajes.Count; i++)
            {
                if (personajes[i].vivo)
                {
                    equipoVictorioso = personajes[i].equipo;
                    break;
                }
            }
            for (int i = 0; i < personajes.Count; i++)
            {
                //sScore sc = scoreCard.scores[personajes[i]];
                //sc.combatesJugados++;
                if (personajes[i].equipo == equipoVictorioso)
                {
                    if (personajes[i].vivo)
                    {
                        ui.m += "\n" + (personajes[i].nombre + " sobrevivio el combate y gano con el equipo " + equipoVictorioso + "!");
                    }
                    else
                    {
                        ui.m += "\n" + (personajes[i].nombre + " no sobrevivio pero su equipo el " + equipoVictorioso + " salio victorioso!");
                    }
                    //sc.wins++;
                }
                else
                {
                    //sc.loses++;
                }
                //scoreCard.scores[personajes[i]] = sc;
            }
            if (esRoguelike)
            {
                //ir al upgrade
                roguelikeUpgrade.SetActive(true);
            }
            VaciarCombate();
        }
        // Si no termino, proxima ronda
        else
        {
            stateID = SIN_RONDA;
        }
    }

    public void RemoverPersonaje(cPersonaje p)
    {
        p.vivo = false;
        for (int i = acciones.Count - 1; i >= 0; i--)
        {
            if (acciones[i].per.nombre == p.nombre)
            {
                acciones.Remove(acciones[i]);
            }
        }
        for (int i = accionesActivas.Count - 1; i >= 0; i--)
        {
            if (accionesActivas[i].per.nombre == p.nombre)
            {
                accionesActivas.Remove(accionesActivas[i]);
            }
        }
        for (int i = accionesReactivas.Count - 1; i >= 0; i--)
        {
            if (accionesReactivas[i].per.nombre == p.nombre)
            {
                accionesReactivas.Remove(accionesReactivas[i]);
            }
        }
        for (int i = 0; i < p.dadosDeAccion.Length; i++)
        {
            p.dadosDeAccion[i] = 11;
        }
        //Miedo de que este rompa cosas, pero por ahora queda
        // es algo redudndnate porque ya existe la varriable vivo, pero creo que este es mas limpio hasta tener una forma grafica de mostrar que murio.
        //personajes.Remove(p);
        //Destroy(p);
    }

    public bool MasDeUnEquipoEnPie()
    {
        bool unEquipoEnPie = false;
        int equipoEnPieIndex = -1;
        foreach (var item in personajes)
        {
            if (item.vivo && item.equipo != equipoEnPieIndex)
            {
                if (!unEquipoEnPie)
                {
                    equipoEnPieIndex = item.equipo;
                    unEquipoEnPie = true;
                }
                else return true;
            }
        }
        return false;
    }

    public void SetUpGC()
    {
        if (personajes != null)
        {
            for (int i = 0; i < personajes.Count; i++)
            {
                personajes[i].gC = personajes[i].gameObject.AddComponent(typeof(GUIComponent)) as GUIComponent;
                personajes[i].gC.p = personajes[i];
            }
        }
    }

    void SetUpZonas(string[] zCode)
    {
        for (int i = 0; i < zCode.Length; i++)
        {
            string[] words = zCode[i].Split('_');

            GameObject temp = Instantiate(zonaPrefab, new Vector3((i * 10) - 10, -1, 0), Quaternion.identity);
            temp.transform.parent = this.transform;
            zonas.Add(temp.GetComponent<cZona>());
            zonas[i].index = int.Parse(words[0]);
            zonas[i].nombre = words[1];
            int nZLimitrofes = int.Parse(words[2]);
            zonas[i].zonasLimitrofes = new int[nZLimitrofes];
            for (int j = 0; j < nZLimitrofes; j++)
            {
                zonas[i].zonasLimitrofes[j] = int.Parse(words[j + 3]);
            }
            int nZEnRango = int.Parse(words[3 + nZLimitrofes]);
            zonas[i].zonasEnRango = new int[nZEnRango];
            for (int j = 0; j < nZEnRango; j++)
            {
                zonas[i].zonasEnRango[j] = int.Parse(words[j + 4 + nZLimitrofes]);
            }
            zonas[i].gz = zonas[i].gameObject.AddComponent(typeof(cGUIZona)) as cGUIZona;
            zonas[i].gz.z = zonas[i];
            zonas[i].gz.text = zonas[i].nombre;
        }
    }

    public void Continuemos()
    {
        esperandoAccion = false;
        esperandoCarga = false;
        esperandoObjetivo = false;
        esperandoReaccion = false;
        esperandoZona = false;
    }

    //Funcion de entrada al combate, llamada por roguelike y escarmuza managers, y pasando quienes van a integrar el combate
    //Mas adelante tambien van a pasar las zonas que va a tener este
    public void NuevoCombate(List<cPersonajeFlyweight> combatientes)
    {
        InstanciarZonas(); //Funcion temporal, mientras no estemos pasando dinamicamente zonas distintas para cada combate
        InstanciarCombatientes(combatientes); // Trasnforma a los flywight en personajes posta

        EsperandoOkOn(true); // Esto pone el flag para que update empieza a manejar el input, y si el  jugador esta listo para la proxima cosa
        stateID = INICIANDO_RONDA; // Esto nos dice que hacer en el update

        //Estos no estoy seguro que sean necesarios
        rondaActual = 0;
        auto = false;
        listos = true;
        play = true;

        //UI
        uiC.CombateSelected();
        uiC.SetText("¡Nuevo Combate!");
        uiC.SetNombres(personajes);
        //Old Gui, por personaje
        SetUpGC();
    }

    public void EsperandoOkOn(bool on)
    {
        esperandoOK = on;
        if (on) py.SwitchCurrentActionMap("EsperandoOk");
        else py.SwitchCurrentActionMap("EsperandoSelect");
    }

    public void InstanciarZonas()
    {
        //Zonas Hardcodeadas
        string[] zCode = {"0_Patio_1_1_2_1_2",
                          "1_Escaleras_2_0_2_2_0_2",
                          "2_Terraza_1_1_2_0_1",};
        SetUpZonas(zCode);
    }

    public void InstanciarCombatientes(List<cPersonajeFlyweight> combatientes)
    {
        for (int i = 0; i < combatientes.Count; i++)
        {
            GameObject nuevoPer;
            if (combatientes[i].esMaton)
            {
                nuevoPer = Instantiate(matonPrefab, new Vector3(1, -1, 0), Quaternion.identity);
                cMatones script = nuevoPer.GetComponent<cMatones>();
                script.cantidadInicial = combatientes[i].cantidad;
                script.cantidad = script.cantidadInicial;
                script.modGuardiaDeMaton = combatientes[i].modGuardiaDeMaton;
                personajes.Add(script.NuevoPersonaje(combatientes[i]));
            }
            else
            {
                nuevoPer = Instantiate(personajePrefab, new Vector3(1, -1, 0), Quaternion.identity);
                personajes.Add(nuevoPer.GetComponent<cPersonaje>().NuevoPersonaje(combatientes[i]));
            }
            nuevoPer.transform.parent = this.transform;
            switch (combatientes[i].equipo)
            {
                case 1:
                    nuevoPer.GetComponent<MeshRenderer>().material = materialEquipo1;
                    break;
                case 2:
                    nuevoPer.GetComponent<MeshRenderer>().material = materialEquipo2;
                    break;
                case 3:
                    nuevoPer.GetComponent<MeshRenderer>().material = materialEquipo3;
                    break;
                case 4:
                    nuevoPer.GetComponent<MeshRenderer>().material = materialEquipo4;
                    break;
                case 5:
                    nuevoPer.GetComponent<MeshRenderer>().material = materialEquipo5;
                    break;
                case 6:
                    nuevoPer.GetComponent<MeshRenderer>().material = materialEquipo6;
                    break;
                default:
                    break;
            }
            personajes[i].zonaActual = Random.Range(0, 3);
            personajes[i].transform.position = new Vector3(personajes[i].zonaActual * 10 - 10, 0, i * 4.5f - 12);
            personajes[i].SetAI(combatientes[i].iA);
            personajes[i].SetArma(combatientes[i].arma);
        }
    }

    public void VaciarCombate()
    {
        foreach (var item in personajes)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in zonas)
        {
            Destroy(item.gameObject);
        }
        personajes.Clear();
        zonas.Clear();
        play = false;
        listos = false;
        acciones.Clear();
        accionesActivas.Clear();
        accionesReactivas.Clear();
        faseActual = 0;
        rondaActual = 0;
        uiC.SetText("");
    }

    public bool ZonaEsteEnRangoDePersonaje(cPersonaje per, int zona)
    {
        if (per.zonaActual == zona) return true;
        if (per.arma.GetDeRango())
        {
            //if (per.arma is cArmasFuego)
            //{
            //    if (!(per.arma as cArmasFuego).cargada) return false;
            //}

            Debug.Log("rango en zona este en rango");
            foreach (var item in zonas[per.zonaActual].zonasEnRango)
            {
                Debug.Log("zona en rango: " + item);
                Debug.Log("zona que buscamos: " + (zona));
                if (item == zona)
                {
                    Debug.Log("return true, deberia tener reaccion disponible");
                    return true;
                }
            }
        }
        else if (per.arma is cArmasPelea)
        {
            if ((per.arma as cArmasPelea).armaImprovisadaActiva)
            {
                foreach (var item in zonas[per.zonaActual].zonasEnRango)
                {
                    Debug.Log("zona en rango: " + item);
                    Debug.Log("zona que buscamos: " + (zona));
                    if (item == zona)
                    {
                        Debug.Log("return true, deberia tener reaccion disponible");
                        return true;
                    }
                }
            }

        }
        return false;
    }
}