using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


//NADA DE ESTO HACE NADA


/// <summary>
/// //////////////////////////////////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// </summary>
/// 

public class UIPausa : MonoBehaviour
{
    private Button bMainMenu;
    private Button bCerrar;
    private Slider volume;

    public cCombate c;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        bMainMenu = root.Q<Button>("PausaMainMenu");
        bCerrar = root.Q<Button>("PausaCerrar");
        volume = root.Q<Slider>("VolumeSlider");

        bMainMenu.RegisterCallback<ClickEvent>(OnBMainMenuClicked);
        bCerrar.RegisterCallback<ClickEvent>(OnBCerrarClicked);

        Debug.Log("A");
        volume.RegisterValueChangedCallback(v =>
        {
            Debug.Log("B");
            c.music.volume = v.newValue;
        });
    }

    private void OnBMainMenuClicked(ClickEvent evt)
    {
        Debug.Log("C");
        UIInterface.GoMainMenu();
    }

    private void OnBCerrarClicked(ClickEvent evt)
    {
        Application.Quit();
    }

}
