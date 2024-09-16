using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Penyata.Tools
{
	public static class XmlTools
	{
		public static string ToXmlString<T>(this T input)
		{
			using (var writer = new StringWriter()) {
				input.ToXml(writer);
				return writer.ToString();
			}
		}
		public static void ToXml<T>(this T objectToSerialize, Stream stream)
		{
			new XmlSerializer(typeof(T)).Serialize(stream, objectToSerialize);
		}

		public static void ToXml<T>(this T objectToSerialize, StringWriter writer)
		{
			new XmlSerializer(typeof(T)).Serialize(writer, objectToSerialize);
		}
		public static string XmlSerializeToString(this object objectInstance)
		{
			var serializer = new XmlSerializer(objectInstance.GetType());
			var sb = new StringBuilder();
	
			using (TextWriter writer = new StringWriter(sb)) {
				serializer.Serialize(writer, objectInstance);
			}
	
			return sb.ToString();
		}
	
		public static T XmlDeserializeFromString<T>(this string objectData)
		{
			return (T)XmlDeserializeFromString(objectData, typeof(T));
		}
	
		public static object XmlDeserializeFromString(this string objectData, Type type)
		{
			var serializer = new XmlSerializer(type);
			object result;
	
			using (TextReader reader = new StringReader(objectData)) {
				result = serializer.Deserialize(reader);
			}
	
			return result;
		}
	}
}