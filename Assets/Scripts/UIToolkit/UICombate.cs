using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UICombate : MonoBehaviour
{
    public string perCambio;

    public cAccionable acc;

    public Camera cam;
    public cCombate combate; //idealmente esto no estaria aca, ya veremos com ose saca, pero por ahroa lo necesito para decirle que los botones manden 

    private VisualElement pCombate;

    private VisualElement menuAccion;
    private VisualElement menuMarcial;
    private VisualElement menuArcana;
    private VisualElement menuMover;
    private VisualElement menuBackOnly;
    private VisualElement menuReaccion;
    private VisualElement menuIntervenir;
    private VisualElement menuDrama;
    private VisualElement fases;

    private Button bAvanzar;

    private Button bMarcial;
    private Button bArcana;
    private Button bMover;
    private Button bGuardar;

    private Button bAtacar;
    private Button bAtacarImprovisada;
    private Button bRecargar;
    private Button bEncontrarImprovisada;
    private Button bMarcialAtras;

    private Button bAMarcial1;
    private Button bAMarcial2;
    private Button bAMarcial3;

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

    private Button bRMarcial1;
    private Button bRMarcial2;
    private Button bRMarcial3;

    private Button bAtrasSolo;

    private Button bDrama;
    private Button bNoDrama;

    private Label texto;
    private Label ronda;

    private VisualElement pPause;
    private Button bReanudar;
    private Button bSalir;

    public VisualElement pTradicioNMarcial;

    public List<List<Label>> iniciativa;
    public bool esperandoZona;
    public bool esperandoPersonaje;

    Label tooltip;
    bool hovering;
    float hoverTimer;
    const float buttonTimeTillTooltip = 0.75f;
    const float labelTimeTillTooltip = 0.2f;
    float timeTillTooltip;

    VisualElement infoVital;
    VisualElement infoTactica;
    VisualElement infoCompleta;

    VisualElement infoCompletaTM;

    Label infoVNombre;
    Label infoVHerCan;
    Label infoVDaño;
    Label infoVDrama;
    Label infoVGuardia;

    Label infoTNombre;
    Label infoTHerCan;
    Label infoTDrama;
    Label infoTGuardia;
    Button infoTArma;
    Label infoTBonus;
    Label infoTSed;
    Label infoTDaño;

    public Label zona1;
    public Label zona2;
    public Label zona3;
    public Label zona4;
    public Label zona5;

    private Slider volume;

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
        menuDrama = root.Q<VisualElement>("MenuDrama");

        bAvanzar = root.Q<Button>("ButtonAvanzar");

        bMarcial = root.Q<Button>("ButtonMarcial");
        bArcana = root.Q<Button>("ButtonArcana");
        bMover = root.Q<Button>("ButtonMover");
        bGuardar = root.Q<Button>("ButtonGuardar");

        bAtacar = root.Q<Button>("ButtonAtacar");
        bAtacarImprovisada = root.Q<Button>("ButtonAtacarImpro");
        bEncontrarImprovisada = root.Q<Button>("ButtonConseguirImpro");
        bRecargar = root.Q<Button>("ButtonRecargar");
        bMarcialAtras = root.Q<Button>("ButtonMarcialAtras");

        bAMarcial1 = root.Q<Button>("ButtonATMarcial1");
        bAMarcial2 = root.Q<Button>("ButtonATMarcial2");
        bAMarcial3 = root.Q<Button>("ButtonATMarcial3");

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

        bRMarcial1 = root.Q<Button>("ButtonDTMarcial1");
        bRMarcial2 = root.Q<Button>("ButtonDTMarcial2");
        bRMarcial3 = root.Q<Button>("ButtonDTMarcial3");

        bDrama = root.Q<Button>("ButtonDrama");
        bNoDrama = root.Q<Button>("ButtonNoDrama");

        bAtrasSolo = root.Q<Button>("ButtonAtrasSolo");

        pPause = root.Q<VisualElement>("PantallaPausa");
        bReanudar = root.Q<Button>("PausaReanudar");
        bSalir = root.Q<Button>("PausaMenu");

        pTradicioNMarcial = root.Q<VisualElement>("PantallaTradicionMarcial");

        tooltip = root.Q<Label>("tooltip");

        infoVital = root.Q<VisualElement>("InfoVitalNueva");
        infoTactica = root.Q<VisualElement>("InfoTacticaNueva");
        //infoCompleta = root.Q<VisualElement>("InfoCompleta");
        infoCompleta = root.Q<VisualElement>("InfoCompletaNueva");
        
        //infoCompletaTM = root.Q<VisualElement>("InfoCompletaTM");
        infoCompletaTM = root.Q<VisualElement>("InfoCompletaNueva");

        infoVNombre = infoVital.ElementAt(0) as Label;
        infoVHerCan = infoVital.ElementAt(1) as Label;
        infoVDaño = infoVital.ElementAt(2) as Label;
        infoVDrama = infoVital.ElementAt(3) as Label;
        infoVGuardia = infoVital.ElementAt(4) as Label;

        infoTNombre = infoTactica.ElementAt(0) as Label;
        infoTHerCan = infoTactica.ElementAt(1).ElementAt(0) as Label;
        infoTDaño = infoTactica.ElementAt(1).ElementAt(1) as Label;
        infoTDrama = infoTactica.ElementAt(1).ElementAt(2) as Label;
        infoTGuardia = infoTactica.ElementAt(1).ElementAt(3) as Label;
        infoTBonus = infoTactica.ElementAt(2).ElementAt(0) as Label;
        infoTSed = infoTactica.ElementAt(2).ElementAt(1) as Label;
        infoTArma = infoTactica.ElementAt(2).ElementAt(2) as Button;



        zona1 = root.Q<Label>("Zona1");
        zona2 = root.Q<Label>("Zona2");
        zona3 = root.Q<Label>("Zona3");
        zona4 = root.Q<Label>("Zona4");
        zona5 = root.Q<Label>("Zona5");

        volume = root.Q<Slider>("VolumeSlider");

        volume.value = combate.music.volume*100;
        volume.RegisterValueChangedCallback(v =>
        {
            combate.music.volume = v.newValue / 100;
        });

        bAvanzar.RegisterCallback<ClickEvent>(OnAvanzarClicked);

        bMarcial.RegisterCallback<ClickEvent>(OnMarcialClicked);
        bMarcial.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bMarcial.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
        bArcana.RegisterCallback<ClickEvent>(OnArcanaClicked);
        bArcana.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bArcana.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
        bMover.RegisterCallback<ClickEvent>(OnMoverClicked);
        bMover.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bMover.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
        bGuardar.RegisterCallback<ClickEvent>(OnGuardarClicked);
        bGuardar.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bGuardar.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);

        bAtacar.RegisterCallback<ClickEvent>(OnAtacarClicked);
        bAtacar.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bAtacar.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
        bAtacarImprovisada.RegisterCallback<ClickEvent>(OnImprovisadaClicked);
        bAtacarImprovisada.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bAtacarImprovisada.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
        bEncontrarImprovisada.RegisterCallback<ClickEvent>(OnEncontrarClicked);
        bEncontrarImprovisada.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bEncontrarImprovisada.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
        bRecargar.RegisterCallback<ClickEvent>(OnRecargarClicked);
        bRecargar.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bRecargar.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
        bMarcialAtras.RegisterCallback<ClickEvent>(OnAtrasClicked);
        bMarcialAtras.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bMarcialAtras.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);

        bAMarcial1.RegisterCallback<ClickEvent>(OnAMarcial1Clicked);
        bAMarcial1.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bAMarcial1.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
        bAMarcial2.RegisterCallback<ClickEvent>(OnAMarcial2Clicked);
        bAMarcial2.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bAMarcial2.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
        bAMarcial3.RegisterCallback<ClickEvent>(OnAMarcial3Clicked);
        bAMarcial3.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bAMarcial3.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);

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

        bRMarcial1.RegisterCallback<ClickEvent>(OnRMarcial1Clicked);
        bRMarcial1.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bRMarcial1.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
        bRMarcial2.RegisterCallback<ClickEvent>(OnRMarcial2Clicked);
        bRMarcial2.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bRMarcial2.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
        bRMarcial3.RegisterCallback<ClickEvent>(OnRMarcial3Clicked);
        bRMarcial3.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        bRMarcial3.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);

        bDrama.RegisterCallback<ClickEvent>(OnDramaClicked);
        RegisterTooltip(bDrama);
        bNoDrama.RegisterCallback<ClickEvent>(OnNoDramaClicked);
        RegisterTooltip(bNoDrama);

        bAtrasSolo.RegisterCallback<ClickEvent>(OnAtrasClicked);
        RegisterTooltip(bAtrasSolo);

        bReanudar.RegisterCallback<ClickEvent>(OnReanudarClicked);
        bSalir.RegisterCallback<ClickEvent>(OnSalirClicked);

        infoCompletaTM.ElementAt(2).ElementAt(1).ElementAt(0).RegisterCallback<ClickEvent>(OnTradicionMarcialClicked);

        RegisterTooltip(infoTHerCan);
        RegisterTooltip(infoTDaño);
        RegisterTooltip(infoTDrama);
        RegisterTooltip(infoTGuardia);
        RegisterTooltip(infoTBonus);
        RegisterTooltip(infoTSed);
        RegisterTooltip(infoTArma);     

        texto = root.Q<Label>("Texto");
        ronda = root.Q<Label>("IniRonda");

        iniciativa = new List<List<Label>>();

        for (int i = 0; i < infoCompleta.ElementAt(0).childCount; i++)
        {
            RegisterTooltip(infoCompleta.ElementAt(0).ElementAt(i));
        }
        for (int i = 0; i < infoCompleta.ElementAt(1).childCount; i++)
        {
            RegisterTooltip(infoCompleta.ElementAt(1).ElementAt(i));
        }

        VisualElement atributos = infoCompleta.ElementAt(2).ElementAt(0).ElementAt(1);
        VisualElement habilidades = infoCompleta.ElementAt(2).ElementAt(1).ElementAt(1);
        VisualElement arma = infoCompleta.ElementAt(2).ElementAt(1).ElementAt(0);
        RegisterTooltip(arma);

        int numeroDeHabilidades = 2;
        Label[] habValores = new Label[numeroDeHabilidades];
        for (int i = 0; i < numeroDeHabilidades; i++)
        {
            RegisterTooltip(habilidades.ElementAt(i));
        }

        int numeroDeAtributos = 5;
        Label[] atrValores = new Label[numeroDeAtributos];
        for (int i = 0; i < numeroDeAtributos; i++)
        {
            RegisterTooltip(atributos.ElementAt(i));
        }


        for (int i = 0; i < infoCompletaTM.ElementAt(0).childCount; i++)
        {
            RegisterTooltip(infoCompletaTM.ElementAt(1).ElementAt(i));
        }
        (infoCompletaTM.ElementAt(1).ElementAt(1).ElementAt(2) as Label).tooltip = "Dados que esta sumando Sed de Sangre a todas sus tiradas.";
        for (int i = 0; i < infoCompletaTM.ElementAt(1).childCount; i++)
        {
            RegisterTooltip(infoCompletaTM.ElementAt(2).ElementAt(i));
        }

        atributos = infoCompletaTM.ElementAt(2).ElementAt(0).ElementAt(1);
        habilidades = infoCompletaTM.ElementAt(2).ElementAt(1).ElementAt(1);
        arma = infoCompletaTM.ElementAt(2).ElementAt(1).ElementAt(0);
        RegisterTooltip(arma);
        RegisterTooltip(infoCompletaTM.ElementAt(2).ElementAt(0).ElementAt(0));
        infoCompletaTM.ElementAt(2).ElementAt(0).ElementAt(0).tooltip = "Las aptitudes naturales del personaje.";

        numeroDeHabilidades = 6;
        habValores = new Label[numeroDeHabilidades];
        habilidades.ElementAt(2).tooltip = "Pasiva. Cada vez que vos o un aliado derribe más de un Matón o cause más de una Herida en un solo Ataque a alguien bajo el efecto de una de tus Habilidades de Combate, tirás 1 dado Adicional en tus tiradas de Combate, Daño y Heridas hasta el final de la ronda por cada Punto en Sed de Sangre.";
        habilidades.ElementAt(3).tooltip = "Defensa. Hasta el final de la ronda, los dados del objetivo no explotan en sus tiradas de Heridas o todos matan un Maton adicional contra el.";
        habilidades.ElementAt(4).tooltip = "Acción. Con Brío contra Brío. Hasta el final de la ronda, los Ataques del objetivo no generan tiradas de Heridas, y en las tiradas de Daño contra el, los 9 también explotan o se derriba un Matón adicional.";
        habilidades.ElementAt(5).tooltip = "Habilidad Civil Marcial. Determina cuantos dados adicionales se tiran al repetir una tirada.";
        for (int i = 0; i < numeroDeHabilidades; i++)
        {
            RegisterTooltip(habilidades.ElementAt(i));
        }

        numeroDeAtributos = 5;
        atrValores = new Label[numeroDeAtributos];
        for (int i = 0; i < numeroDeAtributos; i++)
        {
            RegisterTooltip(atributos.ElementAt(i));
        }
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
                float wProp = 1920.0f / Screen.width;
                tooltip.transform.position = new Vector3(Mathf.Min(tooltip.transform.position.x, (Screen.width - 750) * wProp), tooltip.transform.position.y + 100, tooltip.transform.position.z);
            }
        }
    }

    public void ABBonusChangeChangeHandler(int newVal)
    {
        //info tactica y completa
        Debug.Log("per selec: " + combate.perSeleccionado + ", this per: " + infoTNombre.text);
        if (combate.perSeleccionado == perCambio)
        {
            Debug.Log("bonus change");
            infoTBonus.text = "Bonus: " + newVal;
            Label bonus =  infoCompleta.ElementAt(1).ElementAt(1).ElementAt(1) as Label;
            bonus.text = "Bonus: " + newVal;
        }
    }

    public void DañoChangeHandler(int newVal)
    {
        //info vital, tactica y completa
        if (combate.perSeleccionado == perCambio)
        {
            Debug.Log("daño change");
            infoTDaño.text = "Daño: " + newVal;
            Label d = infoCompleta.ElementAt(1).ElementAt(0).ElementAt(1) as Label;
            d.text = "Daño: " + newVal;
        }
        if (combate.perHovereado == perCambio)
        {
            infoVDaño.text = "Daño: " + newVal;
        }
    }

    public void DadosDeSedDeSangreChangeHandler(int newVal)
    {
        //info tactica y completa
        Debug.Log("sed change, selec: " + combate.perSeleccionado + " cambio: " + perCambio);
        if (combate.perSeleccionado == perCambio)
        {
            Debug.Log("sed y voluntad");
            infoTSed.text = "Ds Sed de Sangre: " + newVal;
            Label sed = infoCompleta.ElementAt(1).ElementAt(0).ElementAt(2) as Label;
            sed.text = "Ds Sed de Sangre: " + newVal;
        }
    }

    public void HeridasChangeHandler(int newVal)
    {
        //info vital, tactica y completa
        if (combate.perSeleccionado == perCambio)
        {
            Debug.Log("heridas change");
            infoTHerCan.text = "Heridas: " + newVal;

            Label h = infoCompleta.ElementAt(1).ElementAt(0).ElementAt(1) as Label;
            h.text = "Heridas: " + newVal;
        }
        if (combate.perHovereado == perCambio)
        {
            infoVHerCan.text = "Heridas: " + newVal;
        }
    }

    public void DramaChangeHandler(bool newVal)
    {
        //info vital, tactica y completa
        string newStr;

        if (newVal) newStr = "Drama - Sí";
        else newStr = "Drama - No";
        if (combate.perSeleccionado == perCambio)
        {
            Debug.Log("drama change");
            infoTDrama.text = newStr;
            Label d = infoCompleta.ElementAt(1).ElementAt(0).ElementAt(2) as Label;
            d.text = newStr;
        }
        if (combate.perHovereado == perCambio)
        {
            infoVDrama.text = newStr;
        }
    }

    public void CantidadChangeHandler(int newVal)
    {
                //info vital, tactica y completa
        if (combate.perSeleccionado == perCambio)
        {
            Debug.Log("cantidad change");
            infoTHerCan.text = "Cantidad: " + newVal;
            Label h = infoCompleta.ElementAt(1).ElementAt(0).ElementAt(1) as Label;
            h.text = "Cantidad: " + newVal;
        }
        if (combate.perHovereado == perCambio)
        {
            infoVHerCan.text = "Cantidad: " + newVal;
        }
    }

    void RegisterTooltip(Button b)
    {
        b.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        b.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
    }

    void RegisterTooltip(VisualElement vE)
    {
        vE.RegisterCallback<MouseEnterEvent>(OnMouseEnterVE);
        vE.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
    }

    public void OnMouseEnterButton(MouseEnterEvent evt)
    {
        timeTillTooltip = buttonTimeTillTooltip;
        hovering = true;
        //tooltip.transform.position = (evt.target as Button).transform.position;
        tooltip.transform.position = evt.mousePosition - evt.localMousePosition;
        tooltip.text = (evt.target as Button).tooltip;
    }

    public void OnMouseEnterVE(MouseEnterEvent evt)
    {
        timeTillTooltip = labelTimeTillTooltip;
        hovering = true;
        tooltip.transform.position = evt.mousePosition - evt.localMousePosition;
        tooltip.text = (evt.target as VisualElement).tooltip;
    }

    public void OnMouseLeaveButtonOrElement(MouseLeaveEvent evt)
    {
        tooltip.style.display = DisplayStyle.None;
        hoverTimer = 0;
        hovering = false;
    }

    public void EsconderBotones()
    {
        bAtacar.style.display = DisplayStyle.None;
        bMoverAgro.style.display = DisplayStyle.None;
        bMoverImpro.style.display = DisplayStyle.None;
        bRecargar.style.display = DisplayStyle.None;
        bEncontrarImprovisada.style.display = DisplayStyle.None;
        bAtacarImprovisada.style.display = DisplayStyle.None;
        bAMarcial1.style.display = DisplayStyle.None;
        bAMarcial2.style.display = DisplayStyle.None;
        bAMarcial3.style.display = DisplayStyle.None;
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
        combate.EsperandoOkOn(false);
        SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + ": que vas a hacer?");
        menuAccion.style.display = DisplayStyle.Flex;
        bMarcial.style.display = DisplayStyle.Flex;
        bMover.style.display = DisplayStyle.Flex;
        EsconderBotones();
        VerQueBotonesPonemos(cAcciones.AC_CAT_GUARDAR, personaje.acciones);
    }

    public void PedirReaccion(cPersonaje personaje)
    {
        combate.EsperandoOkOn(false);
        menuReaccion.style.display = DisplayStyle.Flex;
        string text = UIInterface.NombreDePersonajeEnNegrita(personaje) + ": ¿Queres intervenir contra ";
        if (combate.atacando)
        {
            text += UIInterface.IntEnNegrita(combate.jugadorAtq);
        }
        else
        {
            text += UIInterface.IntEnNegrita(combate.personajeActivo.GetGuardia());
        }
        bReaccionar.style.display = DisplayStyle.Flex;
        text += "?";
        SetText(text);
        bNoIntervenir.style.display = DisplayStyle.Flex;
    }

    public void PedirIntervencion(cPersonaje personaje)
    {
        combate.EsperandoOkOn(false);
        menuReaccion.style.display = DisplayStyle.None;
        menuIntervenir.style.display = DisplayStyle.Flex;
        string text = UIInterface.NombreDePersonajeEnNegrita(personaje) + ": ¿Como intervenimos?";
        SetText(text);
        if (combate.personajeInterversor.arma is cArmasPelea)
        {
            if (combate.personajeInterversor.GetZonaActual() == combate.personajeActivo.GetZonaActual() || combate.personajeInterversor.GetZonaActual() == combate.personajeObjetivo.GetZonaActual()) // si la defensa podria hacerse melee, ni mostramos lo de improv
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

        // ok, ta mal esto. ver como podemos adaptar la version de marcial aca. 
        if (personaje.tieneTradicionMarcial)
        {
            VerQueBotonesPonemos(personaje.reacciones);
            //bRMarcial1.style.display = DisplayStyle.Flex;
        }
        else
        {
            bRMarcial1.style.display = DisplayStyle.None;
            bRMarcial2.style.display = DisplayStyle.None;
            bRMarcial3.style.display = DisplayStyle.None;
        }
    }

    private void OnAMarcial1Clicked(ClickEvent evt)
    {
        int habilidadCode = 0;
        if (combate.personajeActivo.arma is cLaVoluntadDelCreador) { habilidadCode = 4; combate.accionActiva = cPersonaje.AC_IRADIVINA; }
        else if (combate.personajeActivo.arma is cJaieiy) { habilidadCode = 2; combate.accionActiva = cPersonaje.AC_IMPONER; }
        Debug.Log("mar click");
        SetText("¡" + UIInterface.NombreDePersonajeEnNegrita(combate.personajeActivo) + " va a usar " + cArma.GetHabilidadString(combate.personajeActivo.armaCode,habilidadCode) + "!");
     

        bAtacarImprovisada.style.display = DisplayStyle.None;
        menuMarcial.style.display = DisplayStyle.None;
        menuBackOnly.style.display = DisplayStyle.Flex;
        bEncontrarImprovisada.style.display = DisplayStyle.None;
        combate.esperandoObjetivo = true;
        combate.movPrec = false;
        combate.movAgro = false;
        combate.atacando = false;
        VolverAlCombate();
    }

    private void OnAMarcial2Clicked(ClickEvent evt)
    {
        Debug.Log("mar click");
        DejarDePedirIntervencion();
        SetText("¡" + UIInterface.NombreDePersonajeEnNegrita(combate.personajeInterversor) + " va a intervenir con HABILIDAD NUEVA!");
        combate.stateID = cCombate.RESOLVIENDO_REACCION;
        combate.reaccionActiva = cPersonaje.DB_DefensaTerrorDeDios;
        VolverAlCombate();
    }

    private void OnAMarcial3Clicked(ClickEvent evt)
    {
        Debug.Log("mar click");
        DejarDePedirIntervencion();
        SetText("¡" + UIInterface.NombreDePersonajeEnNegrita(combate.personajeInterversor) + " va a intervenir con HABILIDAD NUEVA!");
        combate.stateID = cCombate.RESOLVIENDO_REACCION;
        combate.reaccionActiva = cPersonaje.DB_DefensaTerrorDeDios;
        VolverAlCombate();
    }
    private void OnRMarcial1Clicked(ClickEvent evt)
    {
        int habilidadCode = 0;
        //if (combate.personajeInterversor.tieneTradicionMarcial) bRMarcial1.tooltip = item as cAccionDeImpacto).reglas;
        if (combate.personajeInterversor.arma is cLaVoluntadDelCreador) { habilidadCode = 3; combate.reaccionActiva = cPersonaje.DB_DefensaTerrorDeDios; }
        else if (combate.personajeInterversor.arma is cJaieiy) { habilidadCode = 3; combate.reaccionActiva = cPersonaje.DB_NerviosDeAcero; }
        Debug.Log("kco)de: " + habilidadCode);
        DejarDePedirIntervencion();
        SetText("¡" + UIInterface.NombreDePersonajeEnNegrita(combate.personajeInterversor) + " va a intervenir con " + cArma.GetHabilidadString(combate.personajeActivo.armaCode, habilidadCode) + "!");
        combate.stateID = cCombate.RESOLVIENDO_REACCION;
        VolverAlCombate();
    }

    private void OnRMarcial2Clicked(ClickEvent evt)
    {
        Debug.Log("mar click");
        DejarDePedirIntervencion();
        SetText("¡" + UIInterface.NombreDePersonajeEnNegrita(combate.personajeInterversor) + " va a intervenir con HABILIDAD NUEVA!");
        combate.stateID = cCombate.RESOLVIENDO_REACCION;
        combate.reaccionActiva = cPersonaje.DB_DefensaTerrorDeDios;
        VolverAlCombate();
    }

    private void OnRMarcial3Clicked(ClickEvent evt)
    {
        Debug.Log("mar click");
        DejarDePedirIntervencion();
        SetText("¡" + UIInterface.NombreDePersonajeEnNegrita(combate.personajeInterversor) + " va a intervenir con HABILIDAD NUEVA!");
        combate.stateID = cCombate.RESOLVIENDO_REACCION;
        combate.reaccionActiva = cPersonaje.DB_DefensaTerrorDeDios;
        VolverAlCombate();
    }

    private void VerQueBotonesPonemos(int categoria, List<cAcciones> acciones)
    {
        Debug.Log("ver que botones");
        foreach (var item in acciones)
        {
            Debug.Log(item.nombre);
            if (item.categoria != categoria)
            {
                Debug.Log("otra categoria");
                item.boton.style.display = DisplayStyle.None;
            }
            else
            {
                item.RevisarLegalidad();
                if (item.esLegal)
                {
                    Debug.Log("legal");
                    item.boton.style.display = DisplayStyle.Flex;
                    item.boton.style.backgroundImage = item.icon;
                    item.boton.tooltip = item.reglas; // este sobreescirbie lo del tamaño de armas, ver como implementar
                }
                else
                {
                    Debug.Log("ilegal");
                    item.boton.style.display = DisplayStyle.None;
                }
            }
        }
    }

    private void VerQueBotonesPonemos(List<cReacciones> reacciones)
    {
        Debug.Log("ver que botones");
        foreach (var item in reacciones)
        {

                item.RevisarLegalidad();
            Debug.Log(item.nombre);
            if (item.esLegal)
                {
                    item.boton.style.display = DisplayStyle.Flex;
                    item.boton.style.backgroundImage = item.icon;
                    item.boton.tooltip = item.reglas;// este sobreescirbie lo del tamaño de armas, ver como implementar
            }
                else
                {
                    item.boton.style.display = DisplayStyle.None;
                }
        }
    }

    public void PedirMarcial(cPersonaje personaje)
    {
        combate.EsperandoOkOn(false);
        DejarDePedirAccion();
        SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + ": que habilidad usamos?");
        menuMarcial.style.display = DisplayStyle.Flex;
        VerQueBotonesPonemos(cAcciones.AC_CAT_MARCIAL, personaje.acciones);
        bMarcialAtras.style.display = DisplayStyle.Flex;
        if(!(personaje.tieneTradicionMarcial))
        {
            bAMarcial1.style.display = DisplayStyle.None;
            bAMarcial2.style.display = DisplayStyle.None;
            bAMarcial3.style.display = DisplayStyle.None;
        }
    }

    public void PedirArcana(cPersonaje personaje)
    {
        combate.EsperandoOkOn(false);
        DejarDePedirAccion();
        SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + ": que habilidad usamos?");
        menuArcana.style.display = DisplayStyle.Flex;
        VerQueBotonesPonemos(cAcciones.AC_CAT_ARCANA, personaje.acciones);
        bArcanaAtras.style.display = DisplayStyle.Flex;
    }

    public void PedirMovimiento(cPersonaje personaje)
    {
        combate.EsperandoOkOn(false);
        DejarDePedirAccion();
        SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + ": como nos movemos?");
        menuMover.style.display = DisplayStyle.Flex;
        VerQueBotonesPonemos(cAcciones.AC_CAT_MOVIMIENTO, personaje.acciones);
        bMoverAtras.style.display = DisplayStyle.Flex;
    }

    public void PedirDrama()
    {
        combate.EsperandoOkOn(false);
        menuDrama.style.display = DisplayStyle.Flex;
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

    private void OnAvanzarClicked(ClickEvent evt)
    {
        if (!combate.esperandoObjetivo && !combate.esperandoZona && combate.esperandoOK)
            combate.AvanzarCombate();
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
        combate.EsperandoOkOn(true);
        SetText(UIInterface.NombreDePersonajeEnNegrita(combate.personajeInterversor) + " no intervendra.");
        combate.stateID = cCombate.RESOLVIENDO_ACCION;
        DejarDePedirReaccion();
        VolverAlCombate();
    }

    private void OnAtacarClicked(ClickEvent evt)
    {
        Debug.Log("atac click");
        combate.accionActiva = cPersonaje.AC_ATACAR;
        bAtacarImprovisada.style.display = DisplayStyle.None;
        menuMarcial.style.display = DisplayStyle.None;
        menuBackOnly.style.display = DisplayStyle.Flex;
        bEncontrarImprovisada.style.display = DisplayStyle.None;
        combate.esperandoObjetivo = true;
        combate.movPrec = false;
        combate.movAgro = false;
        VolverAlCombate();
    }

    private void OnImprovisadaClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_ATACARIMPRO;
        bAtacarImprovisada.style.display = DisplayStyle.None;
        menuMarcial.style.display = DisplayStyle.None;
        menuBackOnly.style.display = DisplayStyle.Flex;
        bEncontrarImprovisada.style.display = DisplayStyle.None;
        combate.esperandoObjetivo = true;
        combate.movPrec = false;
        combate.movAgro = false;
        VolverAlCombate();
    }

    private void OnEncontrarClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_ENCONTRAR;
        menuMarcial.style.display = DisplayStyle.None;
        bEncontrarImprovisada.style.display = DisplayStyle.None;
        combate.stateID = cCombate.RESOLVIENDO_ACCION;
        VolverAlCombate();
    }

    private void OnRecargarClicked(ClickEvent evt)
    {
        combate.accionActiva = cPersonaje.AC_RECARGAR;
        menuMarcial.style.display = DisplayStyle.None;
        bRecargar.style.display = DisplayStyle.None;
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

    private void OnDramaClicked(ClickEvent evt)
    {
        acc.UsaDrama();
        if (acc is cReaccionDefensa) (acc as cReaccionDefensa).pidiendoDrama = false;
        menuDrama.style.display = DisplayStyle.None;
    }

    private void OnNoDramaClicked(ClickEvent evt)
    {
        combate.EsperandoOkOn(true);
        if (acc is cReaccionDefensa) (acc as cReaccionDefensa).pidiendoDrama = false;
        menuDrama.style.display = DisplayStyle.None;
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
        combate.EsperandoOkOn(true);
        VolverAlCombate();
    }

    public void OnPersonajeClicked(cPersonaje p)
    {
        menuBackOnly.style.display = DisplayStyle.None;
        RegistrarAccion();
        combate.Ataque(p);
    }

    public void HideAtras()
    {
        menuBackOnly.style.display = DisplayStyle.None;
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
        SetText("¡" + UIInterface.NombreDePersonajeEnNegrita(combate.personajeInterversor) + " va a intervenir!");
        combate.stateID = cCombate.RESOLVIENDO_REACCION;
        combate.reaccionActiva = cPersonaje.DB_DefensaBasica;
        VolverAlCombate();
    }

    private void OnDefenderImproClicked(ClickEvent evt)
    {
        DejarDePedirIntervencion();
        SetText("¡" + UIInterface.NombreDePersonajeEnNegrita(combate.personajeInterversor) + " va a intervenir lanzando su arma improvisada!");
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

    public void OnTradicionMarcialClicked(ClickEvent evt)
    {
        foreach (var item in combate.personajes)
        {
            if (item.nombre == combate.perSeleccionado)
            {
                FillTradicionMarcial(item.armaCode); break;
            }
        }
        Deseleccionar();
        pTradicioNMarcial.style.display = DisplayStyle.Flex;
        //To do: llenar info
    }

    public void FillTradicionMarcial(int arma)
    {
        switch (arma)
        {
            case cArma.JAIEIY:
                cJaieiy.FillInfo(pTradicioNMarcial);
                break;
            case cArma.VOLUNTAD_CREADOR:
                cLaVoluntadDelCreador.FillInfo(pTradicioNMarcial);
                break;
            default:
                break;
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
        EsconderNombresDeZonas();
        Deseleccionar();
    }

    public void EsconderNombresDeZonas()
    {
        //necesitamos que sea un array
        //va, todavia mas importante, necesitamos que se generen dinamicamente para no hacerlos en la pantalla de ui
        zona1.style.display = DisplayStyle.None;
        zona2.style.display = DisplayStyle.None;
        zona3.style.display = DisplayStyle.None;
        zona4.style.display = DisplayStyle.None;
        zona5.style.display = DisplayStyle.None;
    }

    private void VolverAlCombate()
    {
        combate.AvanzarCombate();
    }

    private void OnMouseOver()
    {
    }

    public void MostrarInfoPerVital(cPersonaje per)
    {
        Debug.Log("info pv");
        //infoVital.transform.position = WorldToUIToolkit(per.transform.position, -(Screen.width/27) - Screen.width / 2, (Screen.height / 21));
        float wProp = 1920.0f / Screen.width;
        MyWorldToScreen(per.transform.position, infoVital, -Screen.width / 20.0f / wProp, -Screen.height / 20.0f);
        //-Screen.width/20, -Screen.height / 20
        infoVital.style.display = DisplayStyle.Flex;
        UIInterface.FillPlayerHoverNueva(per,infoVital);
        //LlenarInfoVital(per);
    }

    public void LlenarInfoVital(cPersonaje per)
    {
        Debug.Log("info v");
        infoVNombre.text = per.nombre;
        if (per.Drama)
        {
            infoVDrama.text = "Drama - Si";
        }
        else
        {
            infoVDrama.text = "Drama - No";
        }

        if (per is cMatones)
        {
            infoVHerCan.text = "Cantidad: " + (per as cMatones).Cantidad;
        }
        else
        {
            infoVHerCan.text = "Heridas: " + per.Heridas;
        }

        infoVGuardia.text = "Guardia: " + per.GetGuardia();
    }

    public void EsconderInfoPerVital()
    {
        infoVital.style.display = DisplayStyle.None;
    }

    public void Deseleccionar()
    {
        pTradicioNMarcial.style.display = DisplayStyle.None;
        infoVital.style.display = DisplayStyle.None;
        infoTactica.style.display = DisplayStyle.None;
        infoCompleta.style.display = DisplayStyle.None;
        infoCompletaTM.style.display = DisplayStyle.None;
        combate.perSeleccionado = null;
        foreach (var item in combate.personajes)
        {
            item.mostrandoTactica = false;
        }
    }

    public void MostrarInfoPerTactica(cPersonaje per)
    {
        Debug.Log("info tact");
        infoVital.style.display = DisplayStyle.None;
        infoCompleta.style.display = DisplayStyle.None;
        infoCompletaTM.style.display = DisplayStyle.None;
        infoTactica.style.display = DisplayStyle.Flex;
        //infoTactica.transform.position = WorldToUIToolkit(per.transform.position, -70 - Screen.width / 2, 50);
        //float wProp = 1920.0f / Screen.width;
       // MyWorldToScreen(per.transform.position, infoTactica, -Screen.width / 20.0f / wProp, -Screen.height / 20.0f);
        //-Screen.width/20, -Screen.height / 20
        //LlenarInfoTactica(per);
        UIInterface.FillPlayerTacticaNueva(per, infoTactica);
    }

    public static void MyWorldToScreen(Vector3 pos, VisualElement ui, float xOffset, float yOffset)
    {
        float wProp = 1920.0f / Screen.width;
        float hProp = 1080.0f / Screen.height;
        Vector3 screen = Camera.main.WorldToViewportPoint(pos);
        ui.style.left = (screen.x * Screen.width + xOffset) * wProp;
        ui.style.top = (Screen.height * (1 - screen.y) - yOffset) * hProp;
    }

    public void LlenarInfoTactica(cPersonaje per) //tendria que haber info tactica TM?
    {
        Debug.Log("info tact");
        infoTNombre.text = infoVNombre.text;
        infoTHerCan.text = infoVHerCan.text;
        infoTDrama.text = infoVDrama.text;
        infoTGuardia.text = infoVGuardia.text;

        infoTArma.text = per.arma.GetStringCorto();
        UIRoguelikeUpgrade.ArmaTooltip(per.armaCode, infoTArma, per.arma.maestria);
        infoTBonus.text = "Bonus: " + per.BonusPAtqBporDefB;
        if (per is cMatones) infoTDaño.style.display = DisplayStyle.None;
        else
        {
            infoTDaño.style.display = DisplayStyle.Flex;
            infoTDaño.text = "Daño: " + per.Daño;
        }
    }

    public void MostrarInfoPerCompleta(cPersonaje per)
    {
        infoTactica.style.display = DisplayStyle.None;
        infoCompleta.style.display = DisplayStyle.Flex;
        infoCompleta.style.top = 10;
        infoCompleta.style.right = 10;

        infoCompleta.ElementAt(1).ElementAt(1).ElementAt(1).style.display = DisplayStyle.Flex;

        if (per.tieneTradicionMarcial)
        {
            infoCompleta.ElementAt(1).ElementAt(1).ElementAt(2).style.display = DisplayStyle.Flex;
            infoCompleta.ElementAt(2).ElementAt(1).ElementAt(0).RegisterCallback<ClickEvent>(OnTradicionMarcialClicked);
        }
        else
        {
            infoCompleta.ElementAt(1).ElementAt(1).ElementAt(2).style.display = DisplayStyle.None;
            infoCompleta.ElementAt(2).ElementAt(1).ElementAt(0).UnregisterCallback<ClickEvent>(OnTradicionMarcialClicked);
        }
        UIInterface.FillPlayerNueva(per, infoCompleta);
    }

    public void MostrarArmaEnTooltip(string aBText, string dBtext, string movText, int tamaño)
    {
        string text = " - ";
        switch (tamaño)
        {
            case 0:
                text = " pequeña"; 
                break;
            case 1:
                text = " media";
                break;
            case 2:
                text = " grande";
                break;
            default:
                break;
        }
        text += " (-" + tamaño + "d Ataque, Mus mult: " + (tamaño + 2) + ")";
        bAtacarImprovisada.tooltip = aBText + text;
        bMoverImpro.tooltip = movText + text;
        bDefenderImpro.tooltip = dBtext + text;
    }

    private void OnDisable()
    {
        foreach (var item in combate.personajes)
        {
            item.OnABBonusChange -= ABBonusChangeChangeHandler;
            if (item is cMatones) (item as cMatones).OnCantidadChange -= CantidadChangeHandler;
            else
            {
                item.OnHeridasChange -= HeridasChangeHandler;
                item.OnDañoChange -= DañoChangeHandler;
                item.OnDramaChange -= DramaChangeHandler;
                if (item.arma is cLaVoluntadDelCreador) (item.arma as cLaVoluntadDelCreador).OnDadosDeSedDeSangreChange -= DadosDeSedDeSangreChangeHandler;
            }
        }
    }

}