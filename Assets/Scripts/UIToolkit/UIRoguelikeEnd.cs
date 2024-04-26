using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIRoguelikeEnd : MonoBehaviour
{
    public cRoguelikeManager rM;

    private Label texto;
    private Button reintentar;
    VisualElement[] personajes = new VisualElement[3];

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        texto = root.Q<Label>("mRoguelikeFinal");
        reintentar = root.Q<Button>("ButtonFinalReintentar");

        reintentar.RegisterCallback<ClickEvent>(OnReintentarClicked);

        for (int i = 0; i < personajes.Length; i++)
        {
            personajes[i] = root.Q<VisualElement>("RoguelikePersonajeFinal" + (i + 1));
        }
    }

    public void fillText(int nivel, int equipoVictorioso)
    {
        texto.text = "Esta aventura llego a su fin, llegaste hasta el nivel " + nivel;

        if(nivel == cRoguelikeCombate.FINAL_COMBAT)
        {
            if (equipoVictorioso == 1)
            {

                texto.text += " ¡Lo lograste! Superaste todos los obstáculos que el Nuevo Mundo tenía para vos."; //El Sol, la Sombra y todos los dioses sabran de tus proezas.";
            }
            else
            {
                texto.text += " ¡Caiste en la última batalla! Habra que prepararse mejor para la próxima.";
            }

        }

        int i = 0;

        Debug.Log(" rM.party.Count" + rM.party.Count);
        Debug.Log("personajes.Length" + personajes.Length);
        for (; i < rM.party.Count; i++)
        {
            Debug.Log("un p");
            personajes[i].parent.style.alignItems = Align.FlexStart;
            personajes[i].style.display = DisplayStyle.Flex;
            personajes[i].style.top = 0;
            personajes[i].style.right = 0;
            UIRoguelikeUpgrade.ArmaTooltip(rM.party[i].arma, personajes[i].ElementAt(2).ElementAt(1).ElementAt(0) as Button, rM.party[i].maestria);
            if (rM.party[i].arma > cArma.FUEGO)
            {
                personajes[i].ElementAt(2).ElementAt(1).ElementAt(0).AddToClassList("button");
                personajes[i].ElementAt(2).ElementAt(1).ElementAt(0).AddToClassList("armaButton");
                personajes[i].ElementAt(2).ElementAt(1).ElementAt(0).RegisterCallback<ClickEvent>(rM.rC.combate.uiC.OnTradicionMarcialClicked);
            }
            else
            {
                personajes[i].ElementAt(2).ElementAt(1).ElementAt(0).RemoveFromClassList("button");
                personajes[i].ElementAt(2).ElementAt(1).ElementAt(0).RemoveFromClassList("armaButton");
                personajes[i].ElementAt(2).ElementAt(1).ElementAt(0).UnregisterCallback<ClickEvent>(rM.rC.combate.uiC.OnTradicionMarcialClicked);
            }
           // UIInterface.FillPlayer(rM.party[i], personajes[i],true);
            UIInterface.FillPlayerNueva(rM.party[i], personajes[i],false);

            //for (int j = 0; j < 3; j++)
            //{
            //    rM.rC.combate.rU.uiRU.RegisterTooltip(personajes[i].ElementAt(1).ElementAt(j));
            //}
            //for (int j = 0; j < 5; j++)
            //{
            //    rM.rC.combate.rU.uiRU.RegisterTooltip(personajes[i].ElementAt(1).ElementAt(j));
            //}
            //for (int j = 0; j < 6; j++)
            //{
            //    rM.rC.combate.rU.uiRU.RegisterTooltip(personajes[i].ElementAt(2).ElementAt(1).ElementAt(j));
            //}
            //rM.rC.combate.rU.uiRU.RegisterTooltip(personajes[i].ElementAt(2).ElementAt(0));

        }
        for (; i < personajes.Length; i++)
        {
            //UIInterface.NoPlayer(personajes[i],true);
            //UIInterface.FillPlayerNueva(personajes[i]);
            personajes[i].style.display = DisplayStyle.None;
        }
     
    }

    void OnReintentarClicked(ClickEvent evt)
    {
        UIInterface.GoRoguelike();
    }
}
