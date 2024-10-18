using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportItemScript : ThrowItemScript
{
    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag(ValueDefine.TERRAIN_TAG))
        {
            TeleportPlayer();
            DestroyItem();
        }
    }

    public void TeleportPlayer()
    {
        PlayManager.TeleportPlayer(transform.position);
    }

    private void DestroyItem()
    {
        Destroy(gameObject);
    }
}
