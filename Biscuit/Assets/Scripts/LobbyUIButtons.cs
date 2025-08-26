using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyUIButtons : MonoBehaviour
{
    public GameObject shopCanvas, drugCanvas, fightCanvas;
    public int shopIndex = 0;
    public GameObject index0, index1, index2, index3, index4;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            shopCanvas.SetActive(false);
            drugCanvas.SetActive(false);
            fightCanvas.SetActive(false);
        }
        if (GameObject.Find("ShopOverlay") != null)
        {
            switch (shopIndex)
            {
                case 0:
                    index4.SetActive(false);
                    index0.SetActive(true);
                    break;
                case 1:
                    index0.SetActive(false);
                    index1.SetActive(false);
                    break;
                case 2:
                    index1.SetActive(false);
                    index2.SetActive(true);
                    break;
                case 3:
                    index2.SetActive(false);
                    index3.SetActive(true);
                    break;
                case 4:
                    index3.SetActive(false);
                    index4.SetActive(true);
                    break;
                default:
                    shopIndex = 0; //just in case something goes wrong ig
                    break;
            }
        }
    }

    public void ShopArrow()
    {
        shopIndex++;
        if (shopIndex > 4) shopIndex = 0;
    }
}
