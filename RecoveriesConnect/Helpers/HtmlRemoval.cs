using System;
using System.Text.RegularExpressions;

namespace RecoveriesConnect.Helpers
{

	/// <summary>
	/// Methods to remove HTML from strings.
	/// </summary>
	public static class HtmlRemoval
	{
		/// <summary>
		/// Remove HTML from string with Regex.
		/// </summary>
		public static string StripTagsRegex(string source)
		{
			return Regex.Replace(source, "<.*?>", string.Empty);
		}

		/// <summary>
		/// Compiled regular expression for performance.
		/// </summary>
		static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

		/// <summary>
		/// Remove HTML from string with compiled Regex.
		/// </summary>
		public static string StripTagsRegexCompiled(string source)
		{
			return _htmlRegex.Replace(source, string.Empty);
		}

		/// <summary>
		/// Remove HTML tags from string using char array.
		/// </summary>
		public static string StripTagsCharArray(string source)
		{
			char[] array = new char[source.Length];
			int arrayIndex = 0;
			bool inside = false;

			for (int i = 0; i < source.Length; i++)
			{
				char let = source[i];
				if (let == '<')
				{
					inside = true;
					continue;
				}
				if (let == '>')
				{
					inside = false;
					continue;
				}
				if (!inside)
				{
					array[arrayIndex] = let;
					arrayIndex++;
				}
			}
			return new string(array, 0, arrayIndex);
		}


		public static string RemoveHTMLTags(string content)
		{
			var cleaned = string.Empty;
			try
			{
				string textOnly = content;
				textOnly = textOnly.Replace("&lt;", string.Empty);
				textOnly = textOnly.Replace("/p&gt;", string.Empty);
				textOnly = textOnly.Replace("p&gt;", string.Empty);
				textOnly = textOnly.Replace("&amp;", string.Empty);
				textOnly = textOnly.Replace("nbsp;", string.Empty);
				textOnly = textOnly.Replace("/br", string.Empty);
				textOnly = textOnly.Replace("/&gt;", string.Empty);
				textOnly = textOnly.Replace("&gt;", string.Empty);
				textOnly = textOnly.Replace("br", string.Empty);
				textOnly = textOnly.Replace("<p>", string.Empty);
				textOnly = textOnly.Replace("/span", string.Empty);
				textOnly = textOnly.Replace("span style", string.Empty);
				textOnly = textOnly.Replace("< />", " ");

				cleaned = textOnly;
			}
			catch
			{
				//A tag is probably not closed. fallback to regex string clean.

			}

			return cleaned;
		}
	}
}

