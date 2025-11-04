using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetShopButtonFunctionality : MonoBehaviour
{
    [SerializeField] TMP_Text hatButtonText;
    [SerializeField] Button nextHatButton;
    [SerializeField] Button previousHatButton;
    [SerializeField] TMP_Text dressButtonText;
    [SerializeField] Button nextDressButton;
    [SerializeField] Button previousDressButton;
    [SerializeField] TMP_Text hairButtonText;
    [SerializeField] Button nextHairButton;
    [SerializeField] Button previousHairButton;
    [SerializeField] TMP_Text broomButtonText;
    [SerializeField] Button nextBroomButton;
    [SerializeField] Button previousBroomButton;

    SkinManager skinManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetAllButtons();
    }

    void SetAllButtons()
    {
        skinManager = FindFirstObjectByType<SkinManager>();
        nextHatButton.onClick.AddListener(skinManager.NextHatSkin);
        nextDressButton.onClick.AddListener(skinManager.NextClothesSkin);
        nextHairButton.onClick.AddListener(skinManager.NextHairSkin);
        nextBroomButton.onClick.AddListener(skinManager.NextBroomSkin);

        previousHatButton.onClick.AddListener(skinManager.PreviousHatSkin);
        previousDressButton.onClick.AddListener(skinManager.PreviousClothesSkin);
        previousHairButton.onClick.AddListener(skinManager.PreviousHairSkin);
        previousBroomButton.onClick.AddListener(skinManager.PreviousBroomSkin);

        skinManager.nameTexts[SkinManager.SlotType.Hat] = hatButtonText;
        skinManager.nameTexts[SkinManager.SlotType.Hair] = hairButtonText;
        skinManager.nameTexts[SkinManager.SlotType.Clothes] = dressButtonText;
        skinManager.nameTexts[SkinManager.SlotType.Broom] = broomButtonText;

        skinManager.UpdateAllUIText();
    }
}
