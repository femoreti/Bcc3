using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorPosto : MonoBehaviour {
    private int qtdAtendentes = 0;
    private float postoWidth = 0;
    private string relPostos;
    private string relAtendentes;
    private List<string> tempoPostos = new List<string>();
    private Dictionary<char, int> relPostosCount = new Dictionary<char, int>();

    //public Text nomePostoTxt;
    //public Canvas canvas;
    public GameObject postoPrefab;
    public GameObject containerPostos; //temporario ateh proxima versao
    public List<Posto> postos;
    public List<Text> postosNomes;

    public int totalPostosDistindos { get; set; }

    public void Init()
    {
        //postoWidth = postoPrefab.transform.localScale.x;
        ReadTextFile();
        InstanciaPostos();
        PopulaObjetoPosto();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InstanciaPostos()
    {
        postos = new List<Posto>();
        postosNomes = new List<Text>();
        
        int count = 0;
        Vector3 postoPos = postoPrefab.transform.position;

        foreach (char item in relPostos) {
            GameObject newPosto = Instantiate(postoPrefab) as GameObject;
            newPosto.transform.SetParent(containerPostos.transform,false);

            Posto p = newPosto.GetComponent<Posto>();
            p.letra = item;
            postos.Add(p);

            count++;
        }
    }

    private void ReadTextFile()
    {
        string txt = File.ReadAllText(Application.streamingAssetsPath + "/Setup.txt");

        // Caso não conseguimos ler o texto (ou seja ele é nulo ou vazio)
        // Joga uma exception
        if (txt == null || txt == "")
        {
            throw new Exception("Não foi possível ler a entrada.");
        }

        // Quebra o texto recebido no \n (ENTER)
        string[] txtArray = txt.Split('\n');

        // A primeira posição do vetor gerado pelo Split
        // é a quantidade de atendentes
        qtdAtendentes = Convert.ToInt16(txtArray[0]);

        // Se a primeira posição for RP(Relação de Postos) então retiramos
        // o \r do final da string para ficar no formato certo e guardamos
        // na variável. O mesmo para RA, todo o resto da string guardamos 
        // na lista tempoPostos e depois tratamos.
        foreach (var item in txtArray)
        {
            var temp = item.Split(':');
            if (temp[0] == "RP")
            {
                relPostos = temp[1].Replace("\r", "");
            }
            else if (temp[0] == "RA")
            {
                relAtendentes = temp[1].Replace("\r", "");
            }
            else
            {
                tempoPostos.Add(temp[0]);
                //Aqui da para pegar nome dos postos
            }
        }

        // Remove o primeiro item da lista, pois é a qtd de atendentes
        // Remove o TROCA e o ultimo espaço em branco do vetor para 
        // termos so os postos e os turnos
        tempoPostos.RemoveAt(0);
        tempoPostos.Remove("TROCA");
        tempoPostos.Remove("");
    }

    private void PopulaObjetoPosto()
    {
        int totalDeFilasParaCriar = 0;
        // Adcionamos os turnos ao objeto posto
        foreach (string item in tempoPostos)
        {
            string letra = item.Substring(0, 1);
            string turnos = item.Substring(1, item.Length - 1);

            string lastKnowLetter = "";

            foreach (Posto posto in this.postos)
            {
                if (posto.letra.ToString() == letra)
                {
                    if(lastKnowLetter != letra)
                    {
                        lastKnowLetter = letra;
                        totalDeFilasParaCriar++;
                    }
                    posto.name = letra;

                    posto._myType = (FilaType)Enum.Parse(typeof(FilaType), letra.ToString());
                    posto.turnos = Convert.ToInt16(turnos);
                }
            }         
        }

        //Inicia os atendentes nos postos correspondentes
        for (int i = 0; i < relAtendentes.Length; i++)
        {
            //Debug.Log(relAtendentes[i]);
            foreach (Posto p in this.postos)
            {
                if (p.letra == relAtendentes[i])
                {
                    if (!p.temAtendente)
                    {
                        p.setAtendente = true;
                        break;
                    }
                }
            }
        }

        totalPostosDistindos = totalDeFilasParaCriar;
    }
}
