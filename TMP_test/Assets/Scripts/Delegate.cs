using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delegate : MonoBehaviour
{
    public delegate void Del(string message);
    // Start is called before the first frame update
    void Start()
    {
        //Del handler = new Del(DelegateMethod);
        Del handler = DelegateMethod;
        //handler("Hello Delegate");
        RunHeavyJob(handler);
    }

    void DelegateMethod(string message)
	{
        Debug.Log("Result = " + message);
	}
    // Update is called once per frame
    void Update()
    {
        
    }

    void RunHeavyJob(Del handler)
	{
        string result = "오래 걸리는 작업";
        handler(result);
	}
}
