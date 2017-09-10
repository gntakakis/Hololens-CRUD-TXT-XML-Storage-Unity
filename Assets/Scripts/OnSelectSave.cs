using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSelectSave : MonoBehaviour {
    void OnSelect()
    {
        #if WINDOWS_UWP
        CRUDManager.Instance.SavePins();
        #endif
    }
}
