using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    public int max;
    public float weight;
    public Text text;
    public string name;
    public Sprite image;
    public GameObject popup;

    private GameObject PopUp;
    private int ammo;
    private List<GameObject> objects;
    private int slot;
    private bool click = true;

    void Start()
    {
        text.text = ammo.ToString();
        PopUp = Instantiate(popup, gameObject.transform.parent.parent.parent);
        PopUp.GetComponent<PopUp>().SetAmmo(ammo);
        PopUp.GetComponent<PopUp>().SetWeight(weight);
        PopUp.GetComponent<PopUp>().SetParent(gameObject);
        PopUp.GetComponent<PopUp>().SetObjects(objects);
    }

    public void BuyAmmo() { ammo = max; text.text = ammo.ToString(); PopUp.GetComponent<PopUp>().SetAmmo(ammo); }
    public void Shoot() { ammo--; text.text = ammo.ToString(); }
    public void SetAmmo(int ammo) { this.ammo = ammo; text.text = ammo.ToString(); }
    public void SetObjects(List<GameObject> objects) { this.objects = objects; }
    public int GetAmmo() { return ammo; }
    public int GetMax() { return max; }
    public void SetSlot(int slot) { this.slot = slot; }
    public int GetSlot() { return slot; }
    public void SetClick(bool click) { this.click = click; }
    public bool GetClick() { return click; }

    public void OpenPopUp()
    {
        if (click)
        {
            PopUp.SetActive(true);
            PopUp.transform.GetChild(0).GetComponent<Text>().text = name;
            PopUp.transform.GetChild(1).GetComponent<Text>().text = ammo + "ÿÚ";
            PopUp.transform.GetChild(2).GetComponent<Text>().text = weight * ammo + " „";
            PopUp.transform.GetChild(3).GetComponent<Image>().sprite = image;
        }
    }

    public void Delete()
    {
        Destroy(PopUp);
        Destroy(gameObject);
    }
}
