using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionIraDivina : cAccionDeImpacto
{
    public const int AID_DETERMINANDO_DADOS = 0;
    public const int AID_TIRANDO = 1;
    public const int AID_CONSECUENCIAS = 2;

    public int dadosATirar;
    public int dificultad;
    public int resultado;
    public bool exito;

    // Start is called before the first frame update
  void Start()
    {
             consecuencia = "Hasta el final de la ronda, los Ataques del objetivo no generan tiradas de Heridas, y los 9 tambien explotan en tiradas de daño o se derriba un matón adicional contra el.";
    reglas = "Ira Divina: Especial con Brio contra Brio. ";
    reglas += consecuencia;
        GetObjets();
        reroleandoState = AID_DETERMINANDO_DADOS;
        nombre = "Ira Divina";
        categoria = cAcciones.AC_CAT_MARCIAL;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonATMarcial1");
        boton.tooltip = reglas;
        icon = c.GetComponent<cIconos>().IraDivina;
    }

    override public void Ejecutar()
    {
        switch (acc_state)
        {
            case AID_DETERMINANDO_DADOS:
                uiC.acc = this;
                DeterminadoDados();
                break;
            case AID_TIRANDO:
                Tirando();
                break;
            case AID_CONSECUENCIAS:
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
        int numeroDeDados = 3 + personaje.atr.brio + personaje.tradicionMarcial[4] + (personaje.arma as cLaVoluntadDelCreador).DadosDeSedDeSangre - personaje.impuesto; 
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
            c.personajeObjetivo.tieneIraDivina = true;
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
        acc_state = AID_DETERMINANDO_DADOS - 1;
    }

    override public void RevisarLegalidad()
    {
        esLegal = c.HayEnemigosEnMelee(personaje);
    }
}