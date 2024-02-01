using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIPausa : MonoBehaviour
{
    private Button bContinuar;
    private Button bMainMenu;
    private Button bCerrar;

    private VisualElement pPausa;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        bContinuar = root.Q<Button>("PausaContinuar");
        bMainMenu = root.Q<Button>("PausaMainMenu");
        bCerrar = root.Q<Button>("PausaCerrar");

        bContinuar.RegisterCallback<ClickEvent>(OnBContinuarClicked);
        bMainMenu.RegisterCallback<ClickEvent>(OnBMainMenuClicked);
        bCerrar.RegisterCallback<ClickEvent>(OnBCerrarClicked);

        pPausa = root.Q<VisualElement>("PantallaInicial");
    }

    public void PausaSelected()
    {
        //Por ahora esconde la ui de combate para mostrar esto, no se si va a quedar bien
        UIInterface.SetCurrent(pPausa);
    }

    private void OnBContinuarClicked(ClickEvent evt)
    {
       //To Do: Reanudar Combate - probablemente una funcion en combate
    }

    private void OnBMainMenuClicked(ClickEvent evt)
    {
        UIInterface.GoMainMenu();
    }

    private void OnBCerrarClicked(ClickEvent evt)
    {
        Application.Quit();
    }

}
