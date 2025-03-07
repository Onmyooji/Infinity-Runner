using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ItemControl : MonoBehaviour
{
    RunnerControl runner;
    private bool isDroped;
    private void Start()
    {
        runner = FindObjectOfType<RunnerControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Drop());

            if (tag == "Diamond")
            {
                SoundsManager.instance.PlaySFX("DropItem", 0.5f);
                ScoreManager.Instance.AddScore(500);
            }
            if (tag == "Emerald")
            {
                SoundsManager.instance.PlaySFX("DropItem", 0.5f);
                ScoreManager.Instance.AddScore(100);
            }
            if (tag == "Heart")
            {
                SoundsManager.instance.PlaySFX("DropHeart", 0.5f);
                runner.Health += 20;
                if (runner.Health > 100)
                {
                    runner.Health = 100;
                }
                runner.healthBarGreen.rectTransform.sizeDelta = new Vector2(runner.healthBarGreen.rectTransform.sizeDelta.x + 40, runner.healthBarGreen.rectTransform.sizeDelta.y);
                if (runner.healthBarGreen.rectTransform.sizeDelta.x > 200)
                {
                    runner.healthBarGreen.rectTransform.sizeDelta = new Vector2(200, runner.healthBarGreen.rectTransform.sizeDelta.y);
                }
            }
        }
    }

    void Update()
    {
        if (isDroped)
        {
            transform.Rotate(0, 0, 10 + 300 * Time.deltaTime);
            transform.position = new Vector3(runner.transform.position.x, transform.position.y + 10f * Time.deltaTime, runner.transform.position.z);
        }
        else
        {
            transform.Rotate(0, 0, 50 * Time.deltaTime);
        }
    }
    IEnumerator Drop()
    {
        isDroped = true;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
