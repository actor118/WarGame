using UnityEngine;

namespace WorldMapStrategyKit {

	public class SurfaceBlinker : MonoBehaviour {

		public float duration;
		public Color color1, color2;
		public float speed;
		public Material blinkMaterial;
		public Region customizableSurface;
		public bool smoothBlink;

		Material oldMaterial;
		float startTime, lapTime;
		bool whichColor;
		WMSK map;

		void Start () {
			if (!TryGetComponent(out Renderer renderer)) {
				DestroyImmediate(this);
				return;
			}
			oldMaterial = renderer.sharedMaterial;
			GenerateMaterial (renderer);
			map = WMSK.GetInstance (transform);
			startTime = map.time;
			lapTime = startTime - speed;
		}

		void OnDestroy() {
// Restores material
				Material goodMat;
				if (customizableSurface.customMaterial != null) {
					goodMat = customizableSurface.customMaterial;
				} else {
					goodMat = oldMaterial;
				}
				if (TryGetComponent(out Renderer renderer)) {
					renderer.sharedMaterial = goodMat;
				}
			if (blinkMaterial != null) {
				DestroyImmediate(blinkMaterial);
			}
				// Hide surface?
				if (customizableSurface.customMaterial == null) {
					gameObject.SetActive (false);
				}
		}

        
        // Update is called once per frame
        void Update () {
			float elapsed = map.time - startTime;
			if (elapsed > duration) {
				DestroyImmediate (this);
				return;
			}
			if (smoothBlink) {
				if (!TryGetComponent(out Renderer renderer)) {
					DestroyImmediate(this);
					return;
				}
				Material mat = renderer.sharedMaterial;
				if (mat != blinkMaterial)
					GenerateMaterial (renderer);

				float t = Mathf.PingPong (map.time * speed, 1f);
				blinkMaterial.color = Color.Lerp (color1, color2, t);

			} else if ( (map.time - lapTime) * speed > 1f) {
				lapTime = map.time;
				if (!TryGetComponent(out Renderer renderer)) {
					DestroyImmediate(this);
					return;
				}
				Material mat = renderer.sharedMaterial;
				if (mat != blinkMaterial)
					GenerateMaterial (renderer);
				whichColor = !whichColor;
				if (whichColor) {
					blinkMaterial.color = color1;
				} else {
					blinkMaterial.color = color2;
				}
			}
		}

		void GenerateMaterial (Renderer renderer) {
			blinkMaterial = Instantiate (blinkMaterial);
			renderer.sharedMaterial = blinkMaterial;
		}
	}

}