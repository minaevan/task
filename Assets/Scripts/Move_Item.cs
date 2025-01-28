using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Move_Item : MonoBehaviour
{
    private GameObject[] Inventory;
    private Vector3 offset;
    private Transform oldParent;
    private bool move = true;

    public void SetMove(bool yes) { move = yes; }
    public void SetInventory(GameObject[] inv) { Inventory = inv; }

    public void OnMouseDown()
    {
        if (move)
        {
            offset = gameObject.transform.position - Input.mousePosition;
            oldParent = gameObject.transform.parent;
        }
    }

    public void OnMouseDrag()
    {
        if (move)
        {
            gameObject.transform.SetParent(Inventory[0].transform.parent);
            gameObject.transform.position = Input.mousePosition + offset;
            if (gameObject.tag == "ammo") { gameObject.GetComponent<Gun>().SetClick(false); }
            else if (gameObject.tag == "hat" || gameObject.tag == "jacket") { gameObject.GetComponent<Clothing>().SetClick(false); }
            else { gameObject.GetComponent<Medicine>().SetClick(false); }
        }
    }

    public void OnMouseUp()
    {
        if (gameObject.tag == "ammo") { if (gameObject.GetComponent<Gun>().GetClick()) { gameObject.GetComponent<Gun>().OpenPopUp(); } }
        else if (gameObject.tag == "hat" || gameObject.tag == "jacket") { if (gameObject.GetComponent<Clothing>().GetClick()) { gameObject.GetComponent<Clothing>().OpenPopUp(); } }
        else { if (gameObject.GetComponent<Medicine>().GetClick()) { gameObject.GetComponent<Medicine>().OpenPopUp(); } }
        if (move)
        {
            bool yes = true;
            foreach (GameObject inv in Inventory)
            {
                if (gameObject.transform.position.x > inv.transform.position.x - 20 && gameObject.transform.position.y > inv.transform.position.y - 20 &&
                    gameObject.transform.position.x < inv.transform.position.x + 20 && gameObject.transform.position.y < inv.transform.position.y + 20 &&
                    inv.transform.childCount == 0)
                {
                    gameObject.transform.SetParent(inv.transform);
                    if (gameObject.tag == "jacket" || gameObject.tag == "hat") { gameObject.GetComponent<Clothing>().SetSlot(GetIndex(inv, Inventory)); }
                    else if (gameObject.tag == "ammo") { gameObject.GetComponent<Gun>().SetSlot(GetIndex(inv, Inventory)); }
                    else if (gameObject.tag == "medicine") { gameObject.GetComponent<Medicine>().SetSlot(GetIndex(inv, Inventory)); }
                    gameObject.transform.position = inv.transform.position;
                    yes = false;
                }
            }
            if (yes)
            {
                gameObject.transform.SetParent(oldParent);
                gameObject.transform.position = oldParent.position;
            }
            if (gameObject.tag == "ammo") { gameObject.GetComponent<Gun>().SetClick(true); }
            else if (gameObject.tag == "hat" || gameObject.tag == "jacket") { gameObject.GetComponent<Clothing>().SetClick(true); }
            else { gameObject.GetComponent<Medicine>().SetClick(true); }
        }
    }

    public int GetIndex(GameObject obj, GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (obj == array[i]) return i;
        }
        return 0;
    }
}
