using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cJaieiy : cArmasPesadas
{
    /*
    Listo - Principiante: Otros tienen -1d al Atacarte por cada dado de acci�n te quede esta ronda.
    Listo - Veterano: +1d en tiradas de Heridas por cada Fase desde la Fase de tu Dado de Acci�n menor.
    Listo - Maestro: Tiras un dado adicional en todas tus tiradas de Combate en la fase 6, 2 dados adicionales en la 7, 3 en la 8, 4 en la 9 y 5 en la 10.     // 1 int: DadosMaestro
    Listo - Imponer: Especial con M�sculo contra Br�o. -1d al oponente en sus tiradas de combate y Da�o por el resto de la ronda por cada punto en Imponer.
    Listo? - Nervios de Acero: Defensa. Por cada Fase desde la fase del Dado de Acci�n a usar esta reacci�n, reduc� en 1 tu Extra para Matones durante tu pr�ximo Ataque, o +1d en tu pr�xima tirada de Da�o.
    Listo - Preparaci�n: Pasiva. Al principio de cada ronda, restale a tus Dados de Acci�n hasta tanto como puntos en Preparaci�n, empezando por el menor y pasando al pr�ximo cuando llegue a 1.*/

    private int m_DadosMaestro;
    public int DadosMaestro
    {
        get { return m_DadosMaestro; }
        set
        {
            if (m_DadosMaestro == value) return;
            m_DadosMaestro = value;
            if (OnDadosMaestroChange != null)
                OnDadosMaestroChange(m_DadosMaestro);
        }
    }
    public delegate void OnDadosMaestroChangeDelegate(int newVal);
    public event OnDadosMaestroChangeDelegate OnDadosMaestroChange;

    private int m_DadosNervios;
    public int DadosNervios
    {
        get { return m_DadosNervios; }
        set
        {
            if (m_DadosNervios == value) return;
            m_DadosNervios = value;
            if (OnDadosNerviosChange != null)
                OnDadosNerviosChange(m_DadosNervios);
        }
    }
    public delegate void OnDadosNerviosChangeDelegate(int newVal);
    public event OnDadosNerviosChangeDelegate OnDadosNerviosChange;

    // Start is called before the first frame update
    void Start()
    {
        habilidadesNombres[0] = "Imponer";
        habilidadesNombres[1] = "Nervios de Acero";
        habilidadesNombres[2] = "Preparaci�n";
        habilidadesNombres[3] = "Investigar";

        SetUpHabilidades();
        maestria = CalcularMaestria(habilidades);

        p.tieneTradicionMarcial = true;
        SetUpNombre();

        SetUpNumeros();

        p.CalcularExtraParaMatones();
        p.CalcularGuardia();

        SetUpAccionables();
    }

    static public string GetNombreDeHabilidad(int index)
    {
        switch (index)
        {
            case 0:
                return "Ataque B�sico";
            case 1:
                return "Defensa B�sica";
            case 2:
                return "Imponer";
            case 3:
                return "N. de Acero";
            case 4:
                return "Preparaci�n";
            case 5:
                return "Investigar";
            default:
                return "Error, index no era habilidad";
        }
    }

    static public string GetDescripcionDeHabilidad(int index)
    {
        switch (index)
        {
            case 0:
                return "Ataque B�sico Desc";
            case 1:
                return "Defensa B�sica Desc";
            case 2:
                return "Imponer: Especial con M�sculo contra Br�o. -1d al oponente en sus tiradas de combate y Da�o por el resto de la ronda por cada punto en Imponer.";
            case 3:
                return "Nervios de Acero: Defensa. Por cada Fase desde la fase del Dado de Acci�n a usar esta reacci�n, reduc� en 1 tu Extra para Matones durante tu pr�ximo Ataque, o +1d en tu pr�xima tirada de Da�o.";
            case 4:
                return "Preparaci�n: Pasiva. Al principio de cada ronda, restale a tus Dados de Acci�n hasta tanto como puntos en Preparaci�n, empezando por el menor y pasando al pr�ximo cuando llegue a 1.";
            case 5:
                return "Habilidad Marcial Civil; al repetir una tirada, tira tantos dados adicionales como puntos tenga " + GetNombreDeHabilidad(index) + ".";
            default:
                return "error descripcion habilidad";
        }
    }

    protected override void SetUpNombre()
    {
        nombre = "Jaieiy";
    }

    protected override void SetUpAccionables()
    {

        acciones.Add(gameObject.AddComponent<cAccionMovimientoAgresivo>());
        acciones.Add(gameObject.AddComponent<cAccionAtaqueBasicoJaieiy>());
        acciones.Add(gameObject.AddComponent<cImponer>());
        reacciones.Add(gameObject.AddComponent<cReaccionDefensaBasicaJaieiy>());
        reacciones.Add(gameObject.AddComponent<cReaccionNerviosDeAcero>());
        AgregarAccionablesAlPersonaje();
    }

    public int[] AplicarPreparacion(int[] dados)
    {
        int aRestar = habilidades[4];
        bool restamos;
        while (aRestar > 0)
        {
            restamos = false;
            for (int i = 0; i < dados.Length; i++)
            {
                if (dados[i] > 1) {
                    dados[i]--;
                    aRestar--;
                    restamos = true;
                    break;
                }
            }
            if (!restamos) break;
        }
        return dados;
    }

    public void ResetNervios()
    {
        DadosNervios = 0;
    }

    new static public void FillInfo(VisualElement vE)
    {
        VisualElement contenedor = vE.ElementAt(0);

        VisualElement encabezado = contenedor.ElementAt(0);
        VisualElement tecnicas = contenedor.ElementAt(1);
        VisualElement habilidades = contenedor.ElementAt(2);

        Label titulo = encabezado.ElementAt(0) as Label;
        Label desc = encabezado.ElementAt(1) as Label;

        titulo.text = "<b>" + GetNombre();
        desc.text = "Armas Pesadas, Yvyra, Investigar - No ser golpeado y Guardar Acciones";

        (tecnicas.ElementAt(1).ElementAt(0) as Label).text = "<b>Principiante:</b> Otros tienen -1d al Atacarte por cada dado de acci�n te quede esta ronda.";
        (tecnicas.ElementAt(1).ElementAt(1) as Label).text = "<b>Veterano:</b> +1d en tiradas de Heridas por cada Fase desde la Fase de tu Dado de Acci�n menor";
        (tecnicas.ElementAt(1).ElementAt(2) as Label).text = "<b>Maestro:</b> +1d en todas tus tiradas de Combate en la fase 6, 2 dados adicionales en la 7, 3 en la 8, 4 en la 9 y 5 en la 10.";

        (habilidades.ElementAt(1).ElementAt(0) as Label).text = "<b>Imponer:</b> Especial con M�sculo contra Br�o. -1d al oponente en sus tiradas de combate y Da�o por el resto de la ronda por cada punto en Imponer.";
        (habilidades.ElementAt(1).ElementAt(1) as Label).text = "<b>Nervios de Acero:</b> Defensa. Por cada Fase desde la fase del Dado de Acci�n a usar esta reacci�n, reduc� en 1 tu Extra para Matones durante tu pr�ximo Ataque, o +1d en tu pr�xima tirada de Da�o.";
        (habilidades.ElementAt(1).ElementAt(2) as Label).text = "<b>Preparaci�n:</b> Pasiva. Al principio de cada ronda, restale a tus Dados de Acci�n hasta tanto como puntos en Preparaci�n, empezando por el menor y pasando al pr�ximo cuando llegue a 1.";
        /*
Listo - Principiante: Otros tienen -1d al Atacarte por cada dado de acci�n te quede esta ronda.
Listo - Veterano: +1d en tiradas de Heridas por cada Fase desde la Fase de tu Dado de Acci�n menor.
Listo - Maestro: Tiras un dado adicional en todas tus tiradas de Combate en la fase 6, 2 dados adicionales en la 7, 3 en la 8, 4 en la 9 y 5 en la 10.     // 1 int: DadosMaestro
Listo - Imponer: Especial con M�sculo contra Br�o. -1d al oponente en sus tiradas de combate y Da�o por el resto de la ronda por cada punto en Imponer.
Nervios de Acero: Defensa. Por cada Fase desde la fase del Dado de Acci�n a usar esta reacci�n, reduc� en 1 tu Extra para Matones durante tu pr�ximo Ataque, o +1d en tu pr�xima tirada de Da�o.
Listo - Preparaci�n: Pasiva. Al principio de cada ronda, restale a tus Dados de Acci�n hasta tanto como puntos en Preparaci�n, empezando por el menor y pasando al pr�ximo cuando llegue a 1.*/
    }

    static public string GetNombre()
    {
        return "Jaieiy";
    }
}



