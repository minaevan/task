using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopUp : MonoBehaviour
{
    private GameObject GameManager;
    private GameObject HatSlot, JacketSlot;
    private Text HatProtection, JacketProtection;
    private int ammo, count;
    private float weight;
    private GameObject parent;
    private List<GameObject> objects;

    public void SetPopUp(GameObject GameManager, GameObject HatSlot, GameObject JacketSlot, Text HatProtection, Text JacketProtection, List<GameObject> objects)
    {
        this.GameManager = GameManager;
        this.HatSlot = HatSlot;
        this.JacketSlot = JacketSlot;
        this.HatProtection = HatProtection;
        this.JacketProtection = JacketProtection;
        this.objects = objects;
    }

    public void SetWeight(float weight) { this.weight = weight; }
    public void SetGameManager(GameObject GameManager) { this.GameManager = GameManager; }
    public void SetAmmo(int ammo) { this.ammo = ammo; }
    public void SetObjects(List<GameObject> objects) { this.objects = objects; }
    public void SetCount(int count) { this.count = count; }
    public void SetParent(GameObject parent) { this.parent = parent; }
    public void ClosePopUp() { gameObject.SetActive(false); }

    public void Buy()
    {
        parent.GetComponent<Gun>().BuyAmmo();
        gameObject.transform.GetChild(1).GetComponent<Text>().text = ammo + "ÿÚ";
        gameObject.transform.GetChild(2).GetComponent<Text>().text = weight * ammo + " „";
    }

    public void Wear()
    {
        if (parent.tag == "hat")
        {
            if (HatSlot.transform.childCount == 0)
            {
                parent.transform.SetParent(HatSlot.transform);
                parent.transform.position = HatSlot.transform.position;
                parent.GetComponent<Move_Item>().SetMove(false);
                GameManager.GetComponent<Game>().SetProtection(parent.GetComponent<Clothing>().GetProtection());
                HatProtection.text = parent.GetComponent<Clothing>().GetProtection().ToString();
            }
            else
            {
                GameObject oldhat = HatSlot.transform.GetChild(0).gameObject;
                oldhat.transform.SetParent(parent.transform.parent);
                oldhat.transform.position = oldhat.transform.parent.position;
                oldhat.GetComponent<Move_Item>().SetMove(true);
                GameManager.GetComponent<Game>().SetProtection(-oldhat.GetComponent<Clothing>().GetProtection());

                parent.transform.SetParent(HatSlot.transform);
                parent.transform.position = HatSlot.transform.position;
                parent.GetComponent<Move_Item>().SetMove(false);
                GameManager.GetComponent<Game>().SetProtection(parent.GetComponent<Clothing>().GetProtection());
                HatProtection.text = parent.GetComponent<Clothing>().GetProtection().ToString();
            }
            ClosePopUp();
        }
        else if (parent.tag == "jacket")
        {
            if (JacketSlot.transform.childCount == 0)
            {
                parent.transform.SetParent(JacketSlot.transform);
                parent.transform.position = JacketSlot.transform.position;
                parent.GetComponent<Move_Item>().SetMove(false);
                GameManager.GetComponent<Game>().SetProtection(parent.GetComponent<Clothing>().GetProtection());
                JacketProtection.text = parent.GetComponent<Clothing>().GetProtection().ToString();
            }
            else
            {
                GameObject oldjacket = JacketSlot.transform.GetChild(0).gameObject;
                oldjacket.transform.SetParent(parent.transform.parent);
                oldjacket.transform.position = oldjacket.transform.parent.position;
                oldjacket.GetComponent<Move_Item>().SetMove(true);
                GameManager.GetComponent<Game>().SetProtection(-oldjacket.GetComponent<Clothing>().GetProtection());

                parent.transform.SetParent(JacketSlot.transform);
                parent.transform.position = JacketSlot.transform.position;
                parent.GetComponent<Move_Item>().SetMove(false);
                GameManager.GetComponent<Game>().SetProtection(parent.GetComponent<Clothing>().GetProtection());
                JacketProtection.text = parent.GetComponent<Clothing>().GetProtection().ToString();
            }
            ClosePopUp();
        }
    }

    public void Heal()
    {
        GameManager.GetComponent<Game>().Heal();
        parent.GetComponent<Medicine>().Heal();
        gameObject.transform.GetChild(1).GetComponent<Text>().text = count + "ÿÚ";
        gameObject.transform.GetChild(2).GetComponent<Text>().text = weight * count + " „";
        if (count == 0) { DeleteWeapon(); }
    }

    public void DeleteClothing()
    {
        if (parent.transform.position == HatSlot.transform.position) { HatProtection.text = "0"; GameManager.GetComponent<Game>().SetProtection(-parent.GetComponent<Clothing>().GetProtection()); }
        if (parent.transform.position == JacketSlot.transform.position) { JacketProtection.text = "0"; GameManager.GetComponent<Game>().SetProtection(-parent.GetComponent<Clothing>().GetProtection()); }
        objects.Remove(parent);
        Destroy(parent);
        Destroy(gameObject);
    }

    public void DeleteWeapon()
    {
        objects.Remove(parent);
        Destroy(parent);
        Destroy(gameObject);
    }
}
