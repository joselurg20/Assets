using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonManager : MonoBehaviour
{
    public string ItemName;
    public string ItemDescription;
    public Sprite ItemImage;
    public GameObject Item3DModel;
    private ARInteractionManeger interactionsManager;





    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(ItemName);
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ItemName;
        transform.GetChild(2).GetComponent<RawImage>().texture = ItemImage.texture;
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ItemDescription;

        var button = GetComponent<Button>();
        button.onClick.AddListener(GameManager.instance.ARPosition);
        button.onClick.AddListener(Create3DModel);

        interactionsManager = FindObjectOfType<ARInteractionManeger > ();


    }

    private void Create3DModel()
    {
       interactionsManager.Item3DModel = Instantiate(Item3DModel);
    }

  
}
