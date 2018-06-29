KBEngine_unity3d_demo
=============

## 本项目作为KBEngine服务端引擎的客户端演示而写

http://www.kbengine.org

## 官方论坛

	http://bbs.kbengine.org


## QQ交流群

	461368412


## Releases

	sources		: https://github.com/kbengine/kbengine_unity3d_demo/releases/latest


## KBE插件文档

	kbengine_unity3d_demo\Assets\Plugins\kbengine_unity3d_plugins\README.md


## 开始:
	1. 确保已经下载过KBEngine服务端引擎，如果没有下载请先下载
		下载服务端源码(KBEngine)：
			https://github.com/kbengine/kbengine/releases/latest

		编译(KBEngine)：
			http://kbengine.github.io/docs/build.html

		安装(KBEngine)：
			http://kbengine.github.io/docs/installation.html

	2. 下载服务端Demo资产库:

	    * 使用git命令行，进入到kbengine_unity3d_demo目录执行：

	        git submodule update --init --remote
![submodule_update1](http://kbengine.github.io/assets/img/screenshots/gitbash_submodule.png)

		* 或者使用 TortoiseGit(选择菜单): TortoiseGit -> Submodule Update:
![submodule_update2](http://kbengine.github.io/assets/img/screenshots/unity3d_plugins_submodule_update.jpg)

                * 也可以手动下载服务端Demo资产库

		        服务端资产库下载：
		            https://github.com/kbengine/kbengine_demos_assets/releases/latest
		            下载后请将其解压缩,并将目录文件放置于服务端引擎根目录"kbengine/"之下，如下图：

	3. 拷贝服务端资产库"kbengine_demos_assets"到服务端引擎根目录"kbengine/"之下，如下图：
![demo_configure](http://kbengine.github.io/assets/img/screenshots/demo_copy_kbengine.jpg)


	4. 通过服务端资产库生成KBE客户端插件（可选，默认已经带有一份，除非服务器有相关改动才需要再次生成）
		1: 双击运行 kbengine/kbengine_demos_asset/gensdk.bat
		2: 拷贝kbengine_unity3d_plugins到kbengine_unity3d_demo\Assets\Plugins\


## 配置Demo(可选):

	改变登录IP地址与端口（注意：关于服务端端口部分参看http://kbengine.github.io/cn/docs/installation.html）:
![demo_configure](http://kbengine.github.io/assets/img/screenshots/demo_configure.jpg)

		kbengine_unity3d_demo\Scripts\kbe_scripts\clientapp.cs -> ip
		kbengine_unity3d_demo\Scripts\kbe_scripts\clientapp.cs -> port


## 启动服务器:

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
			(更多参考: http://kbengine.github.io/docs/startup_shutdown.html)


## 启动客户端:

	直接在Unity3D编辑器启动或者编译后启动
	（编译客户端：Unity Editor -> File -> Build Settings -> PC, MAC & Linux Standalone.）


## 生成导航网格(可选):
	
	服务端使用Recastnavigation在3D世界寻路，recastnavigation生成的导航网格（Navmeshs）放置于：
		kbengine\kbengine_demos_assets\res\spaces\*

	在Unity3D中使用插件生成导航网格（Navmeshs）:
		https://github.com/kbengine/unity3d_nav_critterai


## 结构与释义：

	KBE插件与U3D和服务器之间的关系：
		插件与服务器：负责处理与服务端之间的网络消息包、账号登陆/登出流程、由服务端通知创建和销毁逻辑实体、维护同步的逻辑实体属性数据等等。
		插件与U3D：插件将某些事件触发给U3D图形层，图形层决定是否需要捕获某些事件获得数据进行渲染表现（例如：创建怪物、某个NPC的移动速度增加、HP变化）、
			U3D图形层将输入事件触发到插件层（例如：玩家移动了、点击了复活按钮UI），插件逻辑脚本层决定是否需要中转到服务器等等。

	Plugins\kbengine\kbengine_unity3d_plugins：
		KBE客户端插件的核心层代码。

	Scripts\kbe_scripts：
		KBE客户端的逻辑脚本（在此实现对应服务端的实体脚本、实体的背包数据结构、技能客户端判断等）。

		Scripts\kbe_scripts\Account.cs：
			对应KBE服务端的账号实体的客户端部分。

		Scripts\kbe_scripts\Avatar.cs：
			对应KBE服务端的账游戏中玩家实体的客户端部分。

		Scripts\kbe_scripts\Monster.cs：
			对应KBE服务端的怪物实体的客户端部分。

		Scripts\kbe_scripts\clientapp.cs：
			在KBE的体系中抽象出一个客户端APP，其中包含KBE客户端插件的初始化和销毁等等。

		Scripts\kbe_scripts\interfaces：
			对应KBE中entity_defs\interfaces中所声明的模块。

	Scripts\u3d_scripts：
		Unity3D图形层（包括场景渲染、UI、物体部件、人物模型、怪物模型、一切关于显示的东西等等）。

		Scripts\u3d_scripts\GameEntity.cs：
			无论是怪物还是玩家都由此脚本负责模型动画等表现部分。

		Scripts\u3d_scripts\World.cs:
			管理游戏中大地图或副本的渲染层脚本，例如：负责将具体的3D怪物创建到场景中。

		Scripts\u3d_scripts\UI.cs:
			维护游戏的UI处理脚本。

	start.unity:
		起始场景，由此启动进入游戏。

	Scenes\login.unity:
		登陆场景。

	Scenes\selavatars.unity:
		角色选取场景。

	Scenes\world.unity:
		游戏中大地图/副本场景。

## 演示截图:

![screenshots1](http://kbengine.github.io/assets/img/screenshots/unity3d_demo9.jpg)
![screenshots2](http://kbengine.github.io/assets/img/screenshots/unity3d_demo10.jpg)
![screenshots3](http://kbengine.github.io/assets/img/screenshots/unity3d_demo11.jpg)
