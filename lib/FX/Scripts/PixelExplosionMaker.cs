using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using nv;

public class PixelExplosionMaker : GameSingleton<PixelExplosionMaker> {

    public GameObject explosionPrefab;

    public GameObject clearListWhenDisabled;

    List<GameObject> activeObjects = new List<GameObject>();

    public void MakePlayerExplosion(Vector3 position)
    {
        GameObject ex = GameObject.Instantiate( explosionPrefab );
        ex.transform.position = position;
        activeObjects.Add( ex );
    }

    private void Update()
    {
        if(activeObjects.Count > 0 && clearListWhenDisabled.activeInHierarchy == false )
        {
            foreach( var v in activeObjects )
                Destroy( v );
            activeObjects.Clear();
        }
    }

}
