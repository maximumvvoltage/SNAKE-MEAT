using UnityEngine;

public class GeneralThings : MonoBehaviour
{
    public GameObject stampcardButton;
    public GameObject youGotThing;
    public bool stampcardAttained = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    }

    public void EnterStore()
    {
        youGotThing.SetActive(true);
    }
}
