﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class UserCreator : MonoBehaviour
{
    public static UserCreator Instance;
    public GameObject _prefabUser;
    public List<UserBasics> _userLine = new List<UserBasics>();
    public float userSize;

	// Use this for initialization
	void Awake ()
    {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    //irá ler o arquivo fila.txt
    public void ReadFilaFile()
    {
        string myTxt = File.ReadAllText(Application.streamingAssetsPath + "/Fila.txt");

        string[] stringSeparators = new string[] { "\r\n" };

        StringReader reader = new StringReader(myTxt);
        string txtToSplit = myTxt;
        string[] lineSplitEnters = txtToSplit.Split(stringSeparators, System.StringSplitOptions.None);

        //UserCreator uc = GameObject.Find("UserCreator").GetComponent<UserCreator>();
        _userLine.Clear();
        for (int i = 0; i < lineSplitEnters.Length; i++)
        {
            UserBasics ub = new UserBasics();

            string line = lineSplitEnters[i];
            if(string.IsNullOrEmpty(line))
            {
                //Debug.Log("null line");
                break;
            }
            //Debug.Log("################## - " + line);
            string userName = lineSplitEnters[i].Split('C')[0];
            string turnoChegada = "C" + lineSplitEnters[i].Split('C')[1].Split('A')[0];
            string ordemGuiches = 'A' + line.Split('A')[1];

            ub.name = userName;
            ub.arrivalTurn = int.Parse(turnoChegada.Replace("C", ""));
            ub.walkOrder = new List<char>();
            //Debug.Log("name - " + ub.name);
            //Debug.Log("chegada - " + ub.arrivalTurn);

            //Debug.Log("guiches - " + ordemGuiches);
            ub.order = ordemGuiches;
            for (int j = 0; j < ordemGuiches.Length; j++)
            {
                ub.walkOrder.Add(ordemGuiches[j]);
            }

            _userLine.Add(ub);
        }
    }
}
