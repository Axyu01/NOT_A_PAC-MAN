using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTrigger : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.RegisterPoint();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag == "pacman")
        {
            GameManager.Instance.ScorePoint();
            Destroy(gameObject);
        }
    }
}
