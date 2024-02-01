

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct sScore
{
    public float combatesJugados;
    public float wins;
    public float loses;
    public float winrate;

    public float rondas;
    public float firstFirstCount;
    public float firstEqualCount;
    public float secondFirstCount;
    public float secondEqualCount;
    public float thirdFirstCount;
    public float thirdEqualCount;
    public float allFirst;
    public float allFirstOrEqual;
    public float twoFirst;
    public float twoFirstOrEqual;
    public float oneFirst;
    public float oneFirstOrEqual;

    public float allRate;
    public float twoRate;
    public float oneRate;
    public float zeroRate;
    public float atLeastOneRate;
    public float atLeastTwoRate;

    public float allEqualRate;
    public float twoEqualRate;
    public float oneEqualRate;
    public float zeroEqualRate;
    public float atLeastOneEqualRate;
    public float atLeastTwoEqualRate;

    public float allEqualOrLess3;
    public float allEqualOrLess5;
    public float allEqualOrLess7;
}

public class cScoreCard : MonoBehaviour
{
    public List<cPersonaje> personajes = new List<cPersonaje>();
    public Dictionary<cPersonaje,sScore> scores  = new Dictionary<cPersonaje,sScore>();
    public cCombate combate;
    public bool set = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetButtonDown("ShowScorecardWinrate") && set){ 
            ShowScorecardWinrate();
        }
        if (Input.GetButtonDown("ShowScorecardIniciativa") && set){ 
            ShowScorecardIniciativa();
        }*/
    }

    public void SetScorecard()
    {
        combate = GetComponentInParent<cCombate>();
        if(combate != null){
            personajes = combate.personajes;
            scores.Clear();
            sScore[] empty = new sScore[personajes.Count];
            for (int i = 0; i < personajes.Count; i++){
                empty[i] = new sScore();
                scores.Add(personajes[i],empty[i]);
            }
            set = true;
        }
    }

    void ShowScorecardWinrate()
    {
        Debug.Log("Showing Scorecard Winrate");
        for (int i = 0; i < personajes.Count; i++){
            sScore sc = scores[personajes[i]];
            //Calculando Winrate
            if(sc.wins != 0){ 
                sc.winrate = Mathf.Round(sc.wins/sc.combatesJugados*100)/100;
            }else{
                sc.winrate = 0;
            }
            //Output
            scores[personajes[i]] = sc;
            Debug.Log(personajes[i].nombre + " - Combates: " + scores[personajes[i]].combatesJugados + ", winrate: " + scores[personajes[i]].winrate*100 + "%"); 
       }
    }

    void ShowScorecardIniciativa()
    {
        Debug.Log("Showing Scorecard Iniciativa");
        for (int i = 0; i < personajes.Count; i++){
            sScore sc = scores[personajes[i]];
            //Calculando Varibles de Iniciativa
            sc.allRate = Mathf.Round(sc.allFirst / sc.rondas*10000)/100;
            sc.twoRate = Mathf.Round(sc.twoFirst / sc.rondas*10000)/100;
            sc.oneRate = Mathf.Round(sc.oneFirst / sc.rondas*10000)/100;
            sc.zeroRate = 100-Mathf.Round((sc.allRate+sc.twoRate+sc.oneRate)*100)/100;
            sc.atLeastOneRate = 100-sc.zeroRate;
            sc.atLeastTwoRate = sc.allRate+sc.twoRate;
            //Calculando Varibles incluyendo empates como wins de Iniciativa
            sc.allEqualRate = Mathf.Round(sc.allFirstOrEqual / sc.rondas*10000)/100;
            sc.twoEqualRate = Mathf.Round(sc.twoFirstOrEqual / sc.rondas*10000)/100;
            sc.oneEqualRate = Mathf.Round(sc.oneFirstOrEqual / sc.rondas*10000)/100;
            sc.zeroEqualRate = 100-Mathf.Round((sc.allEqualRate+sc.twoEqualRate+sc.oneEqualRate)*100)/100;
            sc.atLeastOneEqualRate = 100-sc.zeroEqualRate;
            sc.atLeastTwoEqualRate = sc.allEqualRate+sc.twoEqualRate;
            //All Less Or Equal Than
            float allEqualOrLess3Rate = Mathf.Round(sc.allEqualOrLess3 / sc.rondas * 10000) / 100;
            float allEqualOrLess5Rate = Mathf.Round(sc.allEqualOrLess5 / sc.rondas * 10000) / 100;
            float allEqualOrLess7Rate = Mathf.Round(sc.allEqualOrLess7 / sc.rondas * 10000) / 100;
            float atLeastOne8OrGreaterRate = 100 - allEqualOrLess7Rate;
            //Output
            scores[personajes[i]] = sc;
            Debug.Log(personajes[i].nombre + " - Rondas Jugadas: " + scores[personajes[i]].rondas);
            Debug.Log("Todas menores que 4: " + allEqualOrLess3Rate + "%, Todas menores que 6:  " + allEqualOrLess5Rate + "%, Todas menores que 8: " + allEqualOrLess7Rate + "%, Todas mayores que 7: " + atLeastOne8OrGreaterRate + "%");
            //Debug.Log("Primero en todas: " + scores[personajes[i]].allRate + "%, Primero en 2: " + scores[personajes[i]].atLeastTwoRate + "%, Primero en 1: " + scores[personajes[i]].atLeastOneRate + "%, Primero en 0: " + scores[personajes[i]].zeroRate + "%");    
            //Debug.Log("Primero o igual en todas: " + scores[personajes[i]].allEqualRate + "%, Primero o igual en 2: " + scores[personajes[i]].atLeastTwoEqualRate + "%, Primero o igual en 1: " + scores[personajes[i]].atLeastOneEqualRate + "%, Primero o igual en 0: " + scores[personajes[i]].zeroEqualRate + "%");    
        }
    }

    /*void PrintScorecard()
    {
        Debug.Log("Printing Scorecard");
        List<string> lines = new List<string>();
        string temp;
        for (int i = 0; i < personajes.Count; i++){
            temp = personajes[i].nombre + " - Combates: " + scores[personajes[i]].combatesJugados + ", winrate: " + scores[personajes[i]].winrate*100 + "%";
            lines.Add(temp);
        }
        File.WriteAllLinesAsync("Scorecard.txt", lines);
    }*/
}
        