using KBEngine;
using UnityEngine;
using System; 
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class UI : MonoBehaviour {
	
	public int ui_state = 0;
	private string stringAccount = "";
	private string stringPasswd = "";
	private string labelMsg = "";
	private Color labelColor = Color.green;
	
	private Dictionary<UInt64, Dictionary<string, object>> ui_avatarList = null;
	
	private string stringAvatarName = "";
	private bool startCreateAvatar = false;
	
	private UInt64 selAvatarDBID = 0;
	
	private UnityEngine.GameObject terrain = null;
	public UnityEngine.GameObject terrainPerfab;
	
	private UnityEngine.GameObject player = null;
	public UnityEngine.GameObject entityPerfab;
	public UnityEngine.GameObject avatarPerfab;
	
	private bool showReliveGUI = false;

	// Use this for initialization
	void Start () {
		installEvents();
	}

	void installEvents()
	{
		CancelInvoke("installEvents");
		
		// login
		KBEngine.Event.register("onCreateAccountResult", this, "onCreateAccountResult");
		KBEngine.Event.register("onLoginFailed", this, "onLoginFailed");
		KBEngine.Event.register("onVersionNotMatch", this, "onVersionNotMatch");
		KBEngine.Event.register("onLoginGatewayFailed", this, "onLoginGatewayFailed");
		KBEngine.Event.register("onLoginSuccessfully", this, "onLoginSuccessfully");
		KBEngine.Event.register("login_baseapp", this, "login_baseapp");
		KBEngine.Event.register("Loginapp_importClientMessages", this, "Loginapp_importClientMessages");
		KBEngine.Event.register("Baseapp_importClientMessages", this, "Baseapp_importClientMessages");
		KBEngine.Event.register("Baseapp_importClientEntityDef", this, "Baseapp_importClientEntityDef");
		
		// selavatars
		KBEngine.Event.register("onReqAvatarList", this, "onReqAvatarList");
		KBEngine.Event.register("onCreateAvatarResult", this, "onCreateAvatarResult");
		KBEngine.Event.register("onRemoveAvatar", this, "onRemoveAvatar");
		KBEngine.Event.register("onAvatarEnterWorld", this, "onAvatarEnterWorld");
		KBEngine.Event.register("onDisableConnect", this, "onDisableConnect");
		
		// in world
		KBEngine.Event.register("addSpaceGeometryMapping", this, "addSpaceGeometryMapping");
		KBEngine.Event.register("onEnterWorld", this, "onEnterWorld");
		KBEngine.Event.register("onLeaveWorld", this, "onLeaveWorld");
		KBEngine.Event.register("set_position", this, "set_position");
		KBEngine.Event.register("set_direction", this, "set_direction");
		KBEngine.Event.register("update_position", this, "update_position");
		KBEngine.Event.register("set_HP", this, "set_HP");
		KBEngine.Event.register("set_MP", this, "set_MP");
		KBEngine.Event.register("set_HP_Max", this, "set_HP_Max");
		KBEngine.Event.register("set_MP_Max", this, "set_MP_Max");
		KBEngine.Event.register("set_level", this, "set_level");
		KBEngine.Event.register("set_name", this, "set_entityName");
		KBEngine.Event.register("set_state", this, "set_state");
		KBEngine.Event.register("set_moveSpeed", this, "set_moveSpeed");
		KBEngine.Event.register("set_modelScale", this, "set_modelScale");
		KBEngine.Event.register("set_modelID", this, "set_modelID");
		KBEngine.Event.register("recvDamage", this, "recvDamage");
		KBEngine.Event.register("otherAvatarOnJump", this, "otherAvatarOnJump");
		KBEngine.Event.register("onAddSkill", this, "onAddSkill");
	}

	void OnDestroy()
	{
		KBEngine.Event.deregister(this);
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKeyUp(KeyCode.Space))
        {
			Debug.Log("KeyCode.Space");
			
			if(KBEngineApp.app.entity_type == "Avatar")
			{
				KBEngine.Avatar avatar = (KBEngine.Avatar)KBEngineApp.app.player();
				if(avatar != null)
					avatar.jump();
			}
        }
	}
	
	void onSelAvatarUI()
	{
        if (startCreateAvatar == false && GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 40, 200, 30), "CreateAvatar(创建角色)"))    
        {
        	startCreateAvatar = !startCreateAvatar;
        }

        if (startCreateAvatar == false && GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 75, 200, 30), "EnterGame(进入游戏)"))    
        {
        	if(selAvatarDBID == 0)
        	{
        		err("Please select a Avatar!(请选择角色!)");
        	}
        	else
        	{
        		info("Please wait...(请稍后...)");
				Account account = (Account)KBEngineApp.app.player();
				if(account != null)
					account.selectAvatarGame(selAvatarDBID);
				
				Application.LoadLevel("world");
			}
        }
		
		if(startCreateAvatar)
		{
	        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 40, 200, 30), "CreateAvatar-OK(创建完成)"))    
	        {
	        	if(stringAvatarName.Length > 1)
	        	{
		        	startCreateAvatar = !startCreateAvatar;
					Account account = (Account)KBEngineApp.app.player();
					account.reqCreateAvatar(1, stringAvatarName);
				}
				else
				{
					err("avatar name is null(角色名称为空)!");
				}
	        }
	        
	        stringAvatarName = GUI.TextField(new Rect(Screen.width / 2 - 100, Screen.height - 75, 200, 30), stringAvatarName, 20);
		}
		
		if(ui_avatarList != null && ui_avatarList.Count > 0)
		{
			int idx = 0;
			foreach(UInt64 dbid in ui_avatarList.Keys)
			{
				Dictionary<string, object> info = ui_avatarList[dbid];
				Byte roleType = (Byte)info["roleType"];
				string name = (string)info["name"];
				UInt16 level = (UInt16)info["level"];
				UInt64 idbid = (UInt64)info["dbid"];

				idx++;
				
				Color color = GUI.contentColor;
				if(selAvatarDBID == idbid)
				{
					GUI.contentColor = Color.red;
				}
				
				if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 120 - 35 * idx, 200, 30), name))    
				{
					Debug.Log("selAvatar:" + name);
					selAvatarDBID = idbid;
				}
				
				GUI.contentColor = color;
			}
		}
		else
		{
			if(KBEngineApp.app.entity_type == "Account")
			{
				KBEngine.Account account = (KBEngine.Account)KBEngineApp.app.player();
				if(account != null)
					ui_avatarList = new Dictionary<ulong, Dictionary<string, object>>(account.avatars);
			}
		}
	}
	
	void onLoginUI()
	{
		if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 30, 200, 30), "Login(登陆)"))  
        {  
        	Debug.Log("stringAccount:" + stringAccount);
        	Debug.Log("stringPasswd:" + stringPasswd);
        	
			if(stringAccount.Length > 0 && stringPasswd.Length > 5)
				login();
			else
				err("account is error!(账号错误!)");
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 70, 200, 30), "CreateAccount(注册账号)"))  
        {  
			Debug.Log("stringAccount:" + stringAccount);
			Debug.Log("stringPasswd:" + stringPasswd);

			if(stringAccount.Length > 0 && stringPasswd.Length > 5)
			{
				createAccount();
			}
			else
			{
				err("account is error!(账号错误!)");
			}
        }
        
		stringAccount = GUI.TextField(new Rect (Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 30), stringAccount, 20);
		stringPasswd = GUI.PasswordField(new Rect (Screen.width / 2 - 100, Screen.height / 2 - 10, 200, 30), stringPasswd, '*');
	}
	
    void OnGUI()  
    {  
		if(ui_state == 1)
		{
			onSelAvatarUI();
   		}
   		else if(ui_state == 2)
   		{
			createPlayer();
   			if(showReliveGUI)
   			{
				if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 30), "Relive(复活)"))  
		        {
					if(KBEngineApp.app.entity_type == "Avatar")
					{
						KBEngine.Avatar avatar = (KBEngine.Avatar)KBEngineApp.app.player();
						if(avatar != null)
							avatar.relive(1);
					}		        	
		        }
   			}
   		}
   		else
   		{
   			onLoginUI();
   		}
   		
   		GUI.contentColor = labelColor;
		GUI.Label(new Rect((Screen.width / 2) - 100, 40, 400, 100), labelMsg);
	}  
	
	public void err(string s)
	{
		labelColor = Color.red;
		labelMsg = s;
	}
	
	public void info(string s)
	{
		labelColor = Color.green;
		labelMsg = s;
	}
	
	public void login()
	{
		info("connect to server...(连接到服务端...)");
		
		KBEngineApp.app.username = stringAccount;
		KBEngineApp.app.password = stringPasswd;
		
		if(!KBEngineApp.app.login_loginapp(true))
		{
			err("connect is error!(连接错误!)");
			return;
		}
		
		info("connect successfully, please wait...(连接成功，请等候...)");
	}
	
	public void createAccount()
	{
		info("connect to server...(连接到服务端...)");
		
		KBEngineApp.app.username = stringAccount;
		KBEngineApp.app.password = stringPasswd;
		if(!KBEngineApp.app.createAccount_loginapp(true))
		{
			err("connect is error!(连接错误!)");
			return;
		}
		
		info("connect successfully, please wait...(连接成功，请等候...)");
	}
	
	public void onCreateAccountResult(UInt16 retcode, byte[] datas)
	{
		if(retcode != 0)
		{
			err("createAccount is error(注册账号错误)! errcode(错误码)=" + KBEngineApp.app.mercuryErr(retcode));
			return;
		}
		
		if(KBEngineApp.validEmail(stringAccount))
		{
			info("createAccount is successfully, Please activate your Email!(注册账号成功，请激活Email!)");
		}
		else
		{
			info("createAccount is successfully!(注册账号成功!)");
		}
	}
	
	public void onLoginFailed(UInt16 failedcode)
	{
		err("login is failed(登陆失败), errcode(错误码)=" + failedcode);
	}
	
	public void onVersionNotMatch(string verInfo, string serVerInfo)
	{
		err("version not match(curr=" + verInfo + ", srv" + serVerInfo + " )(版本不匹配)");
	}
	
	public void onLoginGatewayFailed(UInt16 failedcode)
	{
		err("loginGateway is failed(登陆网关失败), errcode(错误码)=" + failedcode);
	}
	
	public void login_baseapp()
	{
		info("connect to loginGateway, please wait...(连接到网关， 请稍后...)");
	}

	public void onLoginSuccessfully(UInt64 rndUUID, Int32 eid, Account accountEntity)
	{
		info("login is successfully!(登陆成功!)");
		ui_state = 1;

		Application.LoadLevel("selavatars");
	}
	
	public void Loginapp_importClientMessages()
	{
		info("Loginapp_importClientMessages ...");
	}

	public void Baseapp_importClientMessages()
	{
		info("Baseapp_importClientMessages ...");
	}
	
	public void Baseapp_importClientEntityDef()
	{
		info("importClientEntityDef ...");
	}
	
	public void onReqAvatarList(Dictionary<UInt64, Dictionary<string, object>> avatarList)
	{
		ui_avatarList = avatarList;
	}
	
	public void onCreateAvatarResult(Byte retcode, object info, Dictionary<UInt64, Dictionary<string, object>> avatarList)
	{
		if(retcode != 0)
		{
			err("Error creating avatar, errcode(错误码)=" + retcode);
			return;
		}
		
		onReqAvatarList(avatarList);
	}
	
	public void onRemoveAvatar(UInt64 dbid, Dictionary<UInt64, Dictionary<string, object>> avatarList)
	{
		if(dbid == 0)
		{
			err("Delete the avatar error!(删除角色错误!)");
			return;
		}
		
		onReqAvatarList(avatarList);
	}
	
	public void onDisableConnect()
	{
	}
	
	public void addSpaceGeometryMapping(string respath)
	{
		Debug.Log("loading scene(" + respath + ")...");
		info("scene(" + respath + ")");
		if(terrain == null)
			terrain = Instantiate(terrainPerfab) as UnityEngine.GameObject;
	}	
	
	public void onAvatarEnterWorld(UInt64 rndUUID, Int32 eid, KBEngine.Avatar avatar)
	{
		if(!avatar.isPlayer())
		{
			return;
		}

		info("loading scene...(加载场景中...)");
		Debug.Log("loading scene...");
	}

	public void createPlayer()
	{
		if (player != null)
			return;

		if (KBEngineApp.app.entity_type != "Avatar") {
			return;
		}

		KBEngine.Avatar avatar = (KBEngine.Avatar)KBEngineApp.app.player();

		player = Instantiate(avatarPerfab, new Vector3(avatar.position.x, 1.3f, avatar.position.z), 
		                     Quaternion.Euler(new Vector3(avatar.direction.y, avatar.direction.z, avatar.direction.x))) as UnityEngine.GameObject;
		
		avatar.renderObj = player;
		((UnityEngine.GameObject)avatar.renderObj).GetComponent<GameEntity>().isPlayer = true;
	}

	public void onAddSkill(KBEngine.Entity entity)
	{
		Debug.Log("onAddSkill");
	}
	
	public void onEnterWorld(KBEngine.Entity entity)
	{
		if(entity.isPlayer())
			return;
		
		entity.renderObj = Instantiate(entityPerfab, new Vector3(entity.position.x, 1.3f, entity.position.z), 
			Quaternion.Euler(new Vector3(entity.direction.y, entity.direction.z, entity.direction.x))) as UnityEngine.GameObject;
		
		((UnityEngine.GameObject)entity.renderObj).name = entity.classtype + entity.id;
		
		object speed = entity.getDefinedPropterty("moveSpeed");
		if(speed != null)
			set_moveSpeed(entity, speed);
		
		object state = entity.getDefinedPropterty("state");
		if(state != null)
			set_state(entity, state);
		
		object modelScale = entity.getDefinedPropterty("modelScale");
		if(modelScale != null)
			set_modelScale(entity, modelScale);
		
		object name = entity.getDefinedPropterty("name");
		if(name != null)
			set_entityName(entity, (string)name);
		
		object hp = entity.getDefinedPropterty("HP");
		if(hp != null)
			set_HP(entity, hp);
	}
	
	public void onLeaveWorld(KBEngine.Entity entity)
	{
		if(entity.renderObj == null)
			return;
		
		UnityEngine.GameObject.Destroy((UnityEngine.GameObject)entity.renderObj);
		entity.renderObj = null;
	}

	public void set_position(KBEngine.Entity entity)
	{
		if(entity.renderObj == null)
			return;

		((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().destPosition = new Vector3(entity.position.x, 1.3f, entity.position.z);
		((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().position = new Vector3(entity.position.x, 1.3f, entity.position.z);
	}

	public void update_position(KBEngine.Entity entity)
	{
		if(entity.renderObj == null)
			return;

		((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().destPosition = new Vector3(entity.position.x, 1.3f, entity.position.z);
	}
	
	public void set_direction(KBEngine.Entity entity)
	{
		if(entity.renderObj == null)
			return;
		
		((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().destDirection = 
			new Vector3(entity.direction.y, entity.direction.z, entity.direction.x); 
	}

	public void set_HP(KBEngine.Entity entity, object v)
	{
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().hp = "" + (Int32)v + "/" + (Int32)entity.getDefinedPropterty("HP_Max");
		}
	}
	
	public void set_MP(KBEngine.Entity entity, object v)
	{
	}
	
	public void set_HP_Max(KBEngine.Entity entity, object v)
	{
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().hp = (Int32)entity.getDefinedPropterty("HP") + "/" + (Int32)v;
		}
	}
	
	public void set_MP_Max(KBEngine.Entity entity, object v)
	{
	}
	
	public void set_level(KBEngine.Entity entity, object v)
	{
	}
	
	public void set_entityName(KBEngine.Entity entity, object v)
	{
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().name = (string)v;
		}
	}
	
	public void set_state(KBEngine.Entity entity, object v)
	{
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().set_state((SByte)v);
		}
		
		if(entity.isPlayer())
		{
			Debug.Log("player->set_state: " + v);
			
			if(((SByte)v) == 1)
				showReliveGUI = true;
			else
				showReliveGUI = false;
			
			return;
		}
	}

	public void set_moveSpeed(KBEngine.Entity entity, object v)
	{
		float fspeed = ((float)(Byte)v) / 10f;
		
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().speed = fspeed;
		}
	}
	
	public void set_modelScale(KBEngine.Entity entity, object v)
	{
	}
	
	public void set_modelID(KBEngine.Entity entity, object v)
	{
	}
	
	public void recvDamage(KBEngine.Entity entity, KBEngine.Entity attacker, Int32 skillID, Int32 damageType, Int32 damage)
	{
	}
	
	public void otherAvatarOnJump(KBEngine.Entity entity)
	{
		Debug.Log("otherAvatarOnJump: " + entity.id);
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().OnJump();
		}
	}
}
