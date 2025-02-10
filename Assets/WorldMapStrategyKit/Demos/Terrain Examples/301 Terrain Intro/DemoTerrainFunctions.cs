using UnityEngine;
using System.Collections;

namespace WorldMapStrategyKit {

    public class DemoTerrainFunctions : MonoBehaviour {

        WMSK map;
        public FreeCameraMove freeCameraScript;
        public TiltedCameraMove tiltedCameraScript;

        void Start() {
            map = WMSK.instance;
        }

        public void FlyToAustralia() {
            FlyToCountry(map.GetCountryIndex("Australia"));
        }

        void FlyToCountry(int countryIndex) {
            // Get zoom level for the extents of the country
            float zoomLevel = map.GetCountryRegionZoomExtents(countryIndex);
            map.FlyToCountry(countryIndex, 2.0f, zoomLevel);
            map.BlinkCountry(countryIndex, Color.green, Color.black, 3.0f, 0.2f);
        }

        public void FlyToMadrid() {
            map.FlyToCity("Madrid", "Spain", 2.0f, 0.05f);
        }

        public void ResetCamera() {
            RepositionCamera(Vector3.down);
            freeCameraScript.enabled = false;
            tiltedCameraScript.enabled = false;
            map.enableFreeCamera = false;
        }

        public void EnableFreeCamera() {
            RepositionCamera(Camera.main.transform.forward);
            freeCameraScript.enabled = true;
            tiltedCameraScript.enabled = false;
            map.enableFreeCamera = true;
        }

        public void EnableTiltedCamera() {
            RepositionCamera(new Vector3(0, -1, 1));
            freeCameraScript.enabled = false;
            tiltedCameraScript.enabled = true;
            map.enableFreeCamera = true;
        }

        void RepositionCamera(Vector3 forward) {
            Vector3 cursorLocation;
            if (!map.GetCurrentMapLocation(out cursorLocation, true)) {
                cursorLocation = map.Map2DToWorldPosition(Vector2.zero);
            }
            Camera.main.transform.position = cursorLocation - forward * map.GetRenderViewportDistanceToCamera(Camera.main);
            Camera.main.transform.forward = forward;
        }


        public void ColorizeEurope() {
            for (int countryIndex = 0; countryIndex < map.countries.Length; countryIndex++) {
                if (map.countries[countryIndex].continent.Equals("Europe")) {
                    Color color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                    map.ToggleCountrySurface(countryIndex, true, color);
                }
            }

        }

    }
}