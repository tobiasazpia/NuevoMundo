using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
public class UIRoguelike : MonoBehaviour
{
    public PlayerInput py;
    Label tooltip;
    bool hovering;
    float hoverTimer;
    const float buttonTimeTillTooltip = 0.75f;
    const float labelTimeTillTooltip = 0.2f;
    float timeTillTooltip;

    public cRoguelikeManager rM;

    private VisualElement pRoguelikeIni;
    private VisualElement pRoguelikeArma;
    private VisualElement pRoguelikeListo;

    private Button bAtrasE;
    private Button bAtrasRI;
    private Button bAtrasRA;
    private Button bAtrasRL;
    private Button bArma1;
    private Button bArma2;
    private Button bEmpezar;

    private TextField tNombre;
    string nombreJugador = "placeholder";
    private Label lListo;

    int[][] choices = new int[2][];
    int[] eleccion;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        pRoguelikeIni = root.Q<VisualElement>("PantallaRoguelikeIni");
        pRoguelikeArma = root.Q<VisualElement>("PantallaRoguelikeArma");
        pRoguelikeListo = root.Q<VisualElement>("PantallaRoguelikeListo");

        bAtrasE = root.Q<Button>("AtrasEscarmuza"); // eh, atras escarmuza? como es esto?
        bAtrasRI = root.Q<Button>("AtrasRoguelikeIni");
        bAtrasRA = root.Q<Button>("AtrasRoguelikeArma");
        bAtrasRL = root.Q<Button>("AtrasRoguelikeListo");

        bArma1 = root.Q<Button>("Arma1");
        bArma2 = root.Q<Button>("Arma2");

        RegisterTooltip(bArma1);
        RegisterTooltip(bArma2);

        bEmpezar = root.Q<Button>("Empezar");

        tNombre = root.Q<TextField>("NombreInput");

        lListo = root.Q<Label>("mRoguelikeListo");

        tooltip = root.Q<Label>("tooltip");

        bAtrasE.RegisterCallback<ClickEvent>(OnBAtrasClicked);
        bAtrasRI.RegisterCallback<ClickEvent>(OnBAtrasClicked);
        bAtrasRA.RegisterCallback<ClickEvent>(OnBAtrasClicked);
        bAtrasRL.RegisterCallback<ClickEvent>(OnBAtrasClicked);

        bArma1.RegisterCallback<ClickEvent>(onBArma1Clicked);
        bArma2.RegisterCallback<ClickEvent>(onBArma2Clicked);
        bEmpezar.RegisterCallback<ClickEvent>(onBEmpezarClicked);

        tNombre.RegisterCallback<KeyDownEvent>(OnEndEditingName);
    }
    private void Update()
    {
        if (hovering)
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= timeTillTooltip)
            {
                hoverTimer = 0;
                hovering = false;
                tooltip.style.display = DisplayStyle.Flex;
                float hProp = 1080.0f / Screen.height;
                tooltip.transform.position = new Vector3(tooltip.transform.position.x, tooltip.transform.position.y - 380 * hProp, tooltip.transform.position.z);
            }
        }
    }

    public void RoguelikeSelected()
    {
        UIInterface.SetCurrent(pRoguelikeIni);
    }

    // Update is called once per frame
    private void OnBAtrasClicked(ClickEvent evt)
    {
        UIInterface.GoMainMenu();
    }

    private void onBArma1Clicked(ClickEvent evt)
    {
        eleccion = choices[0];
        ARoguelikeListo(bArma1.text);
    }

    private void onBArma2Clicked(ClickEvent evt)
    {
        eleccion = choices[1];
        ARoguelikeListo(bArma2.text);
    }

    private void onBEmpezarClicked(ClickEvent evt)
    {
        rM.AgregarMiembroALaParty(nombreJugador, eleccion);
        rM.EmpezarCombate();
    }

    private void OnEndEditingName(KeyDownEvent evt)
    {
        if (evt.keyCode == KeyCode.Return)
        {
            nombreJugador = tNombre.text;

            choices[0] = new int[3];
            choices[0][0] = Random.Range(0, 6);
            choices[0][1] = Random.Range(0, 5);
            choices[0][2] = Random.Range(0, 2);

            choices[1] = new int[3];
            choices[1][0] = Random.Range(0, 5);
            if (choices[1][0] >= choices[0][0]) choices[1][0]++;
            choices[1][1] = Random.Range(0, 4);
            if (choices[1][1] >= choices[0][1]) choices[1][1]++;
            choices[1][2] = Random.Range(0, 2);

            bArma1.text = cArma.GetString(choices[0][0]) + " - " + cArma.GetHabilidadString(choices[0][2]) + " - " + cPersonaje.GetAtritbutoString(choices[0][1]);
            bArma2.text = cArma.GetString(choices[1][0]) + " - " + cArma.GetHabilidadString(choices[1][2]) + " - " + cPersonaje.GetAtritbutoString(choices[1][1]);

            StartTooltip(choices[0],bArma1);
            StartTooltip(choices[1],bArma2);

            UIInterface.SetCurrent(pRoguelikeArma);
        }
    }

    private void ARoguelikeListo(string armaNombre)
    {
        lListo.text = "Esta todo listo " + nombreJugador + ", espero que tus elecciones te sirvan en batalla. ¡Tu aventura está por empezar!";
        UIInterface.SetCurrent(pRoguelikeListo);
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

    public void StartTooltip(int[] choices, VisualElement button)
    {
        string text = "";
        switch (choices[0]) // Arma
        {
            case cArma.LIGERAS:
                text = "Arma Ligera: " + cArmasLigeras.Descripcion;
                break;
            case cArma.MEDIAS:
                text = "Arma Media: " + cArmasMedias.Descripcion;
                break;
            case cArma.PELEA:
                text = "Arma de Pelea: " + cArmasPelea.Descripcion;
                break; 
            case cArma.PESADAS:
                text = "Arma Pesada: " + cArmasPesadas.Descripcion;
                break;
            case cArma.ARCO:
                text = "Arma Arco: " + cArmasArco.Descripcion;
                break;
            case cArma.FUEGO:
                text = "Arma de Fuego: " + cArmasFuego.Descripcion;
                break;
            default:
                break;
        }

        switch (choices[1]) // Atributo
        {
            case 0:
                text += " - Maña: sirve para atacar.";
                break;
            case 1:
                text += " - Músculo: sirve para hacer daño, algunas armas lo utilizan mas que otras.";
                break;
            case 2:
                text += " - Ingeno: sirve para defenderse.";
                break;
            case 3:
                text += " - Brío: sirve para resistir heridas.";
                break;
            case 4:
                text += " - Donaire: sirve para ir primero y estar listo par actuar.";
                break;
            default:
                break;
        }

        switch (choices[2]) // Habs
        {
            case 0:
                text += " - AB: Hay que atacar para ganar, puede mejorarse con Defensas Básicas exitosas.";
                break;
            case 1:
                text += " - DB: Defender cura daño y si sale mal podés no perder la acción. Éxito en Defensa Básica mejora Ataque Básico.";
                break;
        }
        button.tooltip = text;
        Debug.Log("tooltip");
    }
}
