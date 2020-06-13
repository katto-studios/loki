using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPDisplay : MonoBehaviour
{
    public SpriteRenderer box;
    public SpriteRenderer cap;

    public void Init(ColourPack cp)
    {
        Color nc = cp.color;
        nc.a = 1f;
        cap.color = nc;

        if (cp.additionalColours.Count > 0)
        {
            Color ncb = cp.additionalColours[0].color;
            ncb.a = 1f;
            box.color = ncb;
        }
    }
}
