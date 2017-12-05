using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GraphicalConstraintProgramming
{
    class StringFormatTools
    {
        
        public static string subtext(int i)
        {
            if (i < 0) throw new ArgumentException();

            string output = "";

            for (int l = 0; l < i.ToString().Length; l++)
            {
                char currentchar = i.ToString().ToCharArray()[l];

                output += "\\u208" + currentchar;
            }

            var stringBuilder = new StringBuilder();
            foreach (Match match in Regex.Matches(output, @"\\u(?<Value>[a-zA-Z0-9]{4})"))
            {
                stringBuilder.AppendFormat(@"{0}",
                                           (Char)int.Parse(match.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber));
            }

            return stringBuilder.ToString();
        }
    }
}
