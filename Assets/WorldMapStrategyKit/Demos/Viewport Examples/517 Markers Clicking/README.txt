This example shows how to react to clicks over markers.

A marker in viewport mode is added to the main map, not to the viewport, hence it's part of the viewport texture and cannot be clicked as usual.
The demo script included uses the method GetMarker(map position) which looks for a marker that contains that position and returns it if successfull.

Using markers instead of GameObjectAnimator objects is much faster and recommended for static graphical elements, like city, buildings or resource tokens.


