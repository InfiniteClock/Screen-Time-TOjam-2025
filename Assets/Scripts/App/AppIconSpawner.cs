using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppIconSpawner : MonoBehaviour
{
    public GameObject iconPrefab;
    public Transform iconHolder;

    Dictionary<App.ID, GameObject> icons = new();

    private void Start()
    {
        foreach (App app in App.All.Values)
        {
            GameObject icon = Instantiate(iconPrefab, iconHolder);

            Button button = icon.GetComponentInChildren<Button>();
            button.onClick.AddListener(app.Open); // Tell button to open app

            // Set name and icon
            icon.GetComponentInChildren<TMPro.TMP_Text>().text = app.homeScreenName;
            icon.GetComponentInChildren<Image>().sprite = app.icon;

            icons.Add(app.id, icon);
        }
    }

    private void Update()
    {
        foreach (App app in App.All.Values)
        {
            icons[app.id].gameObject.SetActive(app.ShowButtonOnHomeScreen);
        }
    }
}
