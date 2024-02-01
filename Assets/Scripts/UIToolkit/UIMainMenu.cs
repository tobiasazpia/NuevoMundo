    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIMainMenu : MonoBehaviour
{
    public UIEscarmuza escUI;
    public UIRoguelike rogUI;

    public cRoguelikeManager rogMan;
    public EscarmuzaManager escMan;

    private Button bEscarmuza;
    private Button bRoguelike;
    private Button bCerrar;

    private VisualElement pPrincipal;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        bEscarmuza = root.Q<Button>("PrincipalEscarmuza");
        bRoguelike = root.Q<Button>("PrincipalRoguelike");
        bCerrar = root.Q<Button>("PrincipalCerrar");

        bEscarmuza.RegisterCallback<ClickEvent>(OnBEscarmuzaClicked);
        bRoguelike.RegisterCallback<ClickEvent>(OnBRoguelikeClicked);
        bCerrar.RegisterCallback<ClickEvent>(OnBCerrarClicked);

        pPrincipal = root.Q<VisualElement>("PantallaInicial");

        UIInterface.SetCurrent(pPrincipal);
    }

    public void PrincipalSelected()
    {
        UIInterface.SetCurrent(pPrincipal);
    }

    private void OnBEscarmuzaClicked(ClickEvent evt)
    {
        escUI.EscarmuzaSelected();
    }

    private void OnBRoguelikeClicked(ClickEvent evt)
    {
        rogUI.RoguelikeSelected();
    }

    private void OnBCerrarClicked(ClickEvent evt)
    {
        UIInterface.QuitGame();
    }
   
}