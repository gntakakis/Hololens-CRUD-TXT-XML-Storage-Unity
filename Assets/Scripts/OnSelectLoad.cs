using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSelectLoad : MonoBehaviour {
    void OnSelect()
    {
        CRUDManager.Instance.LoadPins();
    }
}
