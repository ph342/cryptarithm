namespace Alphametiken
{
    class Word
    {
        private char[] characters;

        public Word(string word)
        {
            characters = word.ToCharArray();
        }

        public char[] getCharacters()
        {
            return characters;
        }
    }
}
