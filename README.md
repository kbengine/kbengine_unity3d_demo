KBEngine_unity3d_demo
=============

##This client-project is written for kbengine(a MMOG engine of server)
http://www.kbengine.org


##Releases

	sources		: https://github.com/kbengine/kbengine_unity3d_demo/releases/latest
	binarys		: https://sourceforge.net/projects/kbengine/files/


##Start:
		1: Use git to get the client-plugin(https://github.com/kbengine/kbengine_unity3d_plugins) and server-assets:

			In the kbengine_unity3d_demo directory:

			* Git command: git submodule update --init --remote
![submodule_update1](http://www.kbengine.org/assets/img/screenshots/gitbash_submodule.png)

			* Or use TortoiseGit(menu): TortoiseGit -> Submodule Update:
![submodule_update2](http://www.kbengine.org/assets/img/screenshots/unity3d_plugins_submodule_update.jpg)

			*Or manually get the client-plugin and server-assets
				Download client-plugin:
					https://github.com/kbengine/kbengine_unity3d_plugins/archive/master.zip
					unzip and copy to Assets/plugins/kbengine/kbengine_unity3d_plugins
				Download server-assets:
					https://github.com/kbengine/kbengine_demos_assets/archive/master.zip
					unzip and copy to kbengine/

		2: Build:
			Unity Editor -> File -> Build Settings -> PC, MAC & Linux Standalone.


##Configure demo:

	Change the login address:
![demo_configure](http://www.kbengine.org/assets/img/screenshots/demo_configure.jpg)

		kbengine_unity3d_demo\Scripts\kbe_scripts\clientapp.cs -> ip
		kbengine_unity3d_demo\Scripts\kbe_scripts\clientapp.cs -> port


##Start the servers:

	Build(KBEngine):
		http://www.kbengine.org/docs/build.html

	Installation(KBEngine):
		http://www.kbengine.org/docs/installation.html

	Copy "kbengine_unity3d_demo\kbengine_demos_assets" to KBEngine root directory:
		"kbengine\" is the engine root.

	Start server:
		kbengine\kbengine_demos_assets\start_server_fixed.bat
		(More: http://www.kbengine.org/docs/startup_shutdown.html)


##Start the client:

			Directly start(U3DEditor or Executable file).



##Navmesh-navigation(Optional):
	
	The server to use recastnavigation navigation.
		kbengine\demo\res\spaces\*

	Generation Navmeshs:
		https://github.com/kbengine/unity3d_nav_critterai


![screenshots1](http://www.kbengine.org/assets/img/screenshots/unity3d_demo9.jpg)
![screenshots2](http://www.kbengine.org/assets/img/screenshots/unity3d_demo10.jpg)
![screenshots3](http://www.kbengine.org/assets/img/screenshots/unity3d_demo11.jpg)
