using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UICombate : MonoBehaviour
{
    public cCombate combate; //idealmente esto no estaria aca, ya veremos com ose saca, pero por ahroa lo necesito para decirle que los botones manden 

    private VisualElement pCombate;

    private VisualElement menuAccion;
    private VisualElement menuMarcial;
    private VisualElement menuArcana;
    private VisualElement menuMover;
    private VisualElement menuBackOnly;
    private VisualElement menuReaccion;
    private VisualElement menuIntervenir;
    private VisualElement fases;

    private Button bMarcial;
    private Button bArcana;
    private Button bMover;
    private Button bGuardar;

    private Button bAtacar;
    private Button bAtacarImprovisada;
    private Button bRecargar;
    private Button bEncontrarImprovisada;
    private Button bMarcialAtras;

    private Button bArcanaAtras;

    private Button bMoverImpro;
    private Button bMoverPrec;
    private Button bMoverAgro;
    private Button bMoverAtras;

    private Button bReaccionar;
    private Button bNoIntervenir;

    private Button bDefenderImpro;
    private Button bDefender;
    private Button bIntervenirAtras;

    private Button bAtrasSolo;

    private Label texto;
    private Label ronda;

    private VisualElement pPause;
    private Button bReanudar;
    private Button bSalir;

    public List<List<Label>> iniciativa;
    public bool esperandoZona;
    public bool esperandoPersonaje;

    Label tooltip;
    bool hovering;
    float hoverTimer;
    const float timeTillTooltip = 0.75f;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        pCombate = root.Q<VisualElement>("PantallaCombate");

        fases = root.Q<VisualElement>("IndicadorFaseContainer");
        menuAccion = root.Q<VisualElement>("MenuAccion");
        menuMarcial = root.Q<VisualElement>("MenuMarcial");
        menuArcana = root.Q<VisualElement>("MenuArcana");
        menuBackOnly = root.Q<VisualElement>("MenuBackOnly");
        menuMover = root.Q<VisualElement>("MenuMovimiento");
        menuReaccion = root.Q<VisualElement>("MenuReaccion");
        menuIntervenir = root.Q<VisualElement>("MenuIntervenir");


        bMarcial = root.Q<Button>("ButtonMarcial");
        bArcana = root.Q<Button>("ButtonArcana");
        bMover = root.Q<Button>("ButtonMover");
        bGuardar = root.Q<Button>("ButtonGuardar");

        bAtacar = root.Q<Button>("ButtonAtacar");
        bAtacarImprovisada = root.Q<Button>("ButtonAtacarImpro");
        bEncontrarImprovisada = root.Q<Button>("ButtonConseguirImpro");
        bRecargar = root.Q<Button>("ButtonRecargar");
        bMarcialAtras = root.Q<Button>("ButtonMarcialAtras");

        bArcanaAtras = root.Q<Button>("ButtonArcanaAtras");

        bMoverImpro = root.Q<Button>("ButtonMoverImpro");
        bMoverPrec = root.Q<Button>("ButtonMoverPrec");
        bMoverAgro = root.Q<Button>("ButtonMoverAgro");
        bMoverAtras = root.Q<Button>("ButtonMoverAtras");

        bReaccionar = root.Q<Button>("ButtonIntervenir");
        bNoIntervenir = root.Q<Button>("ButtonNoIntervenir");

        bDefenderImpro = root.Q<Button>("ButtonDefenderImpro");
        bDefender = root.Q<Button>("ButtonDefender");
        bIntervenirAtras = root.Q<Button>("ButtonIntervenirAtras");

        bAtrasSolo = root.Q<Button>("ButtonAtrasSolo");

        pPause = root.Q<VisualElement>("PantallaPausa");
        bReanudar = root.Q<Button>("PausaReanudar");
        bSalir = root.Q<Button>("PausaMenu");

        tooltip = root.Q<Label>("tooltip");

        bMarcial.RegisterCallback<ClickEvent>(OnMarcialClicked);
        bMarcial.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bMarcial.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButton);
        bArcana.RegisterCallback<ClickEvent>(OnArcanaClicked);
        bArcana.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bArcana.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButton);
        bMover.RegisterCallback<ClickEvent>(OnMoverClicked);
        bMover.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bMover.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButton);
        bGuardar.RegisterCallback<ClickEvent>(OnGuardarClicked);
        bGuardar.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bGuardar.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButton);

        bAtacar.RegisterCallback<ClickEvent>(OnAtacarClicked);
        bAtacar.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bAtacar.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButton);
        bAtacarImprovisada.RegisterCallback<ClickEvent>(OnImprovisadaClicked);
        bAtacarImprovisada.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bAtacarImprovisada.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButton);
        bEncontrarImprovisada.RegisterCallback<ClickEvent>(OnEncontrarClicked);
        bEncontrarImprovisada.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bEncontrarImprovisada.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButton);
        bRecargar.RegisterCallback<ClickEvent>(OnRecargarClicked);
        bRecargar.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bRecargar.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButton);
        bMarcialAtras.RegisterCallback<ClickEvent>(OnAtrasClicked);
        bMarcialAtras.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bMarcialAtras.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButton);

        bArcanaAtras.RegisterCallback<ClickEvent>(OnAtrasClicked);
        RegisterTooltip(bArcanaAtras);

        bMoverImpro.RegisterCallback<ClickEvent>(OnMovImproClicked);
        RegisterTooltip(bMoverImpro);
        bMoverPrec.RegisterCallback<ClickEvent>(OnMovPrecClicked);
        RegisterTooltip(bMoverPrec);
        bMoverAgro.RegisterCallback<ClickEvent>(OnMovAgroClicked);
        RegisterTooltip(bMoverAgro);
        bMoverAtras.RegisterCallback<ClickEvent>(OnAtrasClicked);
        RegisterTooltip(bMoverAtras);

        bReaccionar.RegisterCallback<ClickEvent>(OnReaccionarClicked);
        RegisterTooltip(bReaccionar);
        bNoIntervenir.RegisterCallback<ClickEvent>(OnNoIntervenirClicked);
        RegisterTooltip(bNoIntervenir);

        bDefenderImpro.RegisterCallback<ClickEvent>(OnDefenderImproClicked);
        RegisterTooltip(bDefenderImpro);
        bDefender.RegisterCallback<ClickEvent>(OnDefenderClicked);
        RegisterTooltip(bDefender);
        bIntervenirAtras.RegisterCallback<ClickEvent>(OnIntervenirAtrasClicked);
        RegisterTooltip(bIntervenirAtras);

        bAtrasSolo.RegisterCallback<ClickEvent>(OnAtrasClicked);
        RegisterTooltip(bAtrasSolo);

        bReanudar.RegisterCallback<ClickEvent>(OnReanudarClicked);
        bSalir.RegisterCallback<ClickEvent>(OnSalirClicked);

        texto = root.Q<Label>("Texto");
        ronda = root.Q<Label>("IniRonda");

        iniciativa = new List<List<Label>>();
    }

    void Update()
    {
        if (hovering)
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= timeTillTooltip)
            {
                hoverTimer = 0;
                hovering = false;
                tooltip.style.display = DisplayStyle.Flex;
            }
        }
    }

    void RegisterTooltip(Button b)
    {
        b.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        b.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButton);
    }

    public void OnMouseEnterButton(MouseEnterEvent evt)
    {
        Debug.Log("enter");
        hovering = true;
        //tooltip.transform.position = (evt.target as Button).transform.position;
        tooltip.transform.position = evt.mousePosition;
        tooltip.text = (evt.target as Button).tooltip;
    }

    public void OnMouseLeaveButton(MouseLeaveEvent evt)
    {
        Debug.Log("leave");
        tooltip.style.display = DisplayStyle.None;
        hoverTimer = 0;
        hovering = false;
    }

    public void CombateSelected()
    {
        UIInterface.SetCurrent(pCombate);
    }

    public void SetText(string nuevoTexto)
    {
        if (texto != null) texto.text = nuevoTexto;
    }

    public void SetNombres(List<cPersonaje> personajes)
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        iniciativa.Clear();
        int i = 0;
        for (; i < personajes.Count; i++)
        {
            List<Label> uiIni = new List<Label>();
            string text = "IniJugador" + (i + 1) + "Nombre";
            uiIni.Add(root.Q<Label>(text));
            for (int j = 1; j < 11; j++)
            {
                text = "IniJugador" + (i + 1) + "Fase" + j;
                uiIni.Add(root.Q<Label>(text));
                uiIni[j].text = "-";
            }
            iniciativa.Add(uiIni);
            iniciativa[i][0].text = personajes[i].nombre;
            string nombreFila = "IniJugador" + (i + 1);
            VisualElement fila = root.Q<VisualElement>(nombreFila);
            fila.style.display = DisplayStyle.Flex;
        }
        for (; i < 6; i++)
        {
            string nombreFila = "IniJugador" + (i + 1);
            VisualElement fila = root.Q<VisualElement>(nombreFila);
            fila.style.display = DisplayStyle.None;
        }
    }

    public void ActualizarIniciativa(List<cPersonaje> personajes)
    {
        for (int i = 0; i < personajes.Count; i++)
        {
            int[] usados = new int[10];
            foreach (var valor in personajes[i].dadosDeAccion)
            {
                if (valor > 0 && valor < 11) usados[valor - 1]++; // Cada elemento en "Usados" toma nota de cuantos dados de accion tiene el personaje en esta fase
            }
            for (int j = 0; j < usados.Length; j++)
            {
                if (usados[j] > 0) // si el personaje tiene al menos 1 dado en esta fase 
                {
                    iniciativa[i][j + 1].text = (j + 1).ToString();
                    if (usados[j] > 1)
                        iniciativa[i][j + 1].text += "x" + usados[j];
                }
                else { iniciativa[i][j + 1].text = "-"; }
            }
        }
    }

    public void PedirAccion(cPersonaje personaje)
    {
        SetText(personaje.nombre + ": que vas a hacer?");
        menuAccion.style.display = DisplayStyle.Flex;
        bMarcial.style.display = DisplayStyle.Flex;
        bMover.style.display = DisplayStyle.Flex;
        VerQueBotonesPonemos(cAcciones.AC_CAT_GUARDAR, personaje.acciones);
    }

    public void PedirReaccion(cPersonaje personaje)
    {
        menuReaccion.style.display = DisplayStyle.Flex;
        string text = personaje.nombre + ": ¿Queres intervenir";
        bReaccionar.style.display = DisplayStyle.Flex;
        text += "?";
        SetText(text);
        bNoIntervenir.style.display = DisplayStyle.Flex;
    }

    public void PedirIntervencion(cPersonaje personaje)
    {
        menuReaccion.style.display = DisplayStyle.None;
        menuIntervenir.style.display = DisplayStyle.Flex;
        string text = personaje.nombre + ": ¿Como intervenimos?";
        SetText(text);
        if (combate.personajeInterversor.arma is cArmasPelea)
        {
            if (combate.personajeInterversor.zonaActual == combate.personajeActivo.zonaActual) // si la defensa podria hacerse melee, ni mostramos lo de improv
            {
                bDefender.style.display = DisplayStyle.Flex;
            }
            else if (combate.atacando)
            {
                if (combate.personajeInterversor.nombre == combate.personajeObjetivo.nombre) bDefender.style.display = DisplayStyle.Flex;
            }
            else if ((combate.personajeInterversor.arma as cArmasPelea).armaImprovisadaActiva)
            {
                bDefenderImpro.style.display = DisplayStyle.Flex;
            }
        }
        else
        {
            bDefender.style.display = DisplayStyle.Flex;
        }
        bIntervenirAtras.style.display = DisplayStyle.Flex;
    }

    private void VerQueBotonesPonemos(int categoria, List<cAcciones> acciones)
    {
        foreach (var item in acciones)
        {
            if (item.categoria != categoria)
            {
                item.boton.style.display = DisplayStyle.None;
            }
            else
            {
                item.RevisarLegalidad();
                if (item.esLegal)
                {
                    item.boton.style.display = DisplayStyle.Flex;
                }
                else item.boton.style.display = DisplayStyle.None;
            }
        }
    }

    public void PedirMarcial(cPersonaje personaje)
    {
        DejarDePedirAccion();
        SetText(personaje.nombre + ": que habilidad usamos?");
        menuMarcial.style.display = DisplayStyle.Flex;
        VerQueBotonesPonemos(cAcciones.AC_CAT_MARCIAL, personaje.acciones);
        bMarcialAtras.style.display = DisplayStyle.Flex;
    }

    public void PedirArcana(cPersonaje personaje)
    {
        DejarDePedirAccion();
        SetText(personaje.nombre + ": que habilidad usamos?");
        menuArcana.style.display = DisplayStyle.Flex;
        VerQueBotonesPonemos(cAcciones.AC_CAT_ARCANA, personaje.acciones);
        bArcanaAtras.style.display = DisplayStyle.Flex;
    }

    public void PedirMovimiento(cPersonaje personaje)
    {
        DejarDePedirAccion();
        SetText(personaje.nombre + ": como nos movemos?");
        menuMover.style.display = DisplayStyle.Flex;
        VerQueBotonesPonemos(cAcciones.AC_CAT_MOVIMIENTO, personaje.acciones);
        bMoverAtras.style.display = DisplayStyle.Flex;
    }

    public void DejarDePedirAccion()
    {
        bMarcial.style.display = DisplayStyle.None;
        bArcana.style.display = DisplayStyle.None;
        bMover.style.display = DisplayStyle.None;
        bGuardar.style.display = DisplayStyle.None;
        menuAccion.style.display = DisplayStyle.None;
    }

    public void DejarDePedirReaccion()
    {
        bReaccionar.style.display = DisplayStyle.None;
        bNoIntervenir.style.display = DisplayStyle.None;
        menuReaccion.style.display = DisplayStyle.None;
    }

    public void DejarDePedirIntervencion()
    {
        bDefender.style.display = DisplayStyle.None;
        bDefenderImpro.style.display = DisplayStyle.None;
        bIntervenirAtras.style.display = DisplayStyle.None;
        menuIntervenir.style.display = DisplayStyle.None;
    }

    public void SetRonda(int rondaNumero)
    {
        ronda.text = "Ronda: " + rondaNumero.ToString();
        for (int i = 1; i < 11; i++)
        {
            SacarFase(i);
        }
    }

    public void ResetRonda()
    {
        ronda.text = "Ronda:";
        for (int i = 1; i < 11; i++)
        {
            SacarFase(i);
        }
    }

    public void IrAFase(int fase)
    {
        string f = fase.ToString();
        SetText("Fase " + f);
        fases.AddToClassList("fase-" + f);
    }

    public void SacarFase(int fase)
    {
        fases.RemoveFromClassList("fase-" + fase.ToString());
    }

    private void OnMarcialClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_MARCIAL;
        menuAccion.style.display = DisplayStyle.None;
        menuMarcial.style.display = DisplayStyle.Flex;
        combate.AvanzarCombate();
    }

    private void OnArcanaClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_ARCANA;
        menuAccion.style.display = DisplayStyle.None;
        menuArcana.style.display = DisplayStyle.Flex;
        combate.AvanzarCombate();
    }

    private void OnNoIntervenirClicked(ClickEvent evt)
    {
        SetText(combate.personajeInterversor.nombre + " no intervendra.");
        combate.stateID = cCombate.RESOLVIENDO_ACCION;
        DejarDePedirReaccion();
        VolverAlCombate();
    }

    private void OnAtacarClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_ATACAR;
        menuMarcial.style.display = DisplayStyle.None;
        menuBackOnly.style.display = DisplayStyle.Flex;
        esperandoPersonaje = true;
        combate.movPrec = false;
        combate.movAgro = false;
        VolverAlCombate();
    }

    private void OnImprovisadaClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_ATACARIMPRO;
        menuMarcial.style.display = DisplayStyle.None;
        menuBackOnly.style.display = DisplayStyle.Flex;
        esperandoPersonaje = true;
        combate.movPrec = false;
        combate.movAgro = false;
        VolverAlCombate();
    }

    private void OnEncontrarClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_ENCONTRAR;
        menuMarcial.style.display = DisplayStyle.None;
        combate.stateID = cCombate.RESOLVIENDO_ACCION;
        VolverAlCombate();
    }

    private void OnRecargarClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_RECARGAR;
        menuMarcial.style.display = DisplayStyle.None;
        combate.stateID = cCombate.RESOLVIENDO_ACCION;
        VolverAlCombate();
    }

    private void OnMoverClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_MOVER;
        menuAccion.style.display = DisplayStyle.None;
        menuMover.style.display = DisplayStyle.Flex;
        combate.AvanzarCombate();
    }

    private void OnGuardarClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_GUARDAR;
        menuAccion.style.display = DisplayStyle.None;
        combate.stateID = cCombate.RESOLVIENDO_ACCION;
        VolverAlCombate();
    }


    private void OnAtrasClicked(ClickEvent evt)
    {
        //´pr ahora culauqier atras os manda al menu de accion... capaz lo ideal seria que nos manden "un menu arriba"
        // onda, el del menu movimiento y el de seleccionar objetivo de ataque, al menu de accion, pero el de seleccioanr mobjetivo de movimiento, al menu de movimiento
        combate.accionActiva = cPersonaje.AC_SINASIGNAR;
        menuMarcial.style.display = DisplayStyle.None;
        menuArcana.style.display = DisplayStyle.None;
        menuMover.style.display = DisplayStyle.None;
        menuBackOnly.style.display = DisplayStyle.None;
        menuAccion.style.display = DisplayStyle.Flex;
        VolverAlCombate();
    }


    private void OnMovImproClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_MOVIMPRO;
        menuMover.style.display = DisplayStyle.None;
        menuBackOnly.style.display = DisplayStyle.Flex;
        esperandoZona = true;
        combate.movAgro = true;
        combate.movPrec = false;
        VolverAlCombate();
    }

    private void OnMovPrecClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_MOVPREC;
        menuMover.style.display = DisplayStyle.None;
        menuBackOnly.style.display = DisplayStyle.Flex;
        esperandoZona = true;
        combate.movPrec = true;
        combate.movAgro = false;
        VolverAlCombate();
    }

    private void OnMovAgroClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_MOVAGRE;
        menuMover.style.display = DisplayStyle.None;
        menuBackOnly.style.display = DisplayStyle.Flex;
        esperandoZona = true;
        combate.movAgro = true;
        combate.movPrec = false;
        VolverAlCombate();
    }

    public void OnZonaclicked(int zonaIndex)
    {
        menuBackOnly.style.display = DisplayStyle.None;
        esperandoZona = false;
        combate.atacando = false;
        combate.zonaObjetiva = zonaIndex;
        combate.stateID = cCombate.RESOLVIENDO_ACCION;
        //if(combate.movAgro) combate.accionActiva = cPersonaje.AC_MOVAGRE;
        //else combate.accionActiva = cPersonaje.AC_MOVPREC;
        RegistrarAccion();
        VolverAlCombate();
    }

    public void OnPersonajeClicked(cPersonaje p)
    {
        menuBackOnly.style.display = DisplayStyle.None;
        esperandoPersonaje = false;
        combate.personajeObjetivo = p;
        combate.atacando = true;
        combate.stateID = cCombate.RESOLVIENDO_ACCION;
        if (combate.accionActiva == cPersonaje.AC_MOVAGRE) combate.accionActiva = cPersonaje.AC_ATACAR;
        else if (combate.accionActiva == cPersonaje.AC_MOVIMPRO) combate.accionActiva = cPersonaje.AC_ATACARIMPRO;

        RegistrarAccion();
        VolverAlCombate();
    }

    public void RegistrarAccion()
    {
        //se actuo
        foreach (var item in combate.ultimosEnActuar)
        {
            if (item.nombre == combate.personajeActivo.nombre)
            {
                combate.ultimosEnActuar.Remove(combate.personajeActivo);
                break;
            }
        }
        combate.ultimosEnActuar.Add(combate.personajeActivo);
        foreach (var item in combate.personajes)
        {
            item.guardando = false;
        }
    }


    private void OnReaccionarClicked(ClickEvent evt)
    {
        DejarDePedirReaccion();
        PedirIntervencion(combate.personajeInterversor);
    }

    private void OnDefenderClicked(ClickEvent evt)
    {
        DejarDePedirIntervencion();
        SetText("¡" + combate.personajeInterversor.nombre + " va a intervenir!");
        combate.stateID = cCombate.RESOLVIENDO_REACCION;
        combate.reaccionActiva = cPersonaje.DB_DefensaBasica;
        VolverAlCombate();
    }

    private void OnDefenderImproClicked(ClickEvent evt)
    {
        DejarDePedirIntervencion();
        SetText("¡" + combate.personajeInterversor.nombre + " va a intervenir lanzando su arma improvisada!");
        combate.stateID = cCombate.RESOLVIENDO_REACCION;
        combate.reaccionActiva = cPersonaje.DB_DefensaBasicaImpro;
        VolverAlCombate();
    }

    private void OnIntervenirAtrasClicked(ClickEvent evt)
    {
        DejarDePedirIntervencion();
        PedirReaccion(combate.personajeInterversor);
    }

    private void OnReanudarClicked(ClickEvent evt)
    {
        Reanudar();
    }

    public void Reanudar()
    {
        pPause.style.display = DisplayStyle.None;
        combate.Reanudar();
    }

    private void OnSalirClicked(ClickEvent evt)
    {
        combate.LimpiarCombate();
        LeaveCombat();
        pPause.style.display = DisplayStyle.None;
        UIInterface.GoMainMenu();
        if (combate.esRoguelike)
        {
            combate.rM.party.Clear();
            combate.rM.nivel = 1;
        }
    }

    public void Pause()
    {
        pPause.style.display = DisplayStyle.Flex;
    }

    public void LeaveCombat()
    {
        menuAccion.style.display = DisplayStyle.None;
        menuMarcial.style.display = DisplayStyle.None;
        menuArcana.style.display = DisplayStyle.None;
        menuMover.style.display = DisplayStyle.None;
        menuBackOnly.style.display = DisplayStyle.None;
        menuReaccion.style.display = DisplayStyle.None;
        menuIntervenir.style.display = DisplayStyle.None;
    }

    private void VolverAlCombate()
    {
        combate.AvanzarCombate();
    }

    private void OnMouseOver()
    {
        Debug.Log("mouse over");
    }
}