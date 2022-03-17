using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multicast : MonoBehaviour
{
    public delegate void Del(string message);
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    
    void DelegateMethod1(string message)
	{
        Debug.Log("#1 " + message);
	}

    void DelegateMethod2(string message)
	{
        Debug.Log("#2 " + message);
	}
    void DelegateMethod3(string message)
	{
        Debug.Log("#3 " + message);
        //DeleteMethod();

        
	}

    void Init()
	{
        Del handler = DelegateMethod1; // 인스턴스화 한 handler 변수
        handler += DelegateMethod2; // 계속해서 새로운 delegate 추가
        handler += DelegateMethod3;
          
        handler("Hello Delegate");
        handler -= DelegateMethod2;

        int invocationCount = handler.GetInvocationList().GetLength(0);
        Debug.Log(invocationCount);
    }
    void DeleteMethod()
	{
        
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
