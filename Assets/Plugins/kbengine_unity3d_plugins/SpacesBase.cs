/*
	Generated by KBEngine!
	Please do not modify this file!
	Please inherit this module, such as: (class Spaces : SpacesBase)
	tools = kbcmd
*/

namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	// defined in */scripts/entity_defs/Spaces.def
	// Please inherit and implement "class Spaces : SpacesBase"
	public abstract class SpacesBase : Entity
	{
		public EntityBaseEntityCall_SpacesBase baseEntityCall = null;
		public EntityCellEntityCall_SpacesBase cellEntityCall = null;

		public UInt32 modelID = 0;
		public virtual void onModelIDChanged(UInt32 oldValue) {}
		public Byte modelScale = 30;
		public virtual void onModelScaleChanged(Byte oldValue) {}
		public string name = "";
		public virtual void onNameChanged(string oldValue) {}
		public UInt32 uid = 0;
		public virtual void onUidChanged(UInt32 oldValue) {}
		public UInt32 utype = 0;
		public virtual void onUtypeChanged(UInt32 oldValue) {}


		public SpacesBase()
		{
		}

		public override void onGetBase()
		{
			baseEntityCall = new EntityBaseEntityCall_SpacesBase(id, className);
		}

		public override void onGetCell()
		{
			cellEntityCall = new EntityCellEntityCall_SpacesBase(id, className);
		}

		public override void onLoseCell()
		{
			cellEntityCall = null;
		}

		public override EntityCall getBaseEntityCall()
		{
			return baseEntityCall;
		}

		public override EntityCall getCellEntityCall()
		{
			return cellEntityCall;
		}

		public override void attachComponents()
		{
		}

		public override void detachComponents()
		{
		}

		public override void onRemoteMethodCall(MemoryStream stream)
		{
			ScriptModule sm = EntityDef.moduledefs["Spaces"];

			UInt16 methodUtype = 0;
			UInt16 componentPropertyUType = 0;

			if(sm.useMethodDescrAlias)
			{
				componentPropertyUType = stream.readUint8();
				methodUtype = stream.readUint8();
			}
			else
			{
				componentPropertyUType = stream.readUint16();
				methodUtype = stream.readUint16();
			}

			Method method = null;

			if(componentPropertyUType == 0)
			{
				method = sm.idmethods[methodUtype];
			}
			else
			{
				Property pComponentPropertyDescription = sm.idpropertys[componentPropertyUType];
				switch(pComponentPropertyDescription.properUtype)
				{
					default:
						break;
				}

				return;
			}

			switch(method.methodUtype)
			{
				default:
					break;
			};
		}

		public override void onUpdatePropertys(MemoryStream stream)
		{
			ScriptModule sm = EntityDef.moduledefs["Spaces"];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

			while(stream.length() > 0)
			{
				UInt16 _t_utype = 0;
				UInt16 _t_child_utype = 0;

				{
					if(sm.usePropertyDescrAlias)
					{
						_t_utype = stream.readUint8();
						_t_child_utype = stream.readUint8();
					}
					else
					{
						_t_utype = stream.readUint16();
						_t_child_utype = stream.readUint16();
					}
				}

				Property prop = null;

				if(_t_utype == 0)
				{
					prop = pdatas[_t_child_utype];
				}
				else
				{
					Property pComponentPropertyDescription = pdatas[_t_utype];
					switch(pComponentPropertyDescription.properUtype)
					{
						default:
							break;
					}

					return;
				}

				switch(prop.properUtype)
				{
					case 40001:
						Vector3 oldval_direction = direction;
						direction = stream.readVector3();

						if(prop.isBase())
						{
							if(inited)
								onDirectionChanged(oldval_direction);
						}
						else
						{
							if(inWorld)
								onDirectionChanged(oldval_direction);
						}

						break;
					case 41006:
						UInt32 oldval_modelID = modelID;
						modelID = stream.readUint32();

						if(prop.isBase())
						{
							if(inited)
								onModelIDChanged(oldval_modelID);
						}
						else
						{
							if(inWorld)
								onModelIDChanged(oldval_modelID);
						}

						break;
					case 41007:
						Byte oldval_modelScale = modelScale;
						modelScale = stream.readUint8();

						if(prop.isBase())
						{
							if(inited)
								onModelScaleChanged(oldval_modelScale);
						}
						else
						{
							if(inWorld)
								onModelScaleChanged(oldval_modelScale);
						}

						break;
					case 41003:
						string oldval_name = name;
						name = stream.readUnicode();

						if(prop.isBase())
						{
							if(inited)
								onNameChanged(oldval_name);
						}
						else
						{
							if(inWorld)
								onNameChanged(oldval_name);
						}

						break;
					case 40000:
						Vector3 oldval_position = position;
						position = stream.readVector3();

						if(prop.isBase())
						{
							if(inited)
								onPositionChanged(oldval_position);
						}
						else
						{
							if(inWorld)
								onPositionChanged(oldval_position);
						}

						break;
					case 40002:
						stream.readUint32();
						break;
					case 41004:
						UInt32 oldval_uid = uid;
						uid = stream.readUint32();

						if(prop.isBase())
						{
							if(inited)
								onUidChanged(oldval_uid);
						}
						else
						{
							if(inWorld)
								onUidChanged(oldval_uid);
						}

						break;
					case 41005:
						UInt32 oldval_utype = utype;
						utype = stream.readUint32();

						if(prop.isBase())
						{
							if(inited)
								onUtypeChanged(oldval_utype);
						}
						else
						{
							if(inWorld)
								onUtypeChanged(oldval_utype);
						}

						break;
					default:
						break;
				};
			}
		}

		public override void callPropertysSetMethods()
		{
			ScriptModule sm = EntityDef.moduledefs["Spaces"];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

			Vector3 oldval_direction = direction;
			Property prop_direction = pdatas[2];
			if(prop_direction.isBase())
			{
				if(inited && !inWorld)
					onDirectionChanged(oldval_direction);
			}
			else
			{
				if(inWorld)
				{
					if(prop_direction.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onDirectionChanged(oldval_direction);
					}
				}
			}

			UInt32 oldval_modelID = modelID;
			Property prop_modelID = pdatas[4];
			if(prop_modelID.isBase())
			{
				if(inited && !inWorld)
					onModelIDChanged(oldval_modelID);
			}
			else
			{
				if(inWorld)
				{
					if(prop_modelID.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onModelIDChanged(oldval_modelID);
					}
				}
			}

			Byte oldval_modelScale = modelScale;
			Property prop_modelScale = pdatas[5];
			if(prop_modelScale.isBase())
			{
				if(inited && !inWorld)
					onModelScaleChanged(oldval_modelScale);
			}
			else
			{
				if(inWorld)
				{
					if(prop_modelScale.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onModelScaleChanged(oldval_modelScale);
					}
				}
			}

			string oldval_name = name;
			Property prop_name = pdatas[6];
			if(prop_name.isBase())
			{
				if(inited && !inWorld)
					onNameChanged(oldval_name);
			}
			else
			{
				if(inWorld)
				{
					if(prop_name.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onNameChanged(oldval_name);
					}
				}
			}

			Vector3 oldval_position = position;
			Property prop_position = pdatas[1];
			if(prop_position.isBase())
			{
				if(inited && !inWorld)
					onPositionChanged(oldval_position);
			}
			else
			{
				if(inWorld)
				{
					if(prop_position.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onPositionChanged(oldval_position);
					}
				}
			}

			UInt32 oldval_uid = uid;
			Property prop_uid = pdatas[7];
			if(prop_uid.isBase())
			{
				if(inited && !inWorld)
					onUidChanged(oldval_uid);
			}
			else
			{
				if(inWorld)
				{
					if(prop_uid.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onUidChanged(oldval_uid);
					}
				}
			}

			UInt32 oldval_utype = utype;
			Property prop_utype = pdatas[8];
			if(prop_utype.isBase())
			{
				if(inited && !inWorld)
					onUtypeChanged(oldval_utype);
			}
			else
			{
				if(inWorld)
				{
					if(prop_utype.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onUtypeChanged(oldval_utype);
					}
				}
			}

		}
	}
}
