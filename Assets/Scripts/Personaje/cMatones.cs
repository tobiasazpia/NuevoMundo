using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMatones : cPersonaje
{
    public int cantidadInicial;

    private int m_Cantidad;
    public int Cantidad
    {
        get { return m_Cantidad; }
        set
        {
            if (m_Cantidad == value) return;
            m_Cantidad = value;
            if (OnCantidadChange != null)
                OnCantidadChange(m_Cantidad);
        }
    }
    public delegate void OnCantidadChangeDelegate(int newVal);
    public event OnCantidadChangeDelegate OnCantidadChange;

    public int modGuardiaDeMaton;

    public override int GetDadosBaseIniciativa()
    {
        return Cantidad;
    }

    public override int[] RollDadosdeAccion(int numeroDeDados)
    {
        return cDieMath.tomarXMenores(cDieMath.TirarDados(numeroDeDados), Cantidad);
    }

    public override void RecibirGolpe(cPersonaje atacante, int atq, int def)
    {
        CaenMatones(atq, def, atacante.GetExtraParaMatones());
    }

    public void CaenMatones(int atq,int def, int extraNecesario)
    {
        int muertos = 1;
        int dif = atq - Mathf.Max(GetGuardia(),def);
        muertos += dif / extraNecesario;
        string text = "Necesitaba pasarse por " + UIInterface.IntEnNegrita(extraNecesario) + " y se paso por ";
        uiC.perCambio = nombre;
        int cantPrevia = Cantidad;
        Cantidad -= muertos;
        Cantidad = Mathf.Max(Cantidad, 0);
        if (muertos > 1)
        {
           text += UIInterface.IntExitoso(dif) + ", asi que se lleva a multiples enemigos! Caen " + UIInterface.IntEnNegrita(Mathf.Min(muertos,cantPrevia)) + " matones";
        }
        else
        {
            text += UIInterface.IntFallido(dif) + ", asi que se lleva a un maton a la tumba";
        }
        text += " dejando " + (c.personajeObjetivo as cMatones).Cantidad+ " en pie.";
        uiC.SetText(text);
    }

    public void CapazNoHayMasMatones()
    {
        if (Cantidad <= 0)
        {
            //HAY QUE PONER UN MENSAJE
            uiC.SetText(nombre + " ya no tiene matones en pie! Quedan fuera del combate");
            c.RemoverPersonaje(this);
            uiC.ActualizarIniciativa(c.personajes);
            if (!c.MasDeUnEquipoEnPie()) c.stateID = cCombate.TERMINANDO_COMBATE;
        }      
    }

    public override void ResetHP()
    {
        vivo = true;
        uiC.perCambio = nombre;
        Cantidad = cantidadInicial;
    }
    public override void CalcularGuardia()
    {
        guardia = 15 + hab.ataqueBasico + hab.defensaBasica + arma.GetGuardiaMod() + modGuardiaDeMaton;
        if (guardia > 30)
        {
            guardia = 30;
        }
    }
}
