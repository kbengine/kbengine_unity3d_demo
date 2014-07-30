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
	
	void Awake() 
	 {
		DontDestroyOnLoad(transform.gameObject);
	 }
	 
	// Use this for initialization
	void Start () {
		installEvents();
		Application.LoadLevel("login");
	}

	void installEvents()
	{
		CancelInvoke("installEvents");

		// common
		KBEngine.Event.registerOut("onKicked", this, "onKicked");

		// login
		KBEngine.Event.registerOut("onCreateAccountResult", this, "onCreateAccountResult");
		KBEngine.Event.registerOut("onLoginFailed", this, "onLoginFailed");
		KBEngine.Event.registerOut("onVersionNotMatch", this, "onVersionNotMatch");
		KBEngine.Event.registerOut("onLoginGatewayFailed", this, "onLoginGatewayFailed");
		KBEngine.Event.registerOut("onLoginSuccessfully", this, "onLoginSuccessfully");
		KBEngine.Event.registerOut("login_baseapp", this, "login_baseapp");
		KBEngine.Event.registerOut("Loginapp_importClientMessages", this, "Loginapp_importClientMessages");
		KBEngine.Event.registerOut("Baseapp_importClientMessages", this, "Baseapp_importClientMessages");
		KBEngine.Event.registerOut("Baseapp_importClientEntityDef", this, "Baseapp_importClientEntityDef");
		
		// selavatars
		KBEngine.Event.registerOut("onReqAvatarList", this, "onReqAvatarList");
		KBEngine.Event.registerOut("onCreateAvatarResult", this, "onCreateAvatarResult");
		KBEngine.Event.registerOut("onRemoveAvatar", this, "onRemoveAvatar");
		KBEngine.Event.registerOut("onAvatarEnterWorld", this, "onAvatarEnterWorld");
		KBEngine.Event.registerOut("onDisableConnect", this, "onDisableConnect");
		
		// in world
		KBEngine.Event.registerOut("addSpaceGeometryMapping", this, "addSpaceGeometryMapping");
		KBEngine.Event.registerOut("onEnterWorld", this, "onEnterWorld");
		KBEngine.Event.registerOut("onLeaveWorld", this, "onLeaveWorld");
		KBEngine.Event.registerOut("set_position", this, "set_position");
		KBEngine.Event.registerOut("set_direction", this, "set_direction");
		KBEngine.Event.registerOut("update_position", this, "update_position");
		KBEngine.Event.registerOut("set_HP", this, "set_HP");
		KBEngine.Event.registerOut("set_MP", this, "set_MP");
		KBEngine.Event.registerOut("set_HP_Max", this, "set_HP_Max");
		KBEngine.Event.registerOut("set_MP_Max", this, "set_MP_Max");
		KBEngine.Event.registerOut("set_level", this, "set_level");
		KBEngine.Event.registerOut("set_name", this, "set_entityName");
		KBEngine.Event.registerOut("set_state", this, "set_state");
		KBEngine.Event.registerOut("set_moveSpeed", this, "set_moveSpeed");
		KBEngine.Event.registerOut("set_modelScale", this, "set_modelScale");
		KBEngine.Event.registerOut("set_modelID", this, "set_modelID");
		KBEngine.Event.registerOut("recvDamage", this, "recvDamage");
		KBEngine.Event.registerOut("otherAvatarOnJump", this, "otherAvatarOnJump");
		KBEngine.Event.registerOut("onAddSkill", this, "onAddSkill");
		KBEngine.Event.registerOut("onConnectStatus", this, "onConnectStatus");
	}

	void OnDestroy()
	{
		KBEngine.Event.deregisterOut(this);
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
				ui_state = 2;
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
			//	Byte roleType = (Byte)info["roleType"];
				string name = (string)info["name"];
			//	UInt16 level = (UInt16)info["level"];
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
			{
				login();
			}
			else
			{
				err("account or password is error, length < 6!(账号或者密码错误，长度必须大于5!)");
			}
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
				err("account or password is error, length < 6!(账号或者密码错误，长度必须大于5!)");
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
   			
			UnityEngine.GameObject obj = UnityEngine.GameObject.Find("player(Clone)");
			if(obj != null)
			{
				GUI.Label(new Rect((Screen.width / 2) - 100, 20, 400, 100), "position=" + obj.transform.position.ToString()); 
			}
   		}
   		else
   		{
   			onLoginUI();
   		}
   		
		if(KBEngineApp.app != null && KBEngineApp.app.serverVersion != "" 
			&& KBEngineApp.app.serverVersion != KBEngineApp.app.clientVersion)
		{
			labelColor = Color.red;
			labelMsg = "version not match(curr=" + KBEngineApp.app.clientVersion + ", srv=" + KBEngineApp.app.serverVersion + " )(版本不匹配)";
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
		
		KBEngine.Event.fireIn("login", new object[]{stringAccount, stringPasswd});
	}
	
	public void createAccount()
	{
		info("connect to server...(连接到服务端...)");
		
		KBEngine.Event.fireIn("createAccount", new object[]{stringAccount, stringPasswd});
	}
	
	public void onCreateAccountResult(UInt16 retcode, byte[] datas)
	{
		if(retcode != 0)
		{
			err("createAccount is error(注册账号错误)! err=" + KBEngineApp.app.serverErr(retcode));
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
	
	public void onConnectStatus(bool success)
	{
		if(!success)
			err("connect is error! (连接错误)");
		else
			info("connect successfully, please wait...(连接成功，请等候...)");
	}
	
	public void onLoginFailed(UInt16 failedcode)
	{
		if(failedcode == 20)
		{
			err("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode) + ", " + System.Text.Encoding.ASCII.GetString(KBEngineApp.app.serverdatas()));
		}
		else
		{
			err("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode));
		}
	}
	
	public void onVersionNotMatch(string verInfo, string serVerInfo)
	{
		err("");
	}
	
	public void onLoginGatewayFailed(UInt16 failedcode)
	{
		err("loginGateway is failed(登陆网关失败), err=" + KBEngineApp.app.serverErr(failedcode));
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

	public void onKicked(UInt16 failedcode)
	{
		err("kick, disconnect!, reason=" + KBEngineApp.app.serverErr(failedcode));
		Application.LoadLevel("login");
		ui_state = 0;
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
			err("Error creating avatar, errcode=" + retcode);
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
		info("scene(" + respath + "), spaceID=" + KBEngineApp.app.spaceID);
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
		
		object speed = avatar.getDefinedPropterty("moveSpeed");
		if(speed != null)
			set_moveSpeed(avatar, speed);
		
		object state = avatar.getDefinedPropterty("state");
		if(state != null)
			set_state(avatar, state);
		
		object modelScale = avatar.getDefinedPropterty("modelScale");
		if(modelScale != null)
			set_modelScale(avatar, modelScale);
		
		object name = avatar.getDefinedPropterty("name");
		if(name != null)
			set_entityName(avatar, (string)name);
		
		object hp = avatar.getDefinedPropterty("HP");
		if(hp != null)
			set_HP(avatar, hp);
	}

	public void createPlayer()
	{
		if (player != null)
			return;

		if (KBEngineApp.app.entity_type != "Avatar") {
			return;
		}

		KBEngine.Avatar avatar = (KBEngine.Avatar)KBEngineApp.app.player();
		if(avatar == null)
		{
			Debug.Log("wait create(palyer)!");
			return;
		}
		
		float y = avatar.position.y;
		if(avatar.isOnGound)
			y = 1.3f;
		
		player = Instantiate(avatarPerfab, new Vector3(avatar.position.x, y, avatar.position.z), 
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
		
		float y = entity.position.y;
		if(entity.isOnGound)
			y = 1.3f;
		
		entity.renderObj = Instantiate(entityPerfab, new Vector3(entity.position.x, y, entity.position.z), 
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

		((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().destPosition = entity.position;
		((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().position = entity.position;
	}

	public void update_position(KBEngine.Entity entity)
	{
		if(entity.renderObj == null)
			return;
		
		GameEntity gameEntity = ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>();
		gameEntity.destPosition = entity.position;
		gameEntity.isOnGound = entity.isOnGound;
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
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().entity_name = (string)v;
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
