using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class UIRoguelike : MonoBehaviour
{
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
    int arma1Value;
    int arma2Value;
    int armaChosenValue;

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

        bEmpezar = root.Q<Button>("Empezar");

        tNombre = root.Q<TextField>("NombreInput");

        lListo = root.Q<Label>("mRoguelikeListo");

        bAtrasE.RegisterCallback<ClickEvent>(OnBAtrasClicked);
        bAtrasRI.RegisterCallback<ClickEvent>(OnBAtrasClicked);
        bAtrasRA.RegisterCallback<ClickEvent>(OnBAtrasClicked);
        bAtrasRL.RegisterCallback<ClickEvent>(OnBAtrasClicked);

        bArma1.RegisterCallback<ClickEvent>(onBArma1Clicked);
        bArma2.RegisterCallback<ClickEvent>(onBArma2Clicked);
        bEmpezar.RegisterCallback<ClickEvent>(onBEmpezarClicked);

        tNombre.RegisterCallback<KeyDownEvent>(OnEndEditingName);
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
        rM.AgregarMiembroALaParty(nombreJugador, eleccion, false);
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

            UIInterface.SetCurrent(pRoguelikeArma);
        }
    }

    private void ARoguelikeListo(string armaNombre)
    {
        lListo.text = "Esta todo listo " + nombreJugador + ", espero que las " + armaNombre + " hayan sido una sabia elección. Tu aventura está por empezar!";
        UIInterface.SetCurrent(pRoguelikeListo);
    }
}
