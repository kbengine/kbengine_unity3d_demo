using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Mono.Xml;

/* writed by penghuawei
	一个简单读取 xml 配置的工具。
	using DataSection
	XMLParser parser = new XMLParser();
	//XMLSection root = parser.loadXML( "<abc><a k=\"1\">100</a></abc>" );
	XMLSection root = parser.loadFile( "test" );
	rootSection["key"] = value
	rootSection.readString( "anykey" )

	SXML["key"].save( filename )	# save as ... 木有实现
	SXML.save() # save to src file

	注：
	1.由于XMLParser使用的是Mono.Xml来解释xml字符串，因此需要Mono.Xml库支持
	2.由于XMLParser使用的是Resources.Load()方法加载资源，因此配置需要放到Resources文件夹下，且读取时不能输入文件扩展名，例如：要读取"test.xml"，则需要parser.loadFile( "test" )
	
	
*/
namespace DataSection
{
	public class XMLSection : SectionNode<XMLSection>
	{
		private Dictionary<string, string> attrs_ = new Dictionary<string, string>();
		private string filename_ = "";

		public XMLSection() {}
		public XMLSection( string name = "", string value = "", XMLSection parent = null ) : base( name, value, parent ) {}

		public string filename
		{
			get {
				return filename_;
			}
			set {
				filename_ = value;
			}
		}

		public Dictionary<string, string> attrs
		{
			get {
				return attrs_;
			}
		}
	}


	class XMLParser : SmallXmlParser, SmallXmlParser.IContentHandler
	{
		
		private XMLSection root;
		
		public XMLParser () : base ()
		{
			stack = new Stack ();
		}
		
		public XMLSection loadFile( string file )
		{
			return loadXML( Resources.Load( file ).ToString() );
		}

		public XMLSection loadXML (string xml)
		{
			root = null;
			Parse (new StringReader (xml), this);
			return root;
		}
		
		// IContentHandler
		
		private XMLSection current;
		private Stack stack;
		
		public void OnStartParsing (SmallXmlParser parser) {}
		
		public void OnProcessingInstruction (string name, string text) {}
		
		public void OnIgnorableWhitespace (string s) {}
		
		public void OnStartElement (string name, SmallXmlParser.IAttrList attrs)
		{
			if (root == null)
			{
				root = new XMLSection (name);
				current = root;
			}
			else
			{
				XMLSection parent = (XMLSection) stack.Peek ();
				current = parent.createSection(name);
			}
			stack.Push (current);
			// attributes
			int n = attrs.Length;
			for (int i=0; i < n; i++)
				current.attrs[attrs.GetName(i)] = attrs.GetValue(i);
		}
		
		public void OnEndElement (string name)
		{
			current = (XMLSection) stack.Pop ();
		}
		
		public void OnChars (string ch)
		{
			current.value = ch;
		}
		
		public void OnEndParsing (SmallXmlParser parser) {}
	}

}
