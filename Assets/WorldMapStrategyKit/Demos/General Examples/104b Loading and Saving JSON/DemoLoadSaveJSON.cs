using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WorldMapStrategyKit {
    public class DemoLoadSaveJSON : MonoBehaviour {

        /// <summary>
        /// Here we'll store the data - you can save these strings to file or database (ie. System.IO.File.WriteAllText(path, countryGeoData)
        /// </summary>
        string countriesData, provincesData, citiesData, mountPointsData;
        int state;
        GUIStyle buttonStyle;
        WMSK map;

        void Start() {
            map = WMSK.instance;

            // setup GUI resizer - only for the demo
            GUIResizer.Init(800, 500);

            // setup GUI styles - only for the demo
            buttonStyle = new GUIStyle();
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            buttonStyle.normal.background = Texture2D.whiteTexture;
            buttonStyle.normal.textColor = Color.black;

#if UNITY_EDITOR
            EditorUtility.DisplayDialog("Load/Save Demo", "In this demo scene, a map change is simulated (North America is collapsed into one single country), then saved and loaded using jSON format.\n\nTo start, press the top/left button on the screen.", "Ok");
#endif

        }

        void OnGUI() {

            // Do autoresizing of GUI layer
            GUIResizer.AutoResize();

            switch (state) {
                case 0:
                    if (GUI.Button(new Rect(10, 10, 160, 30), "Merge North America", buttonStyle)) {
                        int countryUSA = map.GetCountryIndex("United States of America");
                        int countryCanada = map.GetCountryIndex("Canada");
                        map.CountryTransferCountry(countryUSA, countryCanada, true);

                        countryUSA = map.GetCountryIndex("United States of America");
                        int countryMexico = map.GetCountryIndex("Mexico");
                        map.CountryTransferCountry(countryUSA, countryMexico, true);
                        state++;
                    }
                    break;
                case 1:
                    if (GUI.Button(new Rect(10, 10, 160, 30), "Save Current Frontiers", buttonStyle)) {
                        SaveAllData();
                        state++;
#if UNITY_EDITOR
                        EditorUtility.DisplayDialog("Saved Data", "Data stored in temporary variables. You can now click reset frontiers to simulate a game restart and then Load Saved Data to restore the saved data.", "Ok");
#endif
                    }
                    break;
                case 2:
                    if (GUI.Button(new Rect(10, 10, 160, 30), "Reset Frontiers", buttonStyle)) {
                        map.ReloadData();
                        map.Redraw();
                        state++;
                    }
                    break;
                case 3:
                    if (GUI.Button(new Rect(10, 10, 160, 30), "Load Saved Data", buttonStyle)) {
                        LoadAllData();
                        state++;
#if UNITY_EDITOR
                        EditorUtility.DisplayDialog("Data Loaded!", "Data has been loaded from the temporary variables.", "Ok");
#endif
                    }
                    break;
                case 4:
                    if (GUI.Button(new Rect(10, 50, 160, 30), "Reset Frontiers", buttonStyle)) {
                        map.ReloadData();
                        map.Redraw();
                        state = 0;
                    }
                    break;
            }

        }

        void SaveAllData() {
            // Store current countries information and frontiers data in string variables
            countriesData = map.GetCountriesDataJSON();

            // Same for provinces. This wouldn't be neccesary if you are not using provinces in your app.
            provincesData = map.GetProvincesDataJSON();

            // Same for cities. This wouldn't be neccesary if you are not using cities in your app.
            citiesData = map.GetCitiesDataJSON();

            // Same for mount points. This wouldn't be neccesary if you are not using mount points in your app.
            mountPointsData = map.GetMountPointsDataJSON();
        }

        void LoadAllData() {
            // Load country information from a string.
            map.SetCountriesDataJSON(countriesData);

            // Same for provinces. This wouldn't be neccesary if you are not using provinces in your app.
            map.SetProvincesDataJSON(provincesData);

            // Same for cities. This wouldn't be neccesary if you are not using cities in your app.
            map.SetCitiesDataJSON(citiesData);

            // Same for mount points. This wouldn't be neccesary if you are not using mount points in your app.
            map.SetMountPointsDataJSON(mountPointsData);

            // Redraw everything
            map.Redraw();
        }

    }

}

