using System.IO;

namespace ChatterReborn.Utils
{
    public static class ExtendedStringUtils
    {

        public static void EliminateInvalidCharacters(ref string word)
        {
            foreach (char oldChar in Path.GetInvalidFileNameChars())
            {
                word = word.Replace(oldChar, '_');
            }
            foreach (char oldChar in Path.GetInvalidPathChars())
            {
                word = word.Replace(oldChar, '_');
            }
            word = word.Replace('-', '_');
            word = word.Replace(' ', '_');
        }
    }
}
