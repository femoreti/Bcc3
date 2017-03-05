using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posto {
	public char letra { get; set; }
	public int turnos { get; set; }
	public int quantidade { get; set; }

	public Posto(char letra, int turnos, int quantidade)
	{
		this.letra = letra;
		this.turnos = turnos;
		this.quantidade = quantidade;
	}
}
