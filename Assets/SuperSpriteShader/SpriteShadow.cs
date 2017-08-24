using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour {
    public bool receiveShadows = true;
    public UnityEngine.Rendering.ShadowCastingMode castShadows = UnityEngine.Rendering.ShadowCastingMode.On;
    Renderer rend;
	// Use this for initialization
	void Start () {
        addShadows();
    }

#if UNITY_EDITOR
    // Update is called once per frame
    [ExecuteInEditMode]
	void Update () {
        addShadows();
    }
#endif

    void addShadows()
    {
        if(rend==null)
        {
            rend = GetComponent<Renderer>();
        }
        else
        {
            rend.receiveShadows = receiveShadows;
            rend.shadowCastingMode = castShadows;
        }
    }
}
