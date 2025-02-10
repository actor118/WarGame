using UnityEngine;
using UnityEngine.UI;

namespace WorldMapStrategyKit {
    public class UIPanelDemo2D : MonoBehaviour {

        public RectTransform panel;
        public Text countryName;
        public Text provinceName;
        public Text cityName;
        public Text population;

        public float panelScaling = 1f;

        WMSK map;
        GUIStyle labelStyle;

        void Start() {
            // Get a reference to the World Map API:
            map = WMSK.instance;

            // UI Setup - non-important, only for this demo
            labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleLeft;
            labelStyle.normal.textColor = Color.white;

            // setup GUI resizer - only for the demo
            GUIResizer.Init(800, 500);

            /* Register events: this is optionally but allows your scripts to be informed instantly as the mouse enters or exits a country, province or city */
            map.OnCityEnter += OnCityEnter;
            map.OnCityExit += OnCityExit;

            HidePanel();
        }

        void OnGUI() {
            GUI.Label(new Rect(10, 10, 500, 30), "Move mouse over a city to show data.", labelStyle);
        }

        void OnCityEnter(int cityIndex) {
            City city = map.GetCity(cityIndex);
            ShowCityInfo(city);
        }

        void OnCityExit(int cityIndex) {
            HidePanel();
        }

        void ShowCityInfo(City city) {

            if (city == null)
                return;

            // Update scale if variable panelScaling has changed
            panel.localScale = Vector3.one * panelScaling;

            // Update text labels
            cityName.text = city.name;
            population.text = city.population.ToString();
            countryName.text = map.GetCityCountryName(city);
            provinceName.text = map.GetCityProvinceName(city);

            // Reposition UI Panel next to current cursor location on screen

            // We need the screen coordinates of the current map cursor location. We can simply use Input.mousePosition but we'll obtain it transforming
            // the cursorLocation into screen coordinates in case you need to do this on other locations:
            Vector3 worldPos = map.transform.TransformPoint(map.cursorLocation);
            Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            // Now, since our Panel is anchored on left/bottom and screen coordinates have 0,0 on left/bottom as well,
            // We check if the panel will be cropped to the right and if so we subtract it's width from the anchor position
            if (screenPos.x + panel.rect.width * panelScaling > Screen.width) {
                screenPos.x -= panel.rect.width * panelScaling;
            }
            // We check if it will be cropped at the top of the screen and it so we subtract the panel height from the anchor position
            if (screenPos.y + panel.rect.height * panelScaling > Screen.height) {
                screenPos.y -= panel.rect.height * panelScaling;
            }
            // This way we keep the window within the screen
            // we simply pass the screenPos to the anchoredPosition property
            panel.anchoredPosition = screenPos;
        }

        void HidePanel() {
            // Move panel out of screen
            // use the panel size directly
            panel.anchoredPosition = new Vector3(-panel.rect.width * panelScaling, -panel.rect.height * panelScaling);
        }


    }

}

