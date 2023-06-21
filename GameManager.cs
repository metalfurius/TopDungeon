using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }


        instance = this;
        SceneManager.sceneLoaded += LoadState;
    }

    //resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //references
    public player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public Animator deathMenuAnim;

    //Logic
    public int pesos;
    public int experience;
    private float playerHP;
    private int playerMAXHP;

    //Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }
    //Upgrade weapon
    public bool TryUpgradeWeapon()
    {
        //is the weapon max level
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;

        if (pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }
        return false;
    }

    //hp bar
    public void OnHitpointChange()
    {
        playerHP = GameObject.Find("Player_0").GetComponent<player>().hitpoint;
        playerMAXHP = GameObject.Find("Player_0").GetComponent<player>().maxHitpoint;
        float ratio = playerHP / playerMAXHP;
        hitpointBar.localScale = new Vector3(1, ratio, 1);

    }

    //experience system
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;
        while (experience >= add)
        {
            add += xpTable[r];
            r++;
            if (r == xpTable.Count)//max level
                return r;
        }
        return r;
    }
    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;
        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }
        return xp;
    }
    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if (currLevel < GetCurrentLevel())
            OnLevelUp();
    }
    public void OnLevelUp()
    {
        Debug.Log("lvl up");
        player.OnLevelUp();
        OnHitpointChange();
    }
    
    public void SaveState()
    {
        player = GameObject.Find("Player_0").GetComponent<player>();
        weapon = GameObject.Find("Weapon0").GetComponent<Weapon>();
        string s = "";
        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState",s);

    }
    public void LoadState(Scene s,LoadSceneMode mode)
    {
        player = GameObject.Find("Player_0").GetComponent<player>();
        weapon = GameObject.Find("Weapon0").GetComponent<Weapon>();
        SceneManager.sceneLoaded -= LoadState;
        if (!PlayerPrefs.HasKey("SaveState"))
        {
            return;
        }
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');
        //change player skin
        pesos = int.Parse(data[1]);
        experience = int.Parse(data[1]);
        player.SetLevel(GetCurrentLevel());
        //change lvl
        weapon.SetWeaponLevel(int.Parse(data[3]));
    }
    //Death menu and respawn
    public void Respawn()
    {
        deathMenuAnim.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        player.Respawn();
    }

}
