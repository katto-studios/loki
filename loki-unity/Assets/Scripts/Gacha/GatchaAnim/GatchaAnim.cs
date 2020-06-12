using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;

public class GatchaAnim : MonoBehaviour
{
    public GameObject effect;
    public GameObject glow;

    public VisualEffect effectE;
    public VisualEffect glowE;

    private Animator anim;
    private Animator rewardAnim;
    private RaycastHit hit;
    private Ray ray;

    private bool caseOpened = false;

    public Transform keyCapSpawn;
    public GameObject RewardPrefab;

    public Color rarityColour;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rarityText;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray,out hit))
        {
            if (hit.transform.name == "ScreenArea")
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Hover", true);

                if (Input.GetMouseButtonDown(0))
                {
                    anim.SetBool("Opened", false);
                    anim.SetBool("Open", true);
                }
            }
            else
            {
                anim.SetBool("Idle", true);
                anim.SetBool("Hover", false);
            }
        }
        else
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Hover", false);
        }

        if (caseOpened)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                anim.SetBool("Open", false);
                anim.SetBool("Opened", true);
                rewardAnim.SetBool("Collected", true);
                caseOpened = false;
                effect.SetActive(false);
                glow.SetActive(false);
            }
        }
    }

    public void CaseOpened()
    {
        caseOpened = true;
    }

    public void GatchaReward()
    {
        GameObject Reward = Instantiate(RewardPrefab, keyCapSpawn.position, Quaternion.identity) as GameObject;
        rewardAnim = Reward.GetComponent<Animator>();
    }

    public void SpawnEffect()
    {
        effect.SetActive(true);
        effectE.SetVector3("RarityColourValues", GachaOpenning.Instance.o_rarityColorV3);
    }

    public void GlowEffect()
    {
        canvas.SetActive(true);
        nameText.text = GachaOpenning.Instance.o_name;
        rarityText.text = GachaOpenning.Instance.o_rarity;
        glow.SetActive(true);
        glowE.SetVector3("RarityColourValues", GachaOpenning.Instance.o_rarityColorV3);
    }
}