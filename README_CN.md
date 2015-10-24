KBEngine_unity3d_demo
=============

##本项目作为KBEngine服务端引擎的客户端演示而写
http://www.kbengine.org

##官方论坛

	http://bbs.kbengine.org


##QQ交流群

	16535321 


##Releases

	sources		: https://github.com/kbengine/kbengine_unity3d_demo/releases/latest
	binarys		: https://sourceforge.net/projects/kbengine/files/


##KBE插件文档

	https://github.com/kbengine/kbengine_unity3d_plugins/blob/master/README.md


##开始:
	1. 确保已经下载过KBEngine服务端引擎，如果没有下载请先下载
		下载服务端源码(KBEngine)：
			https://github.com/kbengine/kbengine/releases/latest

		编译(KBEngine)：
			http://www.kbengine.org/docs/build.html

		安装(KBEngine)：
			http://www.kbengine.org/docs/installation.html

	2. 下载kbengine客户端插件与服务端Demo资产库:

	    * 使用git命令行，进入到kbengine_unity3d_demo目录执行：

	        git submodule update --init --remote
![submodule_update1](http://www.kbengine.org/assets/img/screenshots/gitbash_submodule.png)

		* 或者使用 TortoiseGit(选择菜单): TortoiseGit -> Submodule Update:
![submodule_update2](http://www.kbengine.org/assets/img/screenshots/unity3d_plugins_submodule_update.jpg)

                * 也可以手动下载kbengine客户端插件与服务端Demo资产库

		        客户端插件下载：
		            https://github.com/kbengine/kbengine_unity3d_plugins/releases/latest
		            下载后请将其解压缩，插件源码请放置在: Assets/plugins/kbengine/kbengine_unity3d_plugins

		        服务端资产库下载：
		            https://github.com/kbengine/kbengine_demos_assets/releases/latest
		            下载后请将其解压缩,并将目录文件放置于服务端引擎根目录"kbengine/"之下，如下图：

	3. 拷贝服务端资产库"kbengine_demos_assets"到服务端引擎根目录"kbengine/"之下，如下图：
![demo_configure](http://www.kbengine.org/assets/img/screenshots/demo_copy_kbengine.jpg)


##配置Demo(可选):

	改变登录IP地址与端口（注意：关于服务端端口部分参看http://www.kbengine.org/cn/docs/installation.html）:
![demo_configure](http://www.kbengine.org/assets/img/screenshots/demo_configure.jpg)

		kbengine_unity3d_demo\Scripts\kbe_scripts\clientapp.cs -> ip
		kbengine_unity3d_demo\Scripts\kbe_scripts\clientapp.cs -> port


##启动服务器:

	确保“kbengine_unity3d_demo\kbengine_demos_assets”已经拷贝到KBEngine根目录：
		参考上方章节：开始

	使用启动脚本启动服务端：
		Windows:
			kbengine\kbengine_demos_assets\start_server.bat

		Linux:
			kbengine\kbengine_demos_assets\start_server.sh

	检查启动状态：
			如果启动成功将会在日志中找到"Components::process(): Found all the components!"。
			任何其他情况请在日志中搜索"ERROR"关键字，根据错误描述尝试解决。
			(更多参考: http://www.kbengine.org/docs/startup_shutdown.html)


##启动客户端:

	直接在Unity3D编辑器启动或者编译后启动
	（编译客户端：Unity Editor -> File -> Build Settings -> PC, MAC & Linux Standalone.）


##生成导航网格(可选):
	
	服务端使用Recastnavigation在3D世界寻路，recastnavigation生成的导航网格（Navmeshs）放置于：
		kbengine\kbengine_demos_assets\res\spaces\*

	在Unity3D中使用插件生成导航网格（Navmeshs）:
		https://github.com/kbengine/unity3d_nav_critterai


##演示截图:
![screenshots1](http://www.kbengine.org/assets/img/screenshots/unity3d_demo9.jpg)
![screenshots2](http://www.kbengine.org/assets/img/screenshots/unity3d_demo10.jpg)
![screenshots3](http://www.kbengine.org/assets/img/screenshots/unity3d_demo11.jpg)
