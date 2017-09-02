using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollection : MonoBehaviour
{
    public string CollectionName;

    public T[] GetChildren<T>()
    {
        return this.transform.GetComponentsInChildren<T>();
    }  

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
