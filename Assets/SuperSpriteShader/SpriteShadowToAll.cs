using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadowToAll : MonoBehaviour {
    public bool receiveShadows = true;
    public UnityEngine.Rendering.ShadowCastingMode castShadows = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
    bool initialized = false;

    void Start () {
		
	}
	
    [ExecuteInEditMode]
	void Update () {
		if(!initialized)
        {
            initialized = true;
            Renderer[] rends = GameObject.FindObjectsOfType<SpriteRenderer>();
            for (int i = 0; i < rends.Length; i++)
                addShadows(rends[i]);
        }
	}

    void addShadows(Renderer rend)
    {
        if (rend != null)
        {
            rend.receiveShadows = receiveShadows;
            rend.shadowCastingMode = castShadows;
        }
    }
}
