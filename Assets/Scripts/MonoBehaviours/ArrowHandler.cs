using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHandler : MonoBehaviour {

    [Header("Arrow Prefabs")]
    [Header("For Character Values")]
    [SerializeField] GameObject greenArrowSmall;
    [SerializeField] GameObject redArrowSmall;
    [Header("For Ressource Values")]
    [SerializeField] GameObject greenArrowBig;
    [SerializeField] GameObject redArrowBig;

    private void OnEnable()
    {
        //GameManager.Instance.OnArrowCall += DisplayArrow;
    }

    private void OnDisable()
    {
        //GameManager.Instance.OnArrowCall -= DisplayArrow;
    }

    void DisplayArrow()
    {

    }
}
