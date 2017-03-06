using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Recebe os dados do arquivo Setup.txt
public class Posto: MonoBehaviour {
    public static Posto INSTANCE;
	public char letra { get; set; }
	public int turnos { get; set; }
	public int quantidade { get; set; }

    void Awake()
    {
        INSTANCE = this;
    }
}
