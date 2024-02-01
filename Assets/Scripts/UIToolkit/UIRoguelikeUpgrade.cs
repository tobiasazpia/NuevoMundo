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

    public void RevisarParty()
    {
        int i = 0;
        for (; i < rU.rM.party.Count; i++)
        {
            UIInterface.FillPlayer(rU.rM.party[i], personajes[i]);
        }
        for (; i < 3; i++)
        {
            UIInterface.NoPlayer(personajes[i]);
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
