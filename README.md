KBEngine_unity3d_demo
=============

##This client-project is written for kbengine(a MMOG engine of server)
http://www.kbengine.org


##Releases

	binarys		: https://sourceforge.net/projects/kbengine/files/


##Build:
		1: Update submodule(https://github.com/kbengine/kbengine_unity3d_plugins):
			* git command: git submodule update --init

			* use TortoiseGit(menu): TortoiseGit -> Submodule Update:
![submodule_update](http://www.kbengine.org/assets/img/screenshots/unity3d_plugins_submodule_update.jpg)

		2: Build:
			Unity Editor -> File -> Build Settings -> PC, MAC & Linux Standalone.


##Start the servers:

	http://www.kbengine.org/docs/installation.html


##Start the client:

			Directly start.


##Installation:

	Change the login address:
		kbengine_unity3d_demo\Assets\Plugins\kbengine\clientapp.cs -> ip

##Navmesh-navigation:
	
	The server to use recastnavigation navigation.
		kbengine\demo\res\spaces\*

	Generation Navmeshs:
		https://github.com/kbengine/unity3d_nav_critterai


![screenshots1](http://www.kbengine.org/assets/img/screenshots/unity3d_demo9.jpg)
![screenshots2](http://www.kbengine.org/assets/img/screenshots/unity3d_demo10.jpg)
![screenshots3](http://www.kbengine.org/assets/img/screenshots/unity3d_demo11.jpg)
