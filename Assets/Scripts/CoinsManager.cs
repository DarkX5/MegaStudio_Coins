// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour, ICoins
{
    public int coins { get; set; }

    public void Initialize() {
        coins = 5;
        Debug.Log("Initialize coins");
    }
    public void GetFreeCoins() {
        Debug.Log("Get free coins");
    }
    public void GetExtraCoins() {
        Debug.Log("Get Extra Coins");
    }
    public void Gamble() {
        Debug.Log("Gamble");
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
