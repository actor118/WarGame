using System;
using UnityEngine;

namespace WorldMapStrategyKit {

    public class DemoMarkersClicking : MonoBehaviour {

        WMSK map;

        void Start() {

            // Get a reference to the World Map API:
            map = WMSK.instance;

            // Add a sphere marker over each city
            foreach (City city in map.cities) {

                // setup sphere
                GameObject o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                o.transform.localScale = Vector3.one * 0.25f;
                o.name = city.name;
                DestroyImmediate(o.GetComponent<Collider>());

                // Add sphere as marker to the map
                MarkerClickHandler handler = map.AddMarker3DObject(o, city.unity2DLocation, enableEvents: true);
                handler.allowDrag = true;
            }

            map.OnMarkerMouseDown += Map_OnMarkerMouseDown;
            map.OnMarkerMouseUp += Map_OnMarkerMouseUp;
            map.OnMarkerDragStart += Map_OnMarkerDragStart;
            map.OnMarkerDragEnd += Map_OnMarkerDragEnd;

            // these two can be expensive if there're many markers, use only if required!
            //map.OnMarkerMouseEnter += Map_OnMarkerMouseEnter;
            //map.OnMarkerMouseExit += Map_OnMarkerMouseExit;
        }


        private void Map_OnMarkerMouseEnter(MarkerClickHandler marker) {
            marker.markerRenderer.material.color = Color.yellow;
        }

        private void Map_OnMarkerMouseExit(MarkerClickHandler marker) {
            marker.markerRenderer.material.color = Color.white;
        }

        private void Map_OnMarkerMouseDown(MarkerClickHandler marker, int buttonIndex) {
            Debug.Log("Mouse pressed on " + marker.name);
        }

        private void Map_OnMarkerMouseUp(MarkerClickHandler marker, int buttonIndex) {
            Debug.Log("Mouse released on " + marker.name);
        }

        private void Map_OnMarkerDragStart(MarkerClickHandler marker) {
            Debug.Log("Drag start on " + marker.name);
        }

        private void Map_OnMarkerDragEnd(MarkerClickHandler marker) {
            Debug.Log("Drag end on " + marker.name);
        }

        void Update() {
            if (map.input.GetKeyDown(KeyCode.Escape)) {
                if (map.CancelMarkerDrag(true)) {
                    Debug.Log("Drag cancelled!");
                }
            }
        }


    }

}
