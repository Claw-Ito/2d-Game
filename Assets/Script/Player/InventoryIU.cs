using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryIU : MonoBehaviour
{
    public GameObject inventoryPanel;
    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isActive = !isActive;
            inventoryPanel.SetActive(isActive);
        }
    }
}
