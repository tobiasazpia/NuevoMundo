using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIRoguelikeUpgrade : MonoBehaviour
{
    private VisualElement pRoguelikeUListo;
    private VisualElement pRoguelikeUAnticipacion;

    Label revisarText;
    VisualElement[] party = new VisualElement[3];
    Button continuar;

    Label anticiparText;
    Button siguienteNivel;

    Button[] upgrades = new Button[3];

    VisualElement[] personajes = new VisualElement[3];


    public cRoguelikeUpgrade rU;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i] = root.Q<Button>("ButtonUpgrade" + (i + 1));
        }

        upgrades[0].RegisterCallback<ClickEvent>(OnUpgrade1Clicked);
        upgrades[1].RegisterCallback<ClickEvent>(OnUpgrade2Clicked);
        upgrades[2].RegisterCallback<ClickEvent>(OnUpgrade3Clicked);

        for (int i = 0; i < personajes.Length; i++)
        {
            personajes[i] = root.Q<VisualElement>("RoguelikePersonaje" + (i + 1));
        }


        pRoguelikeUListo = root.Q<VisualElement>("PantallaRoguelikeUpgradeRevisar");
        revisarText = root.Q<Label>("mRoguelikeRevisar");

        for (int i = 0; i < party.Length; i++)
        {
            party[i] = root.Q<Button>("RoguelikePersonaje" + (i + 1));
        }

        pRoguelikeUAnticipacion = root.Q<VisualElement>("PantallaRoguelikeUpgradeAnticipar");
        anticiparText = root.Q<Label>("mRoguelikeAnticipar");
        siguienteNivel = root.Q<Button>("ButtonRoguelikeSiguienteNivel");
        siguienteNivel.RegisterCallback<ClickEvent>(OnSiguienteNivelClicked);

        continuar = root.Q<Button>("ButtonRevisarContinuar");
        continuar.RegisterCallback<ClickEvent>(OnContinuarClicked);
    }

    void OnUpgrade1Clicked(ClickEvent evt)
    {
        rU.GetEleccion(0);
        UIInterface.SetCurrent(pRoguelikeUListo);
    }

    void OnUpgrade2Clicked(ClickEvent evt)
    {
        rU.GetEleccion(1);
        UIInterface.SetCurrent(pRoguelikeUListo);
    }

    void OnUpgrade3Clicked(ClickEvent evt)
    {
        rU.GetEleccion(2);
        UIInterface.SetCurrent(pRoguelikeUListo);
    }

    public void SetUpgradeText(int boton, string text)
    {
        upgrades[boton].text = text;
    }

    private void FillPlayer(cPersonajeFlyweight p, VisualElement vE)
    {
        //Buscar elementos UI
        Debug.Log("Tratando de asignar " + vE.name + ", hijos " + vE.childCount);
        VisualElement encabezado = vE.ElementAt(0);
        VisualElement habilidades = vE.ElementAt(1);
        VisualElement atributos = vE.ElementAt(2);

        Label nombre = encabezado.ElementAt(0) as Label;
        Label tipo = encabezado.ElementAt(1) as Label;
        Label arma = encabezado.ElementAt(2) as Label;

        int numeroDeHabilidades = 2;
        Label[] habValores = new Label[numeroDeHabilidades];
        for (int i = 0; i < numeroDeHabilidades; i++)
        {
            habValores[i] = habilidades.ElementAt(i).ElementAt(1) as Label;
        }

        int numeroDeAtributos = 5;
        Label[] atrValores = new Label[numeroDeAtributos];
        for (int i = 0; i < numeroDeAtributos; i++)
        {
            atrValores[i] = atributos.ElementAt(i).ElementAt(1) as Label;
        }

        //Asignar valores
        nombre.text = p.nombre;
        if (p.esMaton) tipo.text = "(matones)";
        else tipo.text = "(heroe)";
        arma.text = cArma.GetString(p.arma);

        habValores[0].text = p.hab.ataqueBasico.ToString();
        habValores[1].text = p.hab.defensaBasica.ToString();

        atrValores[0].text = p.atr.maña.ToString();
        atrValores[1].text = p.atr.musculo.ToString();
        atrValores[2].text = p.atr.ingenio.ToString();
        atrValores[3].text = p.atr.brio.ToString();
        atrValores[4].text = p.atr.donaire.ToString();
    }

    private void NoPlayer(VisualElement vE)
    {
        VisualElement encabezado = vE.ElementAt(0);
        VisualElement habilidades = vE.ElementAt(1);
        VisualElement atributos = vE.ElementAt(2);

        Label nombre = encabezado.ElementAt(0) as Label;
        Label tipo = encabezado.ElementAt(1) as Label;
        Label arma = encabezado.ElementAt(2) as Label;

        //Asignar valores
        nombre.text = " - ";
        tipo.text = " - ";
        arma.text = " - ";
    }

    public void RevisarParty()
    {
        int i = 0;
        for (; i < rU.rM.party.Count; i++)
        {
            FillPlayer(rU.rM.party[i], personajes[i]);
        }
        for (; i < 3; i++)
        {
            NoPlayer(personajes[i]);
        }
    }

    public void OnContinuarClicked(ClickEvent evt)
    {
        anticiparText.text = ("Toma un respiro y preparate, empieza tu pelea numero " + ++rU.rM.nivel);
        UIInterface.SetCurrent(pRoguelikeUAnticipacion);
    }

    public void OnSiguienteNivelClicked(ClickEvent evt)
    {
        rU.rM.EmpezarCombate();
    }
}
