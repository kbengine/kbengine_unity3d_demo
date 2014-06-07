client—warring
=============

##服务端引擎:
http://www.kbengine.org

## QQ交流群
群1: 16535321 群2: 367555003

##warring项目运行效果
(截图是服务端和客户端在windows7-x64下搭建运行的效果，商用时服务端可以在linux CentOS下运行。)
![warring项目运行效果--用腾讯微博的外链接达到唯一地址](http://t2.qpic.cn/mblogpic/0d597a54aaf30d4154bc/2000)


##下载建议
采用以下任一种方式下载就行

#1.中国国内用户请访问国内warring镜像
http://git.oschina.net/likecg/warring
使用SSH协议下载，不要用https协议下载

#2.百度网盘下载
http://pan.baidu.com/s/1bn5YxB5

下载后，必须使用winrar5.0.1以上版本解压，解压之后，用git pull命令更新。该命令是增量更新，速度非常快。

#3.中国以外海外用户请直接在bitbucket上下载
https://bitbucket.org/kbengine/warring/

#git操作指南
如果你不懂git,或者你以前使用过svn,但是不懂git,也没要紧，可以访问这个博客和对应视频，学习git。理论上说git只需要学习10条命令就能学会。
http://herry2013git.blog.163.com/blog/static/21956801120144810133569/

如果你想马上下载就能用，但是还不懂git，先下载windows git客户端msysgit,采用百度网盘下载方式。就是第二种方法。

##GO!

	##创建AssetBundles

	unity3d(菜单上)->Publish->Build Pulish AssetBundles - autoAll

	期间可能一直出现"Moving file failed"， 需要点击"try again"让其继续, 
	这可能是unity3d在移动文件时文件被占用造成的。
	执行完毕后检查 Assets->StreamingAssets是否有内容。

	生成服务端寻路数据:
	选中: Assets->Nav_build->xinshoucun->NavmeshBuild->Build & Bake, 等待生成完成之后
	选中: Assets->Nav_build->xinshoucun->CAIBakedNavmesh->Save, 将其中srv_xxx.navmesh放到服务端kbengine\demo\res\spaces\xinshoucun中

##编译:

	unity3d File->Build Settings->Scenes In Build选择scenes/go.unity->Platform

	选择Web Player->Build。 
	(注意：不能在编辑器中跑这个demo， 目前编辑器没有做兼容会出错。)

## web服务器部署文件夹结构

	->(服务器根目录)
		- StreamingAssets (创建AssetBundles生成的文件夹)
		- ui (Assets下的ui文件夹可以直接拷贝过来)
		- crossdomain.xml
		- initLogo.png
		- initProgressBar.PNG
		- initProgressFrame.PNG
		- index.html
		- Assets.unity3d (在unity3d编译时生成的文件)
		- Assets.html (在unity3d编译时生成的文件)


##运行:

	1. 启动kbengine服务端

	2. 浏览器访问localhost

	如不清楚请下载发布版demo， 并按照其中的文件夹结构放置并看压缩包内文档教程如何启动:

	https://sourceforge.net/projects/kbengine/files/


##日志:

	Windows XP: C:\Documents and Settings\username\Local  Settings\Temp\UnityWebPlayer\log

	Windows Vista/7: C:\Users\username\AppData\Local\Temp\UnityWebPlayer\log
