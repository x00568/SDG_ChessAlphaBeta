using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetContainer : MonoBehaviour
{
    Container container;
    void Start()
    {
        container = GetComponent<Container>();
    }
    
}
