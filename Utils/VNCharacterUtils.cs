using System;
using System.Collections.Generic;
using System.Text;

namespace JobExchange.Utils
{
	public class VNCharacterUtils
	{
		private static readonly Dictionary<char, char> AccentCharacters = new Dictionary<char, char>
		{
			{'À', 'A'}, {'Á', 'A'}, {'Â', 'A'}, {'Ã', 'A'}, {'È', 'E'}, {'É', 'E'},
			{'Ê', 'E'}, {'Ì', 'I'}, {'Í', 'I'}, {'Ò', 'O'}, {'Ó', 'O'}, {'Ô', 'O'},
			{'Õ', 'O'}, {'Ù', 'U'}, {'Ú', 'U'}, {'Ý', 'Y'}, {'à', 'a'}, {'á', 'a'},
			{'â', 'a'}, {'ã', 'a'}, {'è', 'e'}, {'é', 'e'}, {'ê', 'e'}, {'ì', 'i'},
			{'í', 'i'}, {'ò', 'o'}, {'ó', 'o'}, {'ô', 'o'}, {'õ', 'o'}, {'ù', 'u'},
			{'ú', 'u'}, {'ý', 'y'}, {'Ă', 'A'}, {'ă', 'a'}, {'Đ', 'D'}, {'đ', 'd'},
			{'Ĩ', 'I'}, {'ĩ', 'i'}, {'Ũ', 'U'}, {'ũ', 'u'}, {'Ơ', 'O'}, {'ơ', 'o'},
			{'Ư', 'U'}, {'ư', 'u'}, {'Ạ', 'A'}, {'ạ', 'a'}, {'Ả', 'A'}, {'ả', 'a'},
            // Add more characters as needed
        };

		public static string RemoveAccent(string str)
		{
			StringBuilder sb = new StringBuilder(str);
			for (int i = 0; i < sb.Length; i++)
			{
				char ch = sb[i];
				if (AccentCharacters.TryGetValue(ch, out char replacement))
				{
					sb[i] = replacement;
				}
			}
			return sb.ToString();
		}

		public string ToSlug(string name)
		{
			string nameConvert = RemoveAccent(name.Trim().ToLower());
			string code = string.Join("-", nameConvert.Split(' ', StringSplitOptions.RemoveEmptyEntries));
			return code;
		}
	}
}
