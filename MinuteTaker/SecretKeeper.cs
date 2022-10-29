using System;
using System.Linq;
using System.Text;

namespace MinuteTaker
{
    public class SecretKeeper
    {
        private const string PlainText = $"!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz";

        private const string CipherText = $"z0_.J6WH^4'8kr5Pcs>S\\`XGBo/a&jM<RId*Fup1wCDx#Tq,2g]\"%@N:iEVt(l7UO)?mQ-bZn!y+Y;v=L$3Kef9[hA";

        public static string EncyptText(string text, int returnLenght = 64)
        {
            StringBuilder toEncrpt = new StringBuilder();
            string pt = BuildPlainText().Substring(0, returnLenght);

            int len = text.Length;
            int[] pos = new int[len + 1];

            if (returnLenght > PlainText.Length)
            {
                returnLenght = PlainText.Length;
            }

            // Find the hiding locations
            for (int i = 0; i < (len + 1); i++)
            {
                pos[i] = GetUnique(len, (pt.Length - 1), pos);
            }

            toEncrpt = new(HideText(pt, pos, text));

            toEncrpt = HideArray(toEncrpt.ToString(), pos);

            // EnCiper Text
            string output = CaesarCipher(PlainText, CipherText, toEncrpt.ToString());

            return output;
        }

        public static string DecyptText(string text)
        {
            // DeCiper Text
            string temp = CaesarCipher(CipherText, PlainText, text);

            int[] pos = RecoverArray(temp);

            string output = RecoverText(temp, pos);

            return output;
        }

        // *******************************************************************
        private static string BuildPlainText()
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());

            string temp = PlainText;
            string output = "";

            do
            {
                int l = temp.Length;
                int j = rand.Next(l);
                output += temp[j];

                string temp2 = temp.Substring(0, j);
                string temp3 = temp.Substring((j + 1), (l - j - 1));
                temp = temp2 + temp3;
            } while (temp.Length > 0);

            return output;
        }

        private static int GetUnique(int low, int high, int[] filled)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int output = 0;

            do
            {
                output = rand.Next(low, high);
            } while (filled.Contains(output));

            return output;
        }

        private static string CaesarCipher(string from, string too, string input)
        {
            StringBuilder output = new();

            for (int i = 0; i < input.Length; i++)
            {
                string c = input.Substring(i, 1);
                int j = from.IndexOf(c);
                output.Append(too.Substring(j, 1));
            }

            return output.ToString();
        }

        private static string HideText(string pt, int[] pos, string text)
        {
            string tempText = pt;

            for (int i = 0; i < pos.Length - 1; i++)
            {
                int j = pos[i];
                string start = tempText.Substring(0, j);
                string middle = Convert.ToString(text.Substring(i, 1));
                string ending = tempText.Substring(j + 1, (tempText.Length - (j + 1)));

                tempText = start + middle + ending;
            }

            return tempText;
        }

        private static StringBuilder HideArray(string tempText, int[] pos)
        {
            int len = tempText.Length;

            StringBuilder s = new();
            for (int i = 0; i < pos.Length; i++)
            {
                s.Append(Convert.ToChar(pos[i] + 33));
            }

            string m = tempText.Substring(s.Length, (tempText.Length - s.Length - 1));

            // Hide the length
            char e = Convert.ToChar(pos.Length + 32);

            return new(s + m + e);
        }

        private static int[] RecoverArray(string temp)
        {
            // recover the length
            string l = temp.Substring(temp.Length - 1, 1);
            char c = Convert.ToChar(l);
            int len = Convert.ToUInt16(c);
            len = len - 33;

            int[] pos = new int[len];
            // recover the Array
            for (int i = 0; i < (pos.Length); i++)
            {
                pos[i] = Convert.ToInt16(temp[i]);
                pos[i] -= 33;
            }

            return pos;
        }

        private static string RecoverText(string temp, int[] pos)
        {
            StringBuilder output = new();
            for (int i = 0; i < pos.Length; i++)
            {
                string f = temp.Substring(pos[i], 1);
                output.Append(f);
            }

            return output.ToString();
        }
    }
}
