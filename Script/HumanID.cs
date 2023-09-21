using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanID : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 獲取物件的Transform組件
        Transform objectTransform = transform;

        // 獲取物件的InstanceID
        int instanceID = GetInstanceID();

        // 輸出物件的Transform和InstanceID
        Debug.Log("HumanTransform：" + objectTransform);
        Debug.Log("HumanID：" + instanceID);
    }
}
