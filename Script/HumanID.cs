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
        // �������Transform�ե�
        Transform objectTransform = transform;

        // �������InstanceID
        int instanceID = GetInstanceID();

        // ��X����Transform�MInstanceID
        Debug.Log("HumanTransform�G" + objectTransform);
        Debug.Log("HumanID�G" + instanceID);
    }
}
