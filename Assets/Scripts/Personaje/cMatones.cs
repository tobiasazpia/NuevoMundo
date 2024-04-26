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
        if (c.personajeActivo.arma is cJaieiy)
        {
            extraNecesario -= (c.personajeActivo.arma as cJaieiy).DadosNervios;
            (c.personajeActivo.arma as cJaieiy).ResetNervios();
        }
        int muertos = 1;
        int dif = atq - Mathf.Max(GetGuardia(),def);
        muertos += dif / extraNecesario;
        Debug.Log("musmult: " + c.personajeActivo.arma.musMult +", base: "+ c.personajeActivo.arma.basePara2doMaton);
        string text = "Necesitaba pasarse por " + UIInterface.IntEnNegrita(extraNecesario) + " y se paso por ";
        uiC.perCambio = nombre;
        int cantPrevia = Cantidad;
        Cantidad -= muertos;
        Cantidad = Mathf.Max(Cantidad, 0);
        if (dif >= extraNecesario)
        {
           text += UIInterface.IntExitoso(dif) + ", asi que se lleva a multiples enemigos! Caen " + UIInterface.IntEnNegrita(Mathf.Min(muertos,cantPrevia)) + " matones";
            if(Cantidad == 0) c.effect.clip = c.effectMuerte;
            else c.effect.clip = c.effectHeridaHard;
        }
        else
        {
            text += UIInterface.IntFallido(dif) + ", asi que se lleva a un maton a la tumba";
            if (Cantidad == 0) c.effect.clip = c.effectMuerte;
            else c.effect.clip = c.effectHeridaSoft;
        }

        if (tieneTerror)
        {
            muertos++;
            text += " y " + UIInterface.IntEnNegrita(1) + " más por Terror de Dios,";
            Cantidad = Mathf.Max(Cantidad-1, 0);
        }
        if (tieneIraDivina)
        {
            muertos++;
            text += " y " + UIInterface.IntEnNegrita(1) + " más por Ira Divina";
            Cantidad = Mathf.Max(Cantidad - 1, 0);
        }
        if (muertos > 1 && (tieneIraDivina || tieneTerror)) {
            foreach (var item in c.personajes)
            {
                // Esto funciona raro si hay 2 personajes con la voluntad del creador del mismo lado, como que las habilidades de los dos trigerean tambien la sed de sangre del otro. esta buneo esto?
                if(item.arma is cLaVoluntadDelCreador && c.personajeActivo.equipo == item.equipo)
                {
                    Debug.Log("sum sed de sangre");
                    c.uiC.perCambio = item.nombre;
                    (item.arma as cLaVoluntadDelCreador).DadosDeSedDeSangre += item.tradicionMarcial[2];
                }
            }    
        }

        if (Cantidad == 0) c.effect.clip = c.effectMuerte;
        else if (muertos > 1) c.effect.clip = c.effectHeridaHard;
        else c.effect.clip = c.effectHeridaSoft;
        c.effect.Play();

        text += " dejando " + (c.personajeObjetivo as cMatones).Cantidad+ " en pie.";
        uiC.SetText(text);
    }

    public void CapazNoHayMasMatones()
    {
        if (Cantidad <= 0)
        {
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
        guardia = 15 + tradicionMarcial[0] + tradicionMarcial[1] + arma.GetGuardiaMod() + modGuardiaDeMaton;
        if (guardia > 30)
        {
            guardia = 30;
        }
    }
}
