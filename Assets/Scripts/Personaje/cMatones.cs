using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMatones : cPersonaje
{
    public int cantidadInicial;
    public int cantidad;
    public int modGuardiaDeMaton;

    public override int GetDadosBaseIniciativa()
    {
        return cantidad;
    }

    public override int[] RollDadosdeAccion(int numeroDeDados)
    {
        return cDieMath.tomarXMenores(cDieMath.TirarDados(numeroDeDados), cantidad);
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
        string text = "Necesitaba pasarse por " + extraNecesario + " y se paso por " + dif + ", asi que ";

        cantidad -= muertos;
        cantidad = Mathf.Max(cantidad, 0);
        if (muertos > 1)
        {
           text += "se lleva a multiples enemigos! Caen " + muertos + " matones";
        }
        else
        {
            text += "se lleva a un maton a la tumba";
        }
        text += " dejando " + (c.personajeObjetivo as cMatones).cantidad+ " en pie.";
        uiC.SetText(text);
    }

    public void CapazNoHayMasMatones()
    {
        if (cantidad <= 0)
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
        cantidad = cantidadInicial;
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
