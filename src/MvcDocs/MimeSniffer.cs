using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace MvcDocs
{
	/// <summary>
	/// Utility class for detecting MIME or Content types from files and common file extensions.
	/// </summary>
	public static class MimeSniffer
	{
		[DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
		private extern static UInt32 FindMimeFromData(
			UInt32 pBC,
			[MarshalAs(UnmanagedType.LPStr)] String pwzUrl,
			[MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
			UInt32 cbSize,
			[MarshalAs(UnmanagedType.LPStr)] String pwzMimeProposed,
			UInt32 dwMimeFlags,
			out UInt32 ppwzMimeOut,
			UInt32 dwReserverd
		);

		/// <summary>
		/// The different methods that can be used to find content type information
		/// </summary>
		public enum SniffMethods
		{
			Lookup,
			Registry,
			Content
		}

		private const int NumberOfBytesToRead = 256;
		private const string UnknownMimeType = "application/octet-stream";
		private const char MimeTypeDelimiter = '/';
		private const string MimeTypeLookupResourceLocation = "MvcDocs.mime.types";
		private const string MimeTypeLookupResourceCommentStart = "#";
		private const string MimeTypeLookupResourceDelimiter = "\t";
		private const char MimeTypeLookupResourceExtensionDelimiter = ' ';
		private const string MimeTypeLookupUrl = "https://svn.apache.org/repos/asf/httpd/httpd/trunk/docs/conf/mime.types";
		private static IDictionary<string, IEnumerable<string>> MimeLookup { get; set; }

		private static readonly SniffMethods[] DefaultSniffMethodOrder = new[]
			{SniffMethods.Lookup, SniffMethods.Registry, SniffMethods.Content};

		#region IsMimeType Methods

		public static bool IsType(string path, string type)
		{
			if (!IsValidMimeType(type))
				throw new ArgumentException(String.Format("'{0}' is not a vaid Mime Type", type), "type");
			return GetMimeTypeForPath(path).Equals(type, StringComparison.OrdinalIgnoreCase);
		}

		private static bool IsValidMimeType(string mimeType)
		{
			if (String.IsNullOrEmpty(mimeType))
				throw new ArgumentNullException("mimeType");
			var types = new[] {"application", "audio", "chemical", "image", "message", "model", "multipart", "text", "video"};
			return types.Contains(mimeType.ToLower());
		}

		public static bool IsApplication(string path)
		{
			return IsType(path, "application");
		}

		public static bool IsAudio(string path)
		{
			return IsType(path, "audio");
		}

		public static bool IsChemical(string path)
		{
			return IsType(path, "chemical");
		}

		public static bool IsImage(string path)
		{
			return IsType(path, "image");
		}

		public static bool IsMessage(string path)
		{
			return IsType(path, "message");
		}

		public static bool IsModel(string path)
		{
			return IsType(path, "model");
		}

		public static bool IsMultiPart(string path)
		{
			return IsType(path, "multipart");
		}

		public static bool IsText(string path)
		{
			return IsType(path, "text");
		}

		public static bool IsVideo(string path)
		{
			return IsType(path, "video");
		}

		#endregion

		/// <summary>
		/// Gets the MIME type for path. The type is the first identifier before the '/' delimiter and provides a high-level description of the type of file.
		/// For example, this can be 'application', 'image', or 'text'.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>A MIME type.</returns>
		public static string GetMimeTypeForPath(string path)
		{
			return GetMimeTypeForPath(path, DefaultSniffMethodOrder);
		}

		/// <summary>
		/// Gets the MIME type for path. The type is the first identifier before the '/' delimiter and provides a high-level description of the type of file.
		/// For example, this can be 'application', 'image', or 'text'.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="methods">The methods to use to detect the MIME type. The order the search is made is detemined by the order of the parameters.</param>
		/// <returns>A MIME type.</returns>
		public static string GetMimeTypeForPath(string path, params SniffMethods[] methods)
		{
			var mime = GetMimeForPath(path, methods);
			return mime.Substring(0, mime.IndexOf(MimeTypeDelimiter));
		}

		/// <summary>
		/// Gets the MIME for path.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>A MIME type and sub-type.</returns>
		public static string GetMimeForPath(string path)
		{
			return GetMimeForPath(path, DefaultSniffMethodOrder);
		}

		/// <summary>
		/// Gets the MIME for path.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="methods">The methods to use to detect the MIME type. The order the search is made is detemined by the order of the parameters.</param>
		/// <returns>A MIME type and sub-type.</returns>
		public static string GetMimeForPath(string path, params SniffMethods[] methods)
		{
			MimeSniffer.ThrowOnInvalidPath(path);
			MimeSniffer.ThrowOnInvalidSniffMethods(methods);
			var mimeType = MimeSniffer.UnknownMimeType;
			foreach (var method in methods)
			{
				switch (method)
				{
					case SniffMethods.Lookup:
						mimeType = MimeSniffer.GetMimeFromLookup(path);
						break;
					case SniffMethods.Registry:
						mimeType = MimeSniffer.GetMimeFromRegistry(path);
						break;
					case SniffMethods.Content:
						if (File.Exists(path))
							using (var fileStream = new FileStream(path, FileMode.Open))
								mimeType = MimeSniffer.GetMimeFromContent(fileStream);
						break;
				}
				if (mimeType != MimeSniffer.UnknownMimeType) return mimeType;
			}
			return mimeType;
		}

		/// <summary>
		/// Gets the MIME from a lookup list.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>A MIME type and sub-type.</returns>
		public static string GetMimeFromLookup(string path)
		{
			return MimeSniffer.GetMimeFromLookup(path, false);
		}

		/// <summary>
		/// Gets the MIME from a lookup list.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="checkOnline">if set to <c>true</c> check online for the latest MIME type list.</param>
		/// <returns>A MIME type and sub-type.</returns>
		public static string GetMimeFromLookup(string path, bool checkOnline)
		{
			MimeSniffer.ThrowOnInvalidPath(path);
			if (null == MimeSniffer.MimeLookup) MimeSniffer.InitialiseMimeLookup(checkOnline);
			var extension = Path.GetExtension(path);
			return null != MimeSniffer.MimeLookup && MimeSniffer.MimeLookup.ContainsKey(extension) ? MimeSniffer.MimeLookup[extension].FirstOrDefault() : MimeSniffer.UnknownMimeType;
		}

		/// <summary>
		/// Gets the extensions for MIME.
		/// </summary>
		/// <param name="mimeType">Type of the MIME.</param>
		/// <returns>A list of extensions associated with the given MIME type.</returns>
		public static IEnumerable<string> GetExtensionsForMime(string mimeType)
		{
			return MimeSniffer.GetExtensionsForMime(mimeType, false);
		}

		/// <summary>
		/// Gets the extensions for MIME.
		/// </summary>
		/// <param name="mimeType">Type of the MIME.</param>
		/// <param name="checkOnline">if set to <c>true</c> check online for the latest MIME type list.</param>
		/// <returns>A list of extensions associated with the given MIME type.</returns>
		public static IEnumerable<string> GetExtensionsForMime(string mimeType, bool checkOnline)
		{
			MimeSniffer.ThrowOnInvalidMimeType(mimeType);
			if (null == MimeSniffer.MimeLookup) MimeSniffer.InitialiseMimeLookup(checkOnline);
			return null != MimeSniffer.MimeLookup && MimeSniffer.MimeLookup.ContainsKey(mimeType) ? MimeSniffer.MimeLookup[mimeType] : new List<string>();
		}

		/// <summary>
		/// Gets the MIME from registry.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>A MIME type and sub-type.</returns>
		public static string GetMimeFromRegistry(string path)
		{
			MimeSniffer.ThrowOnInvalidPath(path);
			var extension = Path.GetExtension(path);
			if (!String.IsNullOrWhiteSpace(extension))
			{
				var key = Registry.ClassesRoot.OpenSubKey(extension.ToLower());
				if (null != key)
				{
					var value = key.GetValue("Content Type");
					if (null != value) return value.ToString();
				}
			}
			return MimeSniffer.UnknownMimeType;
		}

		/// <summary>
		/// Gets the content of the MIME by analyzing the stream data.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>A MIME type and sub-type.</returns>
		public static string GetMimeFromContent(Stream stream)
		{
			MimeSniffer.ThrowOnCannotReadStream(stream);
			var buffer = new byte[MimeSniffer.NumberOfBytesToRead];
			if (stream.Length >= MimeSniffer.NumberOfBytesToRead)
				stream.Read(buffer, 0, MimeSniffer.NumberOfBytesToRead);
			else
				stream.Read(buffer, 0, (int)stream.Length);	
			return MimeSniffer.GetMimeFromContent(buffer);
		}

		/// <summary>
		/// Gets the content of the MIME from data.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>A MIME type and sub-type.</returns>
		public static string GetMimeFromContent(byte[] data)
		{
			try
			{
				UInt32 mimetype;
				MimeSniffer.FindMimeFromData(0, null, data, MimeSniffer.NumberOfBytesToRead, null, 0, out mimetype, 0);
				var mimeTypePtr = new IntPtr(mimetype);
				var mime = Marshal.PtrToStringUni(mimeTypePtr);
				Marshal.FreeCoTaskMem(mimeTypePtr);
				return mime;
			}
			catch
			{
				return MimeSniffer.UnknownMimeType;
			}
		}

		#region Mime Lookup Methods

		private static void InitialiseMimeLookup(bool checkOnline)
		{
			MimeSniffer.MimeLookup = new Dictionary<string, IEnumerable<string>>();
			using (var reader = MimeSniffer.GetLookupStream(checkOnline))
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					if (!IsValidMimeTypeLookupLine(line)) continue;
					MimeSniffer.AddLookupLine(line);
				}
		}

		private static void AddLookupLine(string line)
		{
			var splitLine = line.Split(new string[] { MimeSniffer.MimeTypeLookupResourceDelimiter }, StringSplitOptions.RemoveEmptyEntries);
			if (splitLine.Length != 2) return;
			var mimeType = splitLine[0];
			var extensions = splitLine[1].Split(MimeSniffer.MimeTypeLookupResourceExtensionDelimiter);
			MimeSniffer.AddMimeTypeExtensions(mimeType, extensions);
			MimeSniffer.AddExtensionsMimeType(mimeType, extensions);
		}

		private static void AddMimeTypeExtensions(string mimeType, IEnumerable<string> extensions)
		{
			if (!MimeSniffer.MimeLookup.ContainsKey(mimeType))
				MimeSniffer.MimeLookup.Add(mimeType, extensions);
			else
				MimeSniffer.MimeLookup[mimeType] = MimeSniffer.MimeLookup[mimeType].Concat(extensions);
		}

		private static void AddExtensionsMimeType(string mimeType, IEnumerable<string> extensions)
		{
			foreach (var extension in extensions)
			{
				if (!MimeSniffer.MimeLookup.ContainsKey(extension))
					MimeSniffer.MimeLookup.Add(extension, new[] { mimeType });
				else
					MimeSniffer.MimeLookup[extension] = MimeSniffer.MimeLookup[extension].Concat(new[] { mimeType });
			}
		}

		private static StreamReader GetLookupStream(bool checkOnline)
		{
			if (checkOnline)
				try
				{
					using (var client = new WebClient())
						return new StreamReader(client.OpenRead(MimeSniffer.MimeTypeLookupUrl));
				}
				catch
				{
					// Todo Implement logging and reintroduce this warning.
					//Log.Warn("Unable to connect to MIME type list");
				}
			return new StreamReader(typeof(MimeSniffer).Assembly.GetManifestResourceStream(MimeSniffer.MimeTypeLookupResourceLocation));
		}

		private static bool IsValidMimeTypeLookupLine(string line)
		{
			return !String.IsNullOrWhiteSpace(line) && !line.StartsWith(MimeSniffer.MimeTypeLookupResourceCommentStart) && line.Contains(MimeSniffer.MimeTypeLookupResourceDelimiter);
		}

		#endregion MimeType Lookup Methods

		private static void ThrowOnInvalidPath(string path)
		{
			if (String.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path", "Path cannot be null or blank");
		}

		private static void ThrowOnInvalidSniffMethods(IEnumerable methods)
		{
			if (methods == null) throw new ArgumentNullException("methods", "Mime Sniff methods cannot be null or blank");
		}

		private static void ThrowOnInvalidMimeType(string mimeType)
		{
			if (String.IsNullOrWhiteSpace(mimeType)) throw new ArgumentNullException("mimeType", "MimeType cannot be null or blank");
		}

		private static void ThrowOnCannotReadStream(Stream stream)
		{
			if (!stream.CanRead) throw new InvalidOperationException("Unable to detect mime type for unreadable stream");
		}
	}

	public static class ArrayExtensions
	{
		public static T[] Truncate<T>(this T[] source, int length)
		{
			if (source.Length <= length) return source;
			T[] destination = new T[length];
			Array.Copy(source, destination, length);
			return destination;
		}
	}
}
