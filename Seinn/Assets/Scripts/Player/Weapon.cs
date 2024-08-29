using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mehroz;
using TMPro;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    //public GameObject sumBullet;
    //public GameObject difBullet;
    //public GameObject multBullet;
    //public GameObject divBullet;
    private Utils.NumberType[] numTypes = new Utils.NumberType[4]{ Utils.NumberType.Natural, Utils.NumberType.Natural, Utils.NumberType.Natural, Utils.NumberType.Natural };
    public GameObject[] bulletsCanvas = new GameObject[4];
    public GameObject[] bullets = new GameObject[4];
    public double[] bulletDmg = new double[4] {20f,20f,20f,20f};
    public int selectedWeapon;
    public int activeWeapons;
    public Vector2 mov;
    private bool redUnlocked = false;
    private bool greenUnlocked = false;
    private bool blackUnlocked = false;

    void Start()
    {
        selectedWeapon = 0;
        activeWeapons = 0;
        GameEvents.current.onQuestCompleted += UnlockingWeapons;
        GameEvents.current.onFirstQuestAssign += UnlockFirstWeapon;
        GameEvents.current.onQuestAssign += UpdateCanvasExtra;
        UpdateCanvas();
    }

    private void UpdateCanvasExtra(Transform ts)
    {
        UpdateCanvas();
    }

    private void UnlockFirstWeapon()
    {
        activeWeapons++; //should be 1
        selectedWeapon = 0;
        UpdateCanvas();
        GameEvents.current.ShowCustomDialogue("Ahora puedes usar la suma! Selecciona el proyectil azul");
        LeanTween.moveLocalY(bulletsCanvas[0], 0f, 1f);
    }

    private void UnlockingWeapons(Quest quest)
    {
        StartCoroutine(UnlockingWeaponRoutine(quest));
    }

    private IEnumerator UnlockingWeaponRoutine(Quest quest)
    {
        yield return new WaitForSeconds(5f);
        if (quest.id == 10001 && !redUnlocked)
        {
            //Completed first quest
            activeWeapons++; //should be 2
            GameEvents.current.ShowCustomDialogue("Ahora puedes usar la resta! Selecciona el proyectil rojo");
            LeanTween.moveLocalY(bulletsCanvas[1], 0f, 1f).setOnComplete(() => { UpdateCanvas(); });
            redUnlocked = true;
        }
        else if (quest.id == 10002 && !greenUnlocked)
        {
            //Completed second quest
            activeWeapons++; //should be 3
            GameEvents.current.ShowCustomDialogue("Ahora puedes usar la multiplicacion! Selecciona el proyectil verde");
            LeanTween.moveLocalY(bulletsCanvas[2], 0f, 1f).setOnComplete(() => { UpdateCanvas(); });
            greenUnlocked = true;
        }
        else if (quest.id == 10003 && !blackUnlocked)
        {
            //Completed third quest
            activeWeapons++; //should be 4 (all weapons)
            GameEvents.current.ShowCustomDialogue("Ahora puedes usar la division! Selecciona el proyectil gris");
            LeanTween.moveLocalY(bulletsCanvas[3], 0f, 1f).setOnComplete(() => { UpdateCanvas(); });
            blackUnlocked = true;
        }
        else
            yield return null;
    }

    public void UpdateCanvas()
    {
        int n = 0;
        foreach (GameObject bulletCanvas in bulletsCanvas)
        {
            if (n == activeWeapons) break;
            //Number.GetComponent<UnityEngine.UI.Text>().text = bulletDmg[n].ToString();
            bulletDmg[n] = CustomNumber.RoundDown(bulletDmg[n], 2);
            bulletCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GetBulletDmgString(n);
            if (selectedWeapon == n)
                bulletCanvas.transform.GetChild(0).GetComponent<Animator>().SetBool("active", true);
            else
                bulletCanvas.transform.GetChild(0).GetComponent<Animator>().SetBool("active", false);
            n++;
        }
    }

    private string GetBulletDmgString(int n)
    {
        int activeQuest = QuestManager.current.activeQuest;
        if(activeQuest != -1)
        {
            Utils.NumberType nT = NumberConfig.current.GetNumberType(activeQuest);
            numTypes[n] = nT;
            return new CustomNumber(n, bulletDmg[n], numTypes[n]).ToString();
        }
        else
        {
            return new CustomNumber(n, bulletDmg[n], numTypes[n]).ToString();
        }
    }

    public void UpdateSingleNumber(object[] args)
    {
        int n = (int)args[0];
        double newDamage = (double)args[1];

        bulletDmg[n] = newDamage;
        bulletsCanvas[n].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = newDamage.ToString();

    }

    // Update is called once per frame  
    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 ||  Input.GetAxisRaw("Vertical") != 0)
        {
            mov = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        
        if (Input.GetButtonDown("Fire1") && (mov.x!=0 || mov.y!=0))
        {
            Shoot();
        }

        if (Input.GetButtonDown("Fire2") && activeWeapons != 0)
        {
            switch (Input.GetAxisRaw("Fire2"))
            {
                case 1:
                    selectedWeapon = Mathf.Abs((++selectedWeapon) % activeWeapons);
                    UpdateCanvas();
                    //Debug.Log(selectedWeapon);
                    break;
                case -1:
                    selectedWeapon = selectedWeapon==0?(activeWeapons-1):Mathf.Abs((--selectedWeapon) % activeWeapons);
                    UpdateCanvas();
                    //Debug.Log(selectedWeapon);
                    break;
                default:
                    break;
            }
        }

        if (Input.GetButtonDown("RotateNumbers") && activeWeapons != 0)
        {

            switch (Input.GetAxisRaw("RotateNumbers"))
            {
                case 1:
                    rotateRight();
                    UpdateCanvas();
                    break;
                case -1:
                    rotateLeft();
                    UpdateCanvas();
                    break;
                default:
                    break;
            }
        }

    }

    void rotateRight()
    {
        int i;
        double x = bulletDmg[activeWeapons - 1];

        for (i = activeWeapons - 1; i > 0; i--)
            bulletDmg[i] = bulletDmg[i - 1];
        bulletDmg[0] = x;
    }

    void rotateLeft()
    {
        int i;
        double x = bulletDmg[0];

        for (i = 0; i < activeWeapons - 1; i++)
            bulletDmg[i] = bulletDmg[i + 1];
        bulletDmg[activeWeapons - 1] = x;
    }

    void Shoot()
    {
        if(activeWeapons != 0)
        {
            GameObject bullet = Instantiate(bullets[selectedWeapon], firePoint.position, Quaternion.identity) as GameObject;

            object[] tempStorage = new object[2];
            tempStorage[0] = new Vector2(mov.x, mov.y);
            tempStorage[1] = bulletDmg[selectedWeapon];

            bullet.SendMessage("ChangeDirectionAndDamage", tempStorage);
        }        
    }
}
