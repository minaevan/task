using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Data
{
    public int player_damage, player_protection, _player_hp, _enemy_hp, enemy_damage;
    public int[] GunAmmos, MachineGunAmmos, Jackets, BulletproofVests, Hats, Helmets, Medicines;
    public int[] GunCount, MachineGunCount, MedicineCount;
}

public class Game : MonoBehaviour
{
    public int gun_damage, mashine_gun_damage;
    public GameObject player_hp, enemy_hp, gun_ammo, machine_gun_ammo, jacket, bulletproof_vest, hat, helmet, medicine;
    public GameObject[] Inventory;
    public GameObject HatSlot, JacketSlot;
    public Text HatProtection, JacketProtection;
    public GameObject gameover;
    public GameObject[] player_hits, enemy_hits;

    private int player_damage, player_protection = 0, _player_hp = 100, _enemy_hp = 100, enemy_damage = 15;
    private List<GameObject> GunAmmos, MachineGunAmmos, Jackets, BulletproofVests, Hats, Helmets, Medicines;

    public int[] CreateSlot(List<GameObject> array)
    {
        int[] slot = new int[array.Count];
        if (array == GunAmmos || array == MachineGunAmmos) { for (int i = 0; i < array.Count; i++) { slot[i] = array[i].GetComponent<Gun>().GetSlot(); } }
        else if (array == Medicines) { for (int i = 0; i < array.Count; i++) { slot[i] = array[i].GetComponent<Medicine>().GetSlot(); } }
        else
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].transform.parent == HatSlot.transform) { slot[i] = -1; }
                else if (array[i].transform.parent == JacketSlot.transform) { slot[i] = -2; }
                else { slot[i] = array[i].GetComponent<Clothing>().GetSlot(); }
            }
        }
        return slot;
    }

    public int[] GetCounts(List<GameObject> array)
    {
        int[] count = new int[array.Count];
        if (array == Medicines) { for (int i = 0; i < array.Count; i++) { count[i] = array[i].GetComponent<Medicine>().GetCount(); } }
        else { for (int i = 0; i < array.Count; i++) { count[i] = array[i].GetComponent<Gun>().GetAmmo(); } }
        return count;
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        Data data = new Data()
        {
            player_damage = this.player_damage,
            player_protection = this.player_protection,
            _player_hp = this._player_hp,
            _enemy_hp = this._enemy_hp,
            enemy_damage = this.enemy_damage,
            GunAmmos = CreateSlot(this.GunAmmos),
            MachineGunAmmos = CreateSlot(this.MachineGunAmmos),
            Jackets = CreateSlot(this.Jackets),
            BulletproofVests = CreateSlot(this.BulletproofVests),
            Hats = CreateSlot(this.Hats),
            Helmets = CreateSlot(this.Helmets),
            Medicines = CreateSlot(this.Medicines),
            MedicineCount = GetCounts(this.Medicines),
            GunCount = GetCounts(this.GunAmmos),
            MachineGunCount = GetCounts(this.MachineGunAmmos)        
        };
        formatter.Serialize(file, data);
        file.Close();
        data = null;
    }

    public void Load()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
        Data data = (Data)formatter.Deserialize(file);
        file.Close();

        player_damage = data.player_damage;
        player_protection = data.player_protection;
        _player_hp = data._player_hp;
        _enemy_hp = data._enemy_hp;
        enemy_damage = data.enemy_damage;

        player_hp.GetComponent<Image>().fillAmount = (float)_player_hp / 100;
        player_hp.transform.GetChild(0).GetComponent<Text>().text = _player_hp.ToString();
        enemy_hp.GetComponent<Image>().fillAmount = (float)_enemy_hp / 100;
        enemy_hp.transform.GetChild(0).GetComponent<Text>().text = _enemy_hp.ToString();

        for (int i = 0; i < data.GunAmmos.Length; i++) { SpawnGunAmmo(data.GunCount[i], data.GunAmmos[i]); }
        for (int i = 0; i < data.MachineGunAmmos.Length; i++) { SpawnMachineGunAmmo(data.MachineGunCount[i], data.MachineGunAmmos[i]); }
        for (int i = 0; i < data.Medicines.Length; i++) { SpawnMedicine(data.MedicineCount[i], data.Medicines[i]); }
        for (int i = 0; i < data.Jackets.Length; i++) { SpawnJacket(data.Jackets[i]); }
        for (int i = 0; i < data.BulletproofVests.Length; i++) { SpawnBulletproofVest(data.BulletproofVests[i]); }
        for (int i = 0; i < data.Hats.Length; i++) { SpawnHat(data.Hats[i]); }
        for (int i = 0; i < data.Helmets.Length; i++) { SpawnHelmet(data.Helmets[i]); }

        data = null;
    }

    public void SetProtection(int protection) { player_protection += protection; }
    public void Heal()
    {
        if (_player_hp + 50 > 100) { _player_hp = 100; }
        else { _player_hp += 50; }
        player_hp.GetComponent<Image>().fillAmount = (float)_player_hp / 100;
        player_hp.transform.GetChild(0).GetComponent<Text>().text = _player_hp.ToString();
    }

    public void SpawnGunAmmo(int ammo, int slot)
    {
        GameObject GunAmmo = Instantiate(gun_ammo, Inventory[slot].transform);
        GunAmmo.GetComponent<Move_Item>().SetInventory(Inventory);
        GunAmmo.GetComponent<Gun>().SetAmmo(ammo);
        GunAmmo.GetComponent<Gun>().SetSlot(slot);
        GunAmmos.Add(GunAmmo);
        GunAmmo.GetComponent<Gun>().SetObjects(GunAmmos);
    }

    public void SpawnMachineGunAmmo(int ammo, int slot)
    {
        GameObject MachineGunAmmo = Instantiate(machine_gun_ammo, Inventory[slot].transform);
        MachineGunAmmo.GetComponent<Move_Item>().SetInventory(Inventory);
        MachineGunAmmo.GetComponent<Gun>().SetAmmo(ammo);
        MachineGunAmmo.GetComponent<Gun>().SetSlot(slot);
        MachineGunAmmos.Add(MachineGunAmmo);
        MachineGunAmmo.GetComponent<Gun>().SetObjects(MachineGunAmmos);
    }

    public void SpawnJacket(int slot)
    {
        GameObject Jacket;
        if (slot == -2) { Jacket = Instantiate(jacket, JacketSlot.transform); }
        else { Jacket = Instantiate(jacket, Inventory[slot].transform); }
        Jacket.GetComponent<Move_Item>().SetInventory(Inventory);
        Jacket.GetComponent<Clothing>().SetClothing(gameObject, HatSlot, JacketSlot, HatProtection, JacketProtection, Jackets);
        Jacket.GetComponent<Clothing>().SetSlot(slot);
        if (slot == -2) { Jacket.GetComponent<Clothing>().SetProtection(); Jacket.GetComponent<Move_Item>().SetMove(false); }
        Jackets.Add(Jacket);
    }

    public void SpawnBulletproofVest(int slot)
    {
        GameObject BulletproofVest;
        if (slot == -2) { BulletproofVest = Instantiate(bulletproof_vest, JacketSlot.transform); }
        else { BulletproofVest = Instantiate(bulletproof_vest, Inventory[slot].transform); }
        BulletproofVest.GetComponent<Move_Item>().SetInventory(Inventory);
        BulletproofVest.GetComponent<Clothing>().SetClothing(gameObject, HatSlot, JacketSlot, HatProtection, JacketProtection, BulletproofVests);
        BulletproofVest.GetComponent<Clothing>().SetSlot(slot);
        if (slot == -2) { BulletproofVest.GetComponent<Clothing>().SetProtection(); BulletproofVest.GetComponent<Move_Item>().SetMove(false); }
        BulletproofVests.Add(BulletproofVest);
    }

    public void SpawnHat(int slot)
    {
        GameObject Hat;
        if (slot == -1) { Hat = Instantiate(hat, HatSlot.transform); }
        else { Hat = Instantiate(hat, Inventory[slot].transform); }
        Hat.GetComponent<Move_Item>().SetInventory(Inventory);
        Hat.GetComponent<Clothing>().SetClothing(gameObject, HatSlot, JacketSlot, HatProtection, JacketProtection, Hats);
        Hat.GetComponent<Clothing>().SetSlot(slot);
        if (slot == -1) { Hat.GetComponent<Clothing>().SetProtection(); Hat.GetComponent<Move_Item>().SetMove(false); }
        Hats.Add(Hat);
    }

    public void SpawnHelmet(int slot)
    {
        GameObject Helmet;
        if (slot == -1) { Helmet = Instantiate(helmet, HatSlot.transform); }
        else { Helmet = Instantiate(helmet, Inventory[slot].transform); }
        Helmet.GetComponent<Move_Item>().SetInventory(Inventory);
        Helmet.GetComponent<Clothing>().SetClothing(gameObject, HatSlot, JacketSlot, HatProtection, JacketProtection, Helmets);
        Helmet.GetComponent<Clothing>().SetSlot(slot);
        if (slot == -1) { Helmet.GetComponent<Clothing>().SetProtection(); Helmet.GetComponent<Move_Item>().SetMove(false); }
        Helmets.Add(Helmet);
    }

    public void SpawnMedicine(int count, int slot)
    {
        GameObject Medicine = Instantiate(medicine, Inventory[slot].transform);
        Medicine.GetComponent<Move_Item>().SetInventory(Inventory);
        Medicine.GetComponent<Medicine>().SetGameManager(gameObject);
        Medicine.GetComponent<Medicine>().SetCount(count);
        Medicine.GetComponent<Medicine>().SetSlot(slot);
        Medicines.Add(Medicine);
        Medicine.GetComponent<Medicine>().SetObjects(Medicines);
    }

    void Start()
    {
        player_damage = gun_damage;

        GunAmmos = new List<GameObject>();
        MachineGunAmmos = new List<GameObject>();
        Jackets = new List<GameObject>();
        BulletproofVests = new List<GameObject>();
        Hats = new List<GameObject>();
        Helmets = new List<GameObject>();
        Medicines = new List<GameObject>();

        if (File.Exists(Application.persistentDataPath + "/save.dat")) { Load(); }
        else
        {
            SpawnGunAmmo(25, 0);
            SpawnMachineGunAmmo(15, 1);
            SpawnJacket(2);
            SpawnBulletproofVest(3);
            SpawnHat(4);
            SpawnHelmet(5);
            SpawnMedicine(4, 6);
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Gun()
    {
        player_damage = gun_damage;
    }

    public void Mashine_gun()
    {
        player_damage = mashine_gun_damage;
    }

    private bool can_shoot = true;
    public void Shoot()
    {
        if (can_shoot)
        {
            if (player_damage == gun_damage && GunAmmos.Count > 0)
            {
                if (_enemy_hp - player_damage <= 0)
                {
                    _enemy_hp = 100;
                    enemy_hp.GetComponent<Image>().fillAmount = (float)_enemy_hp / 100;
                    enemy_hp.transform.GetChild(0).GetComponent<Text>().text = _enemy_hp.ToString();
                    Win();
                    StopAllCoroutines();
                }
                else
                {
                    _enemy_hp -= player_damage;
                    enemy_hp.GetComponent<Image>().fillAmount = (float)_enemy_hp / 100;
                    enemy_hp.transform.GetChild(0).GetComponent<Text>().text = _enemy_hp.ToString();
                }
                enemy_hits[UnityEngine.Random.Range(0, enemy_hits.Length)].GetComponent<Animator>().Play("Hit", -1, 0f);
                GunAmmos[GunAmmos.Count - 1].GetComponent<Gun>().Shoot();
                if (GunAmmos[GunAmmos.Count - 1].GetComponent<Gun>().GetAmmo() <= 0) { GunAmmos[GunAmmos.Count - 1].GetComponent<Gun>().Delete(); GunAmmos.Remove(GunAmmos[GunAmmos.Count - 1]); }
                StartCoroutine(Enemy_shoot());
            }
            else if (player_damage == mashine_gun_damage && MachineGunAmmos.Count > 0)
            {
                StartCoroutine(MachineGun_shoot());
                StartCoroutine(Enemy_shoot());
            }
        }
    }

    public IEnumerator Enemy_shoot()
    {
        can_shoot = false;
        yield return new WaitForSeconds(1.5f);
        player_hits[UnityEngine.Random.Range(0, player_hits.Length)].GetComponent<Animator>().Play("Hit", -1, 0f);
        if (enemy_damage > player_protection)
        {
            if (_player_hp - enemy_damage + player_protection <= 0)
            {
                _player_hp = 0;
                player_hp.GetComponent<Image>().fillAmount = 0;
                player_hp.transform.GetChild(0).GetComponent<Text>().text = "0";
                gameover.SetActive(true);
            }
            else
            {
                _player_hp -= enemy_damage - player_protection;
                player_hp.GetComponent<Image>().fillAmount = (float)_player_hp / 100;
                player_hp.transform.GetChild(0).GetComponent<Text>().text = _player_hp.ToString();
            }
        }
        can_shoot = true;
    }

    public IEnumerator MachineGun_shoot()
    {
        for (int i = 0; i < 3; i++)
        {
            if (MachineGunAmmos.Count > 0)
            {
                if (_enemy_hp - player_damage <= 0)
                {
                    _enemy_hp = 100;
                    enemy_hp.GetComponent<Image>().fillAmount = (float)_enemy_hp / 100;
                    enemy_hp.transform.GetChild(0).GetComponent<Text>().text = _enemy_hp.ToString();
                    Win();
                    StopAllCoroutines();
                }
                else
                {
                    _enemy_hp -= player_damage;
                    enemy_hp.GetComponent<Image>().fillAmount = (float)_enemy_hp / 100;
                    enemy_hp.transform.GetChild(0).GetComponent<Text>().text = _enemy_hp.ToString();
                }
                enemy_hits[UnityEngine.Random.Range(0, enemy_hits.Length)].GetComponent<Animator>().Play("Hit", -1, 0f);
                MachineGunAmmos[MachineGunAmmos.Count - 1].GetComponent<Gun>().Shoot();
                if (MachineGunAmmos[MachineGunAmmos.Count - 1].GetComponent<Gun>().GetAmmo() <= 0) { MachineGunAmmos[MachineGunAmmos.Count - 1].GetComponent<Gun>().Delete(); MachineGunAmmos.Remove(MachineGunAmmos[MachineGunAmmos.Count - 1]); }
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    public void Win()
    {
        SpawnAmmo(GunAmmos);
        SpawnAmmo(MachineGunAmmos);       
        SpawnClothing();
        SpawnHeal(Medicines);
        Save();
    }

    public void SpawnAmmo(List<GameObject> array)
    {
        int oldcount = array.Count;
        int rnd = UnityEngine.Random.Range(0, 11);
        if (array.Count == 0)
        {
            foreach (GameObject inv in Inventory)
            {
                if (inv.transform.childCount == 0 && array == GunAmmos) { SpawnGunAmmo(rnd, GetIndex(inv, Inventory)); break; }
                else if (inv.transform.childCount == 0 && array == MachineGunAmmos) { SpawnMachineGunAmmo(rnd, GetIndex(inv, Inventory)); break; }
            }
        }
        else if (array[array.Count - 1].GetComponent<Gun>().GetAmmo() + rnd <= array[array.Count - 1].GetComponent<Gun>().GetMax())
        {
            array[array.Count - 1].GetComponent<Gun>().SetAmmo(array[array.Count - 1].GetComponent<Gun>().GetAmmo() + rnd);
        }
        else
        {
            foreach (GameObject inv in Inventory)
            {
                if (inv.transform.childCount == 0 && array == GunAmmos) { SpawnGunAmmo(rnd - (array[array.Count - 1].GetComponent<Gun>().GetMax() - array[array.Count - 1].GetComponent<Gun>().GetAmmo()), GetIndex(inv, Inventory)); break; }
                else if (inv.transform.childCount == 0 && array == MachineGunAmmos) { SpawnMachineGunAmmo(rnd - (array[array.Count - 1].GetComponent<Gun>().GetMax() - array[array.Count - 1].GetComponent<Gun>().GetAmmo()), GetIndex(inv, Inventory)); break; }
            }
            if (oldcount != array.Count) { array[array.Count - 2].GetComponent<Gun>().SetAmmo(array[array.Count - 2].GetComponent<Gun>().GetMax()); }
            else { array[array.Count - 1].GetComponent<Gun>().SetAmmo(array[array.Count - 1].GetComponent<Gun>().GetMax()); }
        }
    }

    public void SpawnClothing()
    {
        int rnd = UnityEngine.Random.Range(0, 4);
        if (rnd == 0) { foreach (GameObject inv in Inventory) { if (inv.transform.childCount == 0) { SpawnJacket(GetIndex(inv, Inventory)); break; } } }
        else if (rnd == 1) { foreach (GameObject inv in Inventory) { if (inv.transform.childCount == 0) { SpawnBulletproofVest(GetIndex(inv, Inventory)); break; } } }
        else if (rnd == 2) { foreach (GameObject inv in Inventory) { if (inv.transform.childCount == 0) { SpawnHat(GetIndex(inv, Inventory)); break; } } }
        else { foreach (GameObject inv in Inventory) { if (inv.transform.childCount == 0) { SpawnHelmet(GetIndex(inv, Inventory)); break; } } }
    }

    public void SpawnHeal(List<GameObject> array)
    {
        int oldcount = array.Count;
        int rnd = UnityEngine.Random.Range(0, 4);
        if (array.Count == 0) { foreach (GameObject inv in Inventory) { if (inv.transform.childCount == 0) { SpawnMedicine(rnd, GetIndex(inv, Inventory)); break; } } }
        else if (array[array.Count - 1].GetComponent<Medicine>().GetCount() + rnd <= array[array.Count - 1].GetComponent<Medicine>().GetMax()) { array[array.Count - 1].GetComponent<Medicine>().SetCount(array[array.Count - 1].GetComponent<Medicine>().GetCount() + rnd); }
        else
        {
            foreach (GameObject inv in Inventory) { if (inv.transform.childCount == 0) { SpawnMedicine(rnd - (array[array.Count - 1].GetComponent<Medicine>().GetMax() - array[array.Count - 1].GetComponent<Medicine>().GetCount()), GetIndex(inv, Inventory)); break; } }
            if (oldcount != array.Count) { array[array.Count - 2].GetComponent<Medicine>().SetCount(array[array.Count - 2].GetComponent<Medicine>().GetMax()); }
            else { array[array.Count - 1].GetComponent<Medicine>().SetCount(array[array.Count - 1].GetComponent<Medicine>().GetMax()); }
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene(0);
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
