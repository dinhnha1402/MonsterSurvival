using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumberController : MonoBehaviour
{
    public static DamageNumberController instance;

    public Transform damageCanvas;

    public GameObject damageText;
    private List<GameObject> damageTextPool = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnDamage(float damageAmount, Vector3 location)
    {

        int roundedDamage = Mathf.RoundToInt(damageAmount);

        //GameObject newDamageText = Instantiate(damageText, location, Quaternion.identity, damageCanvas);

        GameObject newDamageText = GetFromPool();



        newDamageText.GetComponent<TMP_Text>().text = roundedDamage.ToString();

        newDamageText.transform.position = location;

        newDamageText.SetActive(true);

        
    }

    public GameObject GetFromPool()
    {
        GameObject damageTextToOutPut = null;

        if (damageTextPool.Count == 0)
        {
            damageTextToOutPut = Instantiate(damageText, damageCanvas);
        } else
        {
            damageTextToOutPut = damageTextPool[0];

            damageTextPool.RemoveAt(0);
        }

        return damageTextToOutPut;
    }

    public void PlaceInPool(GameObject damageTextToPlace)
    {
        damageTextToPlace.SetActive(false);

        damageTextPool.Add(damageTextToPlace);
    }
}
