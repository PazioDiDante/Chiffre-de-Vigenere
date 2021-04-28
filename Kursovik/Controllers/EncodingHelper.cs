using System;
using System.Collections.Generic;

namespace Kursovik.Controllers
{
    public static class EncodingHelper
    {
        public static Dictionary<char, int> russianLetter = new Dictionary<char, int>() { { 'а', 0 }, { 'б', 1 }, { 'в', 2 }, { 'г', 3 }, { 'д', 4 }, { 'е', 5 }, { 'ё', 6 }, { 'ж', 7 }, { 'з', 8 }, { 'и', 9 }, { 'й', 10 }, { 'к', 11 }, { 'л', 12 }, { 'м', 13 }, { 'н', 14 }, { 'о', 15 }, { 'п', 16 }, { 'р', 17 }, { 'с', 18 }, { 'т', 19 }, { 'у', 20 }, { 'ф', 21 }, { 'х', 22 }, { 'ц', 23 }, { 'ч', 24 }, { 'ш', 25 }, { 'щ', 26 }, { 'ъ', 27 }, { 'ы', 28 }, { 'ь', 29 }, { 'э', 30 }, { 'ю', 31 }, { 'я', 32 } };
        public static Dictionary<char, int> englishLetter = new Dictionary<char, int>() { { 'a', 0 }, { 'b', 1 }, { 'c', 2 }, { 'd', 3 }, { 'e', 4 }, { 'f', 5 }, { 'g', 6 }, { 'h', 7 }, { 'i', 8 }, { 'j', 9 }, { 'k', 10 }, { 'l', 11 }, { 'm', 12 }, { 'n', 13 }, { 'o', 14 }, { 'p', 15 }, { 'q', 16 }, { 'r', 17 }, { 's', 18 }, { 't', 19 }, { 'u', 20 }, { 'v', 21 }, { 'w', 22 }, { 'x', 23 }, { 'y', 24 }, { 'z', 25 } };
        public static char[,] VigenereSquare = new char[33, 33];
        public static void GenerateVigenereSquare(string language, string rot)
        {
            int j;
            int jj;
            Dictionary<char, int> letters;
            if (language == "rus") letters = russianLetter;
            else letters = englishLetter;

            if (rot == "ROT1") j = letters.Count - 1;
            else j = letters.Count;

            jj = j;
            for (int i = 0; i < letters.Count; i++)
            {
                foreach (var item in letters)
                {
                    if (jj > letters.Count - 1)
                    {
                        VigenereSquare[i, jj - letters.Count] = item.Key;
                    }
                    else
                    {
                        VigenereSquare[i, jj] = item.Key;
                    }
                    jj++;
                }
                j--;
                jj = j;
            }
        }
        public static string Encryption(string orginalText, string key, string language)
        {
            string lowerKey = key.ToLower();
            Dictionary<char, int> letters;
            if (language == "rus")
            {
                letters = russianLetter;
            }
            else
            {
                letters = englishLetter;
            }
            string lowerText = orginalText.ToLower();
            string res = "";
            int keyPosition = 0;
            for (int i = 0; i < orginalText.Length; i++)
            {
                if (letters.ContainsKey(lowerText[i]))
                {
                    if (char.IsUpper(orginalText[i]))
                    {
                        res += VigenereSquare[letters[lowerKey[keyPosition]], letters[lowerText[i]]].ToString().ToUpper();
                    }
                    else
                    {
                        res += VigenereSquare[letters[lowerKey[keyPosition]], letters[lowerText[i]]];
                    }
                    keyPosition++;
                    if (keyPosition == key.Length)
                    {
                        keyPosition = 0;
                    }
                }
                else
                {
                    res += orginalText[i];
                }
            }
            return res;
        }
        public static string Decryption(string encryptedText, string key, string language)
        {
            Dictionary<char, int> letters;
            string lowerKey = key.ToLower();
            if (language == "rus")
            {
                letters = russianLetter;
            }
            else
            {
                letters = englishLetter;
            }
            string lowerText = encryptedText.ToLower();
            string res = "";
            int keyPosition = 0;
            for (int i = 0; i < encryptedText.Length; i++)
            {
                if (letters.ContainsKey(lowerText[i]))
                {
                    for (int j = 0; j < 33; j++)
                    {
                        if (VigenereSquare[letters[lowerKey[keyPosition]], j] == lowerText[i])
                        {
                            foreach (var letter in letters)
                            {
                                if (letter.Value == j)
                                {
                                    if (char.IsUpper(encryptedText[i]))
                                    {
                                        res += letter.Key.ToString().ToUpper();
                                    }
                                    else
                                    {
                                        res += letter.Key;
                                    }

                                    keyPosition++;
                                    if (keyPosition == lowerKey.Length)
                                    {
                                        keyPosition = 0;
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                else
                {
                    res += encryptedText[i];
                }
            }
            return res;
        }
    }
}
