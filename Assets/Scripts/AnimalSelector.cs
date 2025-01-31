using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnimalSelector : MonoBehaviour
{
    public List<string> selectedAnimals = new List<string>();
    public int maxAnimals = 3;
    public GameObject confirmPanel;
    public Text selectedAnimalsText;
    // public Button confirmButton;

    // void Start()
    // {
    //     confirmButton.onClick.AddListener(OnConfirm);
    // }

    public void SelectAnimal(string animalName)
    {
        if (selectedAnimals.Count < maxAnimals)
        {
            selectedAnimals.Add(animalName);
            UpdateSelectedAnimalsText();
            if (selectedAnimals.Count == maxAnimals)
            {
                confirmPanel.SetActive(true);
            }
        }
    }

    void UpdateSelectedAnimalsText()
    {
        selectedAnimalsText.text = "Selected Animals: " + string.Join(", ", selectedAnimals);
        //confirmButton.interactable = (selectedAnimals.Count == maxAnimals);
    }

    public void OnConfirm()
    {
        PlayerPrefs.SetString("SelectedAnimals", string.Join(",", selectedAnimals));
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void OnCancel()
    {
        selectedAnimals.Clear();
        confirmPanel.SetActive(false);
        UpdateSelectedAnimalsText();
    }
}
