using System;
using System.Collections;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private int gas = 100;
    [SerializeField] private float moveSpeed = 1f;
    
    public int Gas => gas;

    private void Start()
    {
        StartCoroutine(GasCoroutine());
    }

    private IEnumerator GasCoroutine()
    {
        while (true)
        {
            gas -= 10;
            if (gas <= 0) break;
            yield return new WaitForSeconds(1f);
        }
        
        // 게임 종료
        GameManager.Instance.EndGame();
    }

    public void Move(float direction)
    {
        transform.Translate(Vector3.right * (direction * Time.deltaTime));
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1.75f, 1.75f), transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gas"))
        {
            gas += 30;
            other.gameObject.SetActive(false);
        }
    }
}