KBEngine_unity3d_demo
=============

##This client-project is written for kbengine(a MMOG engine of server)
http://www.kbengine.org


##Releases

	binarys		: https://sourceforge.net/projects/kbengine/files/


##Build:
		1: Use git to update the KBE plugin(https://github.com/kbengine/kbengine_unity3d_plugins) and server-assets:

			In the kbengine_unity3d_demo directory:

			* Git command: git submodule update --init --remote
![submodule_update1](http://www.kbengine.org/assets/img/screenshots/gitbash_submodule.png)

			* Or use TortoiseGit(menu): TortoiseGit -> Submodule Update:
![submodule_update2](http://www.kbengine.org/assets/img/screenshots/unity3d_plugins_submodule_update.jpg)

			*Or manually get the client-plugin and server-assets
				download client-plugin:
					https://github.com/kbengine/kbengine_unity3d_plugins/archive/master.zip
					unzip and copy to Assets/plugins/kbengine/kbengine_unity3d_plugins
				download server-assets:
					https://github.com/kbengine/kbengine_demos_assets/archive/master.zip
					unzip and copy to kbengine/

		2: Build:
			Unity Editor -> File -> Build Settings -> PC, MAC & Linux Standalone.


##Configure demo:

	Change the login address:
![demo_configure](http://www.kbengine.org/assets/img/screenshots/demo_configure.jpg)

		kbengine_unity3d_demo\Scripts\kbe_scripts\clientapp.cs -> ip
		kbengine_unity3d_demo\Scripts\kbe_scripts\clientapp.cs -> port


##Navmesh-navigation(Optional):
	
	The server to use recastnavigation navigation.
		kbengine\demo\res\spaces\*

	Generation Navmeshs:
		https://github.com/kbengine/unity3d_nav_critterai


##Start the servers:

	Installation(KBEngine):
		http://www.kbengine.org/docs/installation.html

	copy kbengine_demos_assets to kbengine\

	Start server:
		kbengine\kbengine_demos_assets\start_server_fixed.bat
		(more: http://www.kbengine.org/docs/startup_shutdown.html)

	Kill server:
		kbengine\kbengine_demos_assets\kill_server.bat


##Start the client:

			Directly start(U3DEditor or Executable file).



![screenshots1](http://www.kbengine.org/assets/img/screenshots/unity3d_demo9.jpg)
![screenshots2](http://www.kbengine.org/assets/img/screenshots/unity3d_demo10.jpg)
![screenshots3](http://www.kbengine.org/assets/img/screenshots/unity3d_demo11.jpg)
