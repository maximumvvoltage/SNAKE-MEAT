using UnityEngine;

public class GeneralThings : MonoBehaviour
{
    public GameObject stampcardButton;
    private ErrandManager errandManager;
    public GameObject youGotThing;
    public bool stampcardAttained;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stampcardAttained = false;
        youGotThing.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cool()
    {
        stampcardButton.SetActive(true);
        youGotThing.SetActive(false);
        stampcardAttained = true;
        Debug.Log("attained!");
    }

    public void YouGotThing() 
    {
        youGotThing.SetActive(true);
        // this can't be in EnterTrigger with the other ticket shop stuff because
        // i would have to assign youGotThing to EVERY SINGLE instance of the stampcard obtaining screen. 
    }
}
