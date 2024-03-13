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

    public void fillText(int nivel)
    {
        texto.text = "Esta aventura llego a su fin, llegaste hasta el nivel " + nivel;

        int i = 0;
        for (; i < rM.party.Count; i++)
        {
            UIInterface.FillPlayer(rM.party[i], personajes[i],true);
        }
        for (; i < personajes.Length; i++)
        {
            UIInterface.NoPlayer(personajes[i]);
        }
    }

    void OnReintentarClicked(ClickEvent evt)
    {
        UIInterface.GoRoguelike();
    }
}
