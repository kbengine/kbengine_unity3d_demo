KBEngine_unity3d_demo
=============

##This client-project is written for KBEngine(a MMOG engine of server)
http://www.kbengine.org

##中文
[Demo搭建文档](https://github.com/kbengine/kbengine_unity3d_demo/blob/master/README_CN.md)，QQ交流群：16535321，[论坛](http://bbs.kbengine.org)

##Releases

	sources		: https://github.com/kbengine/kbengine_unity3d_demo/releases/latest
	binarys		: https://sourceforge.net/projects/kbengine/files/


##KBE-Plugins docs

	https://github.com/kbengine/kbengine_unity3d_plugins/blob/master/README.md


##Start:
	1. Download KBEngine(a MMOG engine of server):
		Download(KBEngine):
			https://github.com/kbengine/kbengine/releases/latest

		Build(KBEngine):
			http://www.kbengine.org/docs/build.html

		Installation(KBEngine):
			http://www.kbengine.org/docs/installation.html

	2. Use git to get the plugin(client) and demo-assets(server):

		In the kbengine_unity3d_demo directory:

		* Git command: git submodule update --init --remote
![submodule_update1](http://www.kbengine.org/assets/img/screenshots/gitbash_submodule.png)

		* Or use TortoiseGit(menu): TortoiseGit -> Submodule Update:
![submodule_update2](http://www.kbengine.org/assets/img/screenshots/unity3d_plugins_submodule_update.jpg)

		* Or manually get the plugin(client) and demo-assets(server)

			Download plugin(client):
				https://github.com/kbengine/kbengine_unity3d_plugins/releases/latest
				unzip and copy to "Assets/plugins/kbengine/kbengine_unity3d_plugins"

			Download demo-assets(server):
				https://github.com/kbengine/kbengine_demos_assets/releases/latest
				unzip and copy to "kbengine/"  (The root directory server engine, such as $KBE_ROOT)

	3. Copy "kbengine_unity3d_demo\kbengine_demos_assets" to "kbengine\" root directory
![demo_configure](http://www.kbengine.org/assets/img/screenshots/demo_copy_kbengine.jpg)


##Configure Demo(Optional):

	Change the login address:
![demo_configure](http://www.kbengine.org/assets/img/screenshots/demo_configure.jpg)

		kbengine_unity3d_demo\Scripts\kbe_scripts\clientapp.cs -> ip
		kbengine_unity3d_demo\Scripts\kbe_scripts\clientapp.cs -> port


##Start the Servers:

	Ensure that the "kbengine_unity3d_demo\kbengine_demos_assets" has been copied to the "kbengine\" directory
		Reference：Start

	Check the startup status:
		If successful will find log "Components::process(): Found all the components!".
		Otherwise, please search the "ERROR" keyword in logs, according to the error description to try to solve.
		(More: http://www.kbengine.org/docs/startup_shutdown.html)

	Start server:
		Windows:
			kbengine\kbengine_demos_assets\start_server.bat

		Linux:
			kbengine\kbengine_demos_assets\start_server.sh

		(More: http://www.kbengine.org/docs/startup_shutdown.html)


##Start the Client:

	Directly start(U3DEditor or Executable file).
	(Build Client：Unity Editor -> File -> Build Settings -> PC, MAC & Linux Standalone.)


##Navmesh-navigation(Optional):
	
	The server to use recastnavigation navigation.
	Recastnavigation generated navigation mesh (Navmeshs) placed on the:
		kbengine\kbengine_demos_assets\res\spaces\*

	Generation Navmeshs:
		https://github.com/kbengine/unity3d_nav_critterai


##Screenshots:
![screenshots1](http://www.kbengine.org/assets/img/screenshots/unity3d_demo9.jpg)
![screenshots2](http://www.kbengine.org/assets/img/screenshots/unity3d_demo10.jpg)
![screenshots3](http://www.kbengine.org/assets/img/screenshots/unity3d_demo11.jpg)
