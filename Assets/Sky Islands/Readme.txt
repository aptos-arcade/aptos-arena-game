Sky Islands Parallaxing 2d Background
By Jess Triska of Trideka Designs


Firstly, thanks for purchasing this asset! 

Included in this package are as follows:

Example Scene Folder
	A dead simple example of how to use the asset.
	Included is a very simple parallax scrolling script,
	created by Unity Tech for their 2D Character tutorial.

	It should be noted that this script does not handle infinite tiling,
	nor does it deal with vertical parallax very well (or at all.) It may
	be in your interest to write your own, or find another on the Asset Store!
	
	The component can be found within the Camera
	gameobject. Access it to change scrolling speed.

	Playing the scene enables a scripted animation on the camera
	to simply move it from left to right, to show off
	the parallaxing effect. 
	

Art Folder
	Contains all layers of the background art, which is split into individual sprites except for the
	deepest background layer (Layer 0) which is a tiling cloud background.

	Art\Animations Folder
		Contains Animation data for the waterfalls
		If you'd like to speed up or slow down (or even just stop) the speed of animation, 
		be sure to check the Animator's of these Waterfall animations!
	
Sprite Prefabs Folder
	Unlike my other backgrounds, this background is componentized into separate sprites. 
	This is to allow for a high degree of customization, and will allow for games like
	infinite runners to procedurally place the islands. 

	Sorting layers were pre-configured. A sorting layer named "Parallax-Background" in the example scene
	keeps all background sprites together. Each Parallax layer within uses its own "Order In Layer" 

	Parallax Layer				Order In Layer Number
	-------------------------------------------------
	Deep Background				0
	Background Islands			1
	Background Clouds			2
	Midground Islands			3
	Midground Clouds			4
	Foreground Islands			5
	*Foreground Clouds			7

	Foreground Clouds jump to Layer number 7 because several foreground islands have waterfalls which use their own layer - Layer number 6.


"Sky Islands Parallaxing Background.prefab"
	A simple prefab with all the image layers
	grouped together and ready to go.

	Simply drag it into your scene view.
		
Contact / Support

You can reach me via email at jtriska@gmail.com, or via twitter @Tridekaphobia. 

See https://www.facebook.com/trideka.Designs for more assets and other projects!