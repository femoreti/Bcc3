using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileReader : MonoBehaviour {
	public int qtdAtendentes = 0;
	List<string> relPostos = new List<string>();
	List<string> relAtendentes = new List<string>();
	List<string> tempoPostos = new List<string>();

	public void ReadFile() {
		string txt = File.ReadAllText("test.txt");
		string[] txtArray = txt.Split('\n');
		qtdAtendentes = Convert.ToInt16(txtArray[0]);

		foreach (var item in txtArray) {
			var temp = item.Split(':');
			if (temp[0] == "RP") {
				relPostos.Add(temp[1].Replace("\r", ""));
			} else if (temp[0] == "RA") {
				relAtendentes.Add(temp[1].Replace("\r", ""));
			} else {
				tempoPostos.Add(temp[0]);
			}
		}

		tempoPostos.RemoveAt(0);
		tempoPostos.Remove("TROCA");
	}
}
