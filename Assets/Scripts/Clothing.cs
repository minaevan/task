using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Clothing : MonoBehaviour
{
    public float weight;
    public int protection;
    public string name;
    public Sprite image;
    public GameObject popup;

    private GameObject PopUp;
    private GameObject GameManager, HatSlot, JacketSlot;
    private Text HatProtection, JacketProtection;
    private List<GameObject> objects;
    private int slot;
    private bool click = true;

    public void SetClick(bool click) { this.click = click; }
    public bool GetClick() { return click; }
    public void SetSlot(int slot) { this.slot = slot; }
    public int GetSlot() { return slot; }
    public int GetProtection() { return protection; }
    public void SetProtection()
    {
        if (gameObject.tag == "hat") { HatProtection.text = protection.ToString(); }
        else { JacketProtection.text = protection.ToString(); }
    }

    public void SetClothing(GameObject GameManager, GameObject HatSlot, GameObject JacketSlot, Text HatProtection, Text JacketProtection, List<GameObject> objects)
    {
        this.GameManager = GameManager;
        this.HatSlot = HatSlot;
        this.JacketSlot = JacketSlot;
        this.HatProtection = HatProtection;
        this.JacketProtection = JacketProtection;
        this.objects = objects;
    }

    void Start()
    {
        PopUp = Instantiate(popup, gameObject.transform.parent.parent.parent);
        PopUp.GetComponent<PopUp>().SetParent(gameObject);
        PopUp.GetComponent<PopUp>().SetPopUp(GameManager, HatSlot, JacketSlot, HatProtection, JacketProtection, objects);
    }

    public void OpenPopUp()
    {
        if (click)
        {
            PopUp.SetActive(true);
            PopUp.transform.GetChild(0).GetComponent<Text>().text = name;
            PopUp.transform.GetChild(1).GetComponent<Text>().text = "+" + protection;
            PopUp.transform.GetChild(2).GetComponent<Text>().text = weight + " „";
            PopUp.transform.GetChild(3).GetComponent<Image>().sprite = image;
        }
    }
}
