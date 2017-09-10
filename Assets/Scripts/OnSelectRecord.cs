using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSelectRecord : MonoBehaviour {
    void OnSelect()
    {
        CRUDManager.Instance.RecordCursorPins();
    }
}
