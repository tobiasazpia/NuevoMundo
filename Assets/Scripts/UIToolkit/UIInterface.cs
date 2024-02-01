using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIInterface : MonoBehaviour
{
    static private VisualElement pCurrent;

    static private VisualElement pPrincipal;

    static private VisualElement pRoguelike;
    static private VisualElement pEscarmuza;

    static private VisualElement pUpgrade;

    public cRoguelikeUpgrade rU;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        pPrincipal = root.Q<VisualElement>("PantallaInicial");

        pEscarmuza = root.Q<VisualElement>("PantallaEscarmuza");
        pRoguelike = root.Q<VisualElement>("PantallaRoguelikeIni");

        pUpgrade = root.Q<VisualElement>("PantallaRoguelikeUpgradeElegir");
    }

    static public void SetCurrent(VisualElement obj)
    {
        if (pCurrent != null)
        {
            pCurrent.style.display = DisplayStyle.None;
        }
        pCurrent = obj;
        pCurrent.style.display = DisplayStyle.Flex;
    }

    static public void GoMainMenu()
    {
        SetCurrent(pPrincipal);
    }

    static public void GoEscarmuza()
    {
        SetCurrent(pEscarmuza);
    }

    static public void GoRoguelike()
    {
        SetCurrent(pRoguelike);
    }

    static public void GoUpgrade()
    {
        SetCurrent(pUpgrade);
    }

    static public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
