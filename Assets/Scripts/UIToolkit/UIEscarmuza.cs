using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class UIEscarmuza : MonoBehaviour
{
    public EscarmuzaManager eM;

    private VisualElement pEscarmuza;

    private Button bAtrasE;
    private Button bJugar;
    private Button bSim;
    private Button bPreparar;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        pEscarmuza = root.Q<VisualElement>("PantallaEscarmuza");

        bAtrasE = root.Q<Button>("AtrasEscarmuza");
        bJugar = root.Q<Button>("Jugar");
        bSim = root.Q<Button>("Simular");
        bPreparar = root.Q<Button>("Preparar");

        bAtrasE.RegisterCallback<ClickEvent>(OnBAtrasClicked);
        bJugar.RegisterCallback<ClickEvent>(OnJugarClicked);
        bSim.RegisterCallback<ClickEvent>(OnSimularClicked);
        bPreparar.RegisterCallback<ClickEvent>(OnprepararClicked);
    }

    public void EscarmuzaSelected()
    {
        Debug.Log("escarmuza");
        UIInterface.SetCurrent(pEscarmuza);
    }

    private void OnBAtrasClicked(ClickEvent evt)
    {
        UIInterface.GoMainMenu();
    }

    public void OnJugarClicked(ClickEvent evt)
    {
        eM.IniciarCombate();
    }

    public void OnSimularClicked(ClickEvent evt)
    {

    }

    public void OnprepararClicked(ClickEvent evt)
    {

    }
}
