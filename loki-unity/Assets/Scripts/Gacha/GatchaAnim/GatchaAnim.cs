using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatchaAnim : MonoBehaviour
{
    public GameObject effect;

    private Animator anim;
    private Animator rewardAnim;
    private RaycastHit hit;
    private Ray ray;

    private bool caseOpened = false;

    public Transform keyCapSpawn;

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
            }
        }
    }

    public void CaseOpened()
    {
        caseOpened = true;
    }

    public void GatchaReward(GameObject RewardPrefab)
    {
        GameObject Reward = Instantiate(RewardPrefab, keyCapSpawn.position, Quaternion.identity) as GameObject;
        rewardAnim = Reward.GetComponent<Animator>();
    }

    public void SpawnEffect()
    {
        effect.SetActive(true);
    }
}