using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cImponer : cAccionDeImpacto
{
    public const int AIMP_DETERMINANDO_DADOS = 0;
    public const int AIMP_TIRANDO = 1;
    public const int AIMP_CONSECUENCIAS = 2;

    public int dadosATirar;
    public int dificultad;
    public int resultado;
    public bool exito;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        SetUp();
    }

    override public void SetUp()
    {
            reglas = "Imponer: Especial con Músculo contra Brío. ";
            consecuencia = "Hasta el final de la ronda, el objetivo tira -1d en sus tiradas de combate y Daño por cada punto en Imponer.";
    reglas += consecuencia;
        Debug.Log("imponer set up");
        GetObjets();
        reroleandoState = AIMP_DETERMINANDO_DADOS;
        nombre = "Imponer";
        categoria = cAcciones.AC_CAT_MARCIAL;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonATMarcial1");
        boton.tooltip = reglas;
        icon = c.GetComponent<cIconos>().Imponer;
    }

    override public void Ejecutar()
    {
        switch (acc_state)
        {
            case AIMP_DETERMINANDO_DADOS:
                uiC.acc = this;
                DeterminadoDados();
                break;
            case AIMP_TIRANDO:
                Tirando();
                break;
            case AIMP_CONSECUENCIAS:
                Consecuencias();
                break;
            default:
                break;
        }
        acc_state++;
        c.EsperandoOkOn(true);
    }

    protected void DeterminadoDados()
    {
        dadosATirar = DeterminarNumeroDeDados();
        dificultad = 18 + 3 * c.personajeObjetivo.atr.brio;
        uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " usa su " + nombre + ", tirando " + dadosATirar + " dados contra el Brío de " + UIInterface.NombreDePersonajeEnNegrita(c.personajeObjetivo) + " y su dificultad de " + UIInterface.IntEnNegrita(dificultad) + ".");
    }

    override public int DeterminarNumeroDeDados()
    {
        int numeroDeDados = 3 + personaje.atr.musculo + personaje.tradicionMarcial[2] - personaje.impuesto;
        if (personaje.arma.maestria > 4) numeroDeDados += (personaje.arma as cJaieiy).DadosMaestro;
        if (reroleando)
        {
            Debug.Log("reroll true, sumamos");
            numeroDeDados += dadosExtrasParaReroll;
        }
        else Debug.Log("reroll false, no sumamos");
        return numeroDeDados;
    }

    virtual protected void Tirando()
    {
        tirada tr = cDieMath.TirarDados(dadosATirar);
        resultado = cDieMath.sumaDe3Mayores(tr);
        string numText;
        string resText;

        exito = resultado >= dificultad;
        if (exito)
        {
            resText = "acertando";
            numText = UIInterface.IntExitoso(resultado);
            c.personajeObjetivo.impuesto = personaje.tradicionMarcial[3];
        }
        else
        {
            resText = "fallando";
            numText = UIInterface.IntFallido(resultado);
        }
        uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " saca " + numText + ", " + resText + " su acción.");
        if (personaje.Drama && !exito) uiC.PedirDrama();
    }

    public virtual void Consecuencias()
    {
        string text = "";
        if (exito)
        {
            uiC.perCambio = personaje.nombre;
            text = ("Tuvo éxito, " + consecuencia);
        }
        else text = ("Falla y continúa el combate.");

        personaje.GastarDado(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
        c.stateID = cCombate.BUSCANDO_ACCION;
        cEventManager.StartPersonajeActuoEvent(c.personajeActivo);
        c.personajeActivo.GetAccionPorNumero(c.accionActiva).ResetState();
        c.accionActiva = -1;
        uiC.ActualizarIniciativa(c.personajes);
        acc_state = AIMP_DETERMINANDO_DADOS - 1;
    }

    override public void RevisarLegalidad()
    {
        Debug.Log(c);
        esLegal = c.HayEnemigosEnMelee(personaje);
    }
}
