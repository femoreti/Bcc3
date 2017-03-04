using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct UserBasics
{
    public string name;
    public int arrivalTurn;
    public List<char> walkOrder;
}

public class User : MonoBehaviour
{
    public UserBasics userStats;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
