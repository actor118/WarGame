using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace WorldMapStrategyKit {

    public class DemoTerraforming : MonoBehaviour {


        WMSK map;

        void Start() {
            map = WMSK.instance;
        }

        void Update() {

            if (!map.input.GetMouseButton(0)) {
                map.allowUserDrag = true;
                return;
            }

            Vector2 mapPosition = map.cursorLocation;

            if (map.input.GetKey(KeyCode.LeftShift)) {
                map.allowUserDrag = false;
                map.EarthRaiseElevation(mapPosition, 0.01f, 0.05f);
            }

            if (map.input.GetKey(KeyCode.LeftControl)) {
                map.allowUserDrag = false;
                map.EarthLowerElevation(mapPosition, 0.01f, 0.05f);
            }

        }
    }

}

