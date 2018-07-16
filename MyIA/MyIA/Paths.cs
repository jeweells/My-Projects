using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;

namespace MyIA
{
	public static class Paths
	{
		public static class StopPhrases
		{
			public static string Normals = "StopPhrases/Normals";
		}
		public static class Sentences {
			public static string Subjects = "Sentences/Subjects";
			public static string Verbs = "Sentences/Verbs";
			public static string Objects = "Sentences/Objects";
			public static string Complements = "Sentences/Complements";
		}
		public static class Greetings
		{
			public static string Normals = "Greetings/Normals";
		}
		public static string GetInCurrentDirectory(this string name)
		{
			return $"{Environment.CurrentDirectory}/{name}";
		}
		public static string GetInDataDirectory(this string name)
		{
			return $"{Environment.CurrentDirectory}/Data/{name}";
		}
		static void CreatePathsAux(Type t)
		{
			foreach (MemberInfo mi in t.GetTypeInfo().DeclaredMembers)
			{
				if (mi.MemberType == MemberTypes.NestedType)
				{
					CreatePathsAux(Type.GetType($"{t}+{mi.Name}"));
				}
			}
			FieldInfo[] fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo field in fields)
			{
				//Console.WriteLine($"{t} =>"+field.GetValue(null).ToString().GetInDataDirectory());
				Directory.CreateDirectory(field.GetValue(null).ToString().GetInDataDirectory());
			}
		}
		public static void CreatePaths()
		{
			CreatePathsAux(typeof(Paths));
		}
		public static string CleanFileName(this string str)
		{
			string nstr = str;
			foreach (var item in Path.GetInvalidFileNameChars())
			{
				nstr = nstr.Replace(item, '-');
			}
			return nstr;
		}
	}
}
