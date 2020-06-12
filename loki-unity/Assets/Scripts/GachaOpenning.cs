using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaOpenning : Singleton<GachaOpenning>
{
    public string o_name;
    public string o_rarity;
    public Color o_rarityColor;
    public Vector3 o_rarityColorV3;

    public GameObject o_prefab;

    public int type = -1;

    ColourPack colourPack;
    ArtisanKeycap artisanKeycap;

    public void Init(Object obj)
    {
        if(obj.GetType().Equals(typeof(ArtisanKeycap)))
        {
            type = 0;
        }
        else if (obj.GetType().Equals(typeof(ColourPack)))
        {
            colourPack = (ColourPack)obj;
            type = 1;
            ColourPack cp = (ColourPack)obj;
            switch (cp.colourPackRarity)
            {
                case ColourPackRarity.COMMON:
                    o_rarityColor = new Color(0.8f, 0.8f, 0.8f);
                    break;
                case ColourPackRarity.RARE:
                    o_rarityColor = new Color(0.2f, 0.6f, 0.8f);
                    break;
                case ColourPackRarity.EPIC:
                    o_rarityColor = new Color(0.7f, 0.2f, 0.7f);
                    break;
                case ColourPackRarity.LEGENDARY:
                    o_rarityColor = new Color(0.8f, 0.6f, 0.2f);
                    break;
                case ColourPackRarity.UNIQUE:
                    o_rarityColor = new Color(0.8f, 0.3f, 0.3f);
                    break;
            }
            o_name = cp.name;
            o_rarity = cp.colourPackRarity.ToString();
            o_rarityColorV3 = new Vector3(o_rarityColor.r, o_rarityColor.g, o_rarityColor.b);
            o_prefab = Resources.Load<GameObject>("Colourpack Display");
        }
    }

    public void GachaReward(GameObject reward)
    {
        GameObject newReward = Instantiate(o_prefab, reward.transform);
        if(type == 1)
        {
            newReward.GetComponent<CPDisplay>().Init(colourPack);
            newReward.transform.localPosition = new Vector3(0, 0.1f, 0);
            newReward.transform.localEulerAngles = new Vector3(45, 180, 0);
            newReward.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
