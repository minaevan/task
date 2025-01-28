using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Medicine : MonoBehaviour
{
    public float weight;
    public int max;
    public string name;
    public Sprite image;
    public GameObject popup;
    public Text text;

    private GameObject PopUp;
    private GameObject GameManager;
    private int count;
    private List<GameObject> objects;
    private int slot;
    private bool click = true;

    public void SetClick(bool click) { this.click = click; }
    public bool GetClick() { return click; }
    public void SetSlot(int slot) { this.slot = slot; }
    public int GetSlot() { return slot; }
    public void SetGameManager(GameObject GameManager) { this.GameManager = GameManager; }
    public void SetObjects(List<GameObject> objects) { this.objects = objects; }
    public void Heal() { count--; text.text = count.ToString(); PopUp.GetComponent<PopUp>().SetCount(count); }
    public void SetCount(int count) { this.count = count; text.text = count.ToString(); }
    public int GetCount() { return count; }
    public int GetMax() { return max; }

    void Start()
    {
        text.text = count.ToString();
        PopUp = Instantiate(popup, gameObject.transform.parent.parent.parent);
        PopUp.GetComponent<PopUp>().SetParent(gameObject);
        PopUp.GetComponent<PopUp>().SetGameManager(GameManager);
        PopUp.GetComponent<PopUp>().SetObjects(objects);
        PopUp.GetComponent<PopUp>().SetWeight(weight);
    }

    public void OpenPopUp()
    {
        if (click)
        {
            PopUp.SetActive(true);
            PopUp.transform.GetChild(0).GetComponent<Text>().text = name;
            PopUp.transform.GetChild(1).GetComponent<Text>().text = count + "ÿÚ";
            PopUp.transform.GetChild(2).GetComponent<Text>().text = weight * count + " „";
            PopUp.transform.GetChild(3).GetComponent<Image>().sprite = image;
        }
    }
}
