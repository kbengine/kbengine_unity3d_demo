using UnityEngine;
using System.Collections.Generic;

/* writed by penghuawei
 * DataSection基类，声明了一些基础的接口
 */

namespace DataSection {

	public class SectionBase {
		private string name_ = "";	// section key
		private string value_ = "";	// section value

		public SectionBase( string name = "", string value = "" )
		{
			name_ = name;
			value_ = value.Trim();
		}

		public string name
		{
			get {
				return name_;
			}
			protected set {
				name_ = value;
			}
		}

		public string value
		{
			get {
				return value_;
			}
			set {
				value_ = value.Trim();
			}
		}

		public string asString
		{
			get {
				return value_;
			}
			set {
				value_ = value;
			}
		}

		public bool asBool
		{
			get {
				return value_ == "True" || value_ == "true";
			}
			set {
				if (value)
					value_ = "true";
				else
					value_ = "false";
			}
		}
		
		public int asInt
		{
			get {
				return int.Parse(value_);
			}
			set {
				value_ = value.ToString();
			}
		}
		
		public float asFloat
		{
			get {
				return float.Parse(value_);
			}
			set {
				value_ = value.ToString();
			}
		}
		
		public double asDouble
		{
			get {
				return double.Parse(value_);
			}
			set {
				value_ = value.ToString();
			}
		}
		
		public Vector2 asVector2
		{
			get {
				Vector2 vec = new Vector2(0.0f, 0.0f);
				string[] vs = value_.Split( ' ' );
				if (vs.Length >= 2)
				{
					vec.Set( float.Parse(vs[0]), float.Parse(vs[1]) );
				}
				return vec;
			}
			set {
				value_ = string.Format( "%f %f", value.x, value.y );
			}
		}
		
		public Vector3 asVector3
		{
			get {
				Vector3 vec = new Vector3(0.0f, 0.0f, 0.0f);
				string[] vs = value_.Split( ' ' );
				if (vs.Length >= 3)
				{
					vec.Set( float.Parse(vs[0]), float.Parse(vs[1]), float.Parse(vs[2]) );
				}
				return vec;
			}
			set {
				value_ = string.Format( "%f %f %f", value.x, value.y, value.z );
			}
		}
		
		public Vector4 asVector4
		{
			get {
				Vector4 vec = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
				string[] vs = value_.Split( ' ' );
				if (vs.Length >= 4)
				{
					vec.Set( float.Parse(vs[0]), float.Parse(vs[1]), float.Parse(vs[2]), float.Parse(vs[3]) );
				}
				return vec;
			}
			set {
				value_ = string.Format( "%f %f %f %f", value.x, value.y, value.z, value.w );
			}
		}
		
		public int[] asIntArray
		{
			get {
				string[] vs = value_.Split( ' ' );
				int[] array = new int[vs.Length];
				int index = 0;
				foreach (string v in vs)
				{
					array[index] = int.Parse(v);
					index++;
				}
				return array;
			}
			set {
				string[] s = new string[value.Length];
				int index = 0;
				foreach (int v in value)
				{
					s[index] = v.ToString();
					index++;
				}
				value_ = string.Join (" ", s);
			}
		}

		public float[] asFloatArray
		{
			get {
				string[] vs = value_.Split( ' ' );
				float[] array = new float[vs.Length];
				int index = 0;
				foreach (string v in vs)
				{
					array[index] = float.Parse(v);
					index++;
				}
				return array;
			}
			set {
				string[] s = new string[value.Length];
				int index = 0;
				foreach (float v in value)
				{
					s[index] = v.ToString();
					index++;
				}
				value_ = string.Join (" ", s);
			}
		}

	}


	public class SectionNode<T> : SectionBase
		where T : SectionNode<T>, new()
	{
		private T parent_ = null;
		private List<T> childNodes_ = new List<T>();

		public SectionNode( string name = "", string value = "", T parent = null ) : base( name, value )
		{
			parent_ = parent;
		}

		private string[] stringLastSplit( string str )
		{
			int pos = str.LastIndexOf( "/" );
			if (pos == -1)
			{
				return new string[1]{ str };
			}
			else
			{
				return new string[2]{str.Substring(0, pos), str.Substring( pos + 1 )};
			}
		}

		private T newSection_( string key )
		{
			T section = new T();
			section.name = key;
			section.parent = (T)this;
			childNodes_.Add( section );
			return section;
		}

		private int findChildIndex( string key )
		{
			return childNodes_.FindIndex( (T v) => v.name == key );
		}

		private int findChildIndex( int startIndex, string key )
		{
			return childNodes_.FindIndex( startIndex, (T v) => v.name == key );
		}
		
		private T getSection_( string path, bool createIfNotExisted = false )
		{
			string[] p = path.Split( '/' );
			T section = (T)this;
			foreach (string e in p)
			{
				int index = section.findChildIndex( e );
				if (index == -1)
				{
					if (createIfNotExisted)
						section = newSection_( e );
					else
						return null;
				}
				else
				{
					section = section.childNodes_[index];
				}
			}
			return section;
		}

		/* 取得路径指向的section的前一个section，并返最后一个key
		 * 例如：getPrevSection_( "a/b/c/d", section ) 则返回“d”，并且section指向"a/b/c"
		 */
		private string getPrevSection_( out T section, string path, bool createIfNotExisted = false )
		{
			string[] splitP = stringLastSplit( path );
			string key;
			if (splitP.Length == 1)
			{
				section = (T)this;
				key = splitP[0];
			}
			else
			{
				section = getSection_( splitP[0], createIfNotExisted );
				key = splitP[1];
			}
			return key;
		}
		
		private List<T> getSections_( string path )
		{
			string[] splitPath = stringLastSplit( path );
			T section = null;
			string p;

			if (splitPath.Length == 1)
			{
				section = (T)this;
				p = splitPath[0];
			}
			else
			{
				p = splitPath[1];
				section = getSection_( splitPath[0] );
			}

			int sectionPos = 0;
			List<T> nodes = new List<T>();
			while (true)
			{
				int index = section.findChildIndex( sectionPos, p );
				if (index == -1)
					return nodes;

				sectionPos = index + 1;
				nodes.Add( section.childNodes_[index] );
			}
		}

		/* 仅在当前section中移除指定名称的子section（仅删除一个），不做深度搜索 */
		private bool removeSection_( string key )
		{
			int index = findChildIndex( key );
			if (index != -1)
			{
				childNodes_.RemoveAt( index );
				return true;
			}
			return false;
		}

		public T parent
		{
			get {
				return parent_;
			}
			set {
				parent_ = value;
			}
		}

		public T child( int index )
		{
			return childNodes_[index];
		}

		public string childName( int index )
		{
			return childNodes_[index].name;
		}

		public T createSection( string path )
		{
			string[] splitP = path.Split ( '/' );
			T section = (T)this;

			if (splitP.Length == 1)
			{
				section = section.newSection_( splitP[0] );
			}
			else
			{
				foreach (string key in splitP)
				{
					section = section.newSection_( key );
				}
			}

			return section;
		}

		public bool deleteSection( string path )
		{
			string[] splitS = stringLastSplit( path );
			T section = null;
			string key = "";
			if (splitS.Length == 1)
			{
				key = splitS[0];
				section = (T)this;
			}
			else
			{
				key = splitS[1];
				section = getSection_( splitS[0] );
			}

			if (section != null)
			{
				section.removeSection_( key );
				return true;
			}
			return false;
		}

		public void copy( T source )
		{
			value = source.value;
			foreach (T section in source.childNodes_)
			{
				T newSection = createSection( section.name );
				newSection.copy ( section );
			}
		}

		public bool has_key( string key )
		{
			return childNodes_.FindIndex( (T v) => v.name == key ) > -1;
		}

		public string[] keys()
		{
			string[] result = new string[childNodes_.Count];
			int i = 0;
			foreach (T section in childNodes_)
			{
				result[i] = section.name;
				i++;
			}
			return result;
		}

		public T[] values( string key )
		{
			return childNodes_.ToArray();
		}

		public T this [ string path ]
		{
			get {
				return getSection_( path );
			}
		}

		public bool readBool( string path )
		{
			T section = getSection_( path );
			if (section != null)
				return section.asBool;
			else
				return false;
		}

		public float readFloat( string path )
		{
			T section = getSection_( path );
			if (section != null)
				return section.asFloat;
			else
				return 0.0f;
		}
		
		public float[] readFloats( string path )
		{
			List<float> result = new List<float>();
			List<T> sections = getSections_( path );
			foreach (T section in sections)
			{
				result.Add( section.asFloat );
			}
			return result.ToArray();
		}
		
		public double readDouble( string path )
		{
			T section = getSection_( path );
			if (section != null)
				return section.asDouble;
			else
				return 0.0;
		}
		
		public double[] readDoubles( string path )
		{
			List<double> result = new List<double>();
			List<T> sections = getSections_( path );
			foreach (T section in sections)
			{
				result.Add( section.asDouble );
			}
			return result.ToArray();
		}
		
		public int readInt( string path )
		{
			T section = getSection_( path );
			if (section != null)
				return section.asInt;
			else
				return 0;
		}
		
		public int[] readInts( string path )
		{
			List<int> result = new List<int>();
			List<T> sections = getSections_( path );
			foreach (T section in sections)
			{
				result.Add( section.asInt );
			}
			return result.ToArray();
		}
		
		public string readString( string path )
		{
			T section = getSection_( path );
			if (section != null)
				return section.asString;
			else
				return "";
		}
		
		public string[] readStrings( string path )
		{
			List<string> result = new List<string>();
			List<T> sections = getSections_( path );
			foreach (T section in sections)
			{
				result.Add( section.asString );
			}
			return result.ToArray();
		}
		
		public Vector2 readVector2( string path )
		{
			T section = getSection_( path );
			if (section != null)
				return section.asVector2;
			else
				return new Vector2();
		}
		
		public Vector2[] readVector2s( string path )
		{
			List<Vector2> result = new List<Vector2>();
			List<T> sections = getSections_( path );
			foreach (T section in sections)
			{
				result.Add( section.asVector2 );
			}
			return result.ToArray();
		}
		
		public Vector3 readVector3( string path )
		{
			T section = getSection_( path );
			if (section != null)
				return section.asVector3;
			else
				return new Vector3();
		}
		
		public Vector3[] readVector3s( string path )
		{
			List<Vector3> result = new List<Vector3>();
			List<T> sections = getSections_( path );
			foreach (T section in sections)
			{
				result.Add( section.asVector3 );
			}
			return result.ToArray();
		}
		
		public Vector4 readVector4( string path )
		{
			T section = getSection_( path );
			if (section != null)
				return section.asVector4;
			else
				return new Vector4();
		}
		
		public Vector4[] readVector4s( string path )
		{
			List<Vector4> result = new List<Vector4>();
			List<T> sections = getSections_( path );
			foreach (T section in sections)
			{
				result.Add( section.asVector4 );
			}
			return result.ToArray();
		}
		
		public T writeBool( string path, bool val )
		{
			T section = getSection_( path, true );
			section.asBool = val;
			return section;
		}
		
		public T writeFloat( string path, float val )
		{
			T section = getSection_( path, true );
			section.asFloat = val;
			return section;
		}
		
		public void writeFloats( string path, float[] vals )
		{
			T section = null;
			string key = getPrevSection_( out section, path, true );

			foreach (float v in vals)
			{
				section.createSection( key ).asFloat = v;
			}
		}
		
		public T writeDouble( string path, double val )
		{
			T section = getSection_( path, true );
			section.asDouble = val;
			return section;
		}
		
		public void writeDoubles( string path, double[] vals )
		{
			T section = null;
			string key = getPrevSection_( out section, path, true );
			
			foreach (double v in vals)
			{
				section.createSection( key ).asDouble = v;
			}
		}
		
		public T writeInt( string path, int val )
		{
			T section = getSection_( path, true );
			section.asInt = val;
			return section;
		}
		
		public void writeInts( string path, int[] vals )
		{
			T section = null;
			string key = getPrevSection_( out section, path, true );
			
			foreach (int v in vals)
			{
				section.createSection( key ).asInt = v;
			}
		}
		
		public T writeString( string path, string val )
		{
			T section = getSection_( path, true );
			section.asString = val;
			return section;
		}

		public void writeStrings( string path, string[] vals )
		{
			T section = null;
			string key = getPrevSection_( out section, path, true );
			
			foreach (string v in vals)
			{
				section.createSection( key ).asString = v;
			}
		}
		
		public T writeVector2( string path, Vector2 val )
		{
			T section = getSection_( path, true );
			section.asVector2 = val;
			return section;
		}
		
		public void writeVector2s( string path, Vector2[] vals )
		{
			T section = null;
			string key = getPrevSection_( out section, path, true );
			
			foreach (Vector2 v in vals)
			{
				section.createSection( key ).asVector2 = v;
			}
		}
		
		public T writeVector3( string path, Vector3 val )
		{
			T section = getSection_( path, true );
			section.asVector3 = val;
			return section;
		}
		
		public void writeVector3s( string path, Vector3[] vals )
		{
			T section = null;
			string key = getPrevSection_( out section, path, true );
			
			foreach (Vector3 v in vals)
			{
				section.createSection( key ).asVector3 = v;
			}
		}
		
		public T writeVector4( string path, Vector4 val )
		{
			T section = getSection_( path, true );
			section.asVector4 = val;
			return section;
		}
		
		public void writeVector4S( string path, Vector4[] vals )
		{
			T section = null;
			string key = getPrevSection_( out section, path, true );
			
			foreach (Vector4 v in vals)
			{
				section.createSection( key ).asVector4 = v;
			}
		}
		
	}
}
