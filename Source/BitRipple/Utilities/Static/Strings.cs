using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Utilities
{
	public static class Strings
	{
		public static string ReplaceCharacters(this string replace, string characters, string with)
		{
			string[] temp;

			temp = replace.Split(characters.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			return String.Join(with, temp);
		}

		public static string RemoveDiacritics(this string text)
		{
			var normalizedString = text.Normalize(NormalizationForm.FormD);
			StringBuilder sb = new StringBuilder();

			foreach (var c in normalizedString)
			{
				var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
				if (unicodeCategory != UnicodeCategory.NonSpacingMark)
				{
					sb.Append(c);
				}
			}

			return sb.ToString().Normalize(NormalizationForm.FormC);
		}

		public static string ToStringExpanded(this object obj)
		{
			if (obj == null) { return "[null]"; }

			var list = new List<string>();

			foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
			{
				string name = descriptor.Name;
				object value = descriptor.GetValue(obj);
				if (value == null)
				{
					list.Add(String.Format("{0}={1}", name, "null"));
				}
				else
				{
					Type type = value.GetType();
					string parsed = type.IsPrimitive || type == typeof(Decimal) || type == typeof(String) ? value.ToString() : value.ToStringExpanded();
					list.Add(String.Format("{0}={1}", name, parsed));
				}
			}

			return "[" + String.Join(", ", list.ToArray()) + "]";
		}
	}
}
