using System.Diagnostics;

namespace SentimentAnalysis
{
    public static class Tokenizer
    {
        private const string DATA_DIRECTORY_PATH = @"C:\Users\kim01\source\repos\SentimentAnalysis\SentimentAnalysis\data";
        private static readonly HashSet<string> noun = new();
        private static readonly HashSet<string> postPosition = new();
        private static readonly HashSet<string> verb = new();

        static Tokenizer()
        {
            initializeSetByFile(noun, Path.Combine(DATA_DIRECTORY_PATH, "noun.csv"));
            initializeSetByFile(postPosition, Path.Combine(DATA_DIRECTORY_PATH, "postPosition.csv"));
            initializeSetByFile(verb, Path.Combine(DATA_DIRECTORY_PATH, "joyolist.csv"));
        }

        public static List<string> ExtractNouns(string word)
        {
            Debug.Assert(word.Split(" ").Length == 1);

            if (noun.Contains(word))
            {
                return new() { word };
            }

            #region SplitTwo
            for (int i = word.Length - 1; i > 0; --i)
            {
                string front = word[..i];
                string rear = word[i..];

                if (noun.Contains(front) && verb.Contains(rear))
                {
                    return new() { front };
                }

                if (noun.Contains(front) && noun.Contains(rear))
                {
                    return new() { front, rear };
                }

                if (noun.Contains(front) && postPosition.Contains(rear))
                {
                    return new() { front };
                }
            }
            #endregion

            #region SplitThree
            for (int i = 1; i < word.Length - 1; i++)
            {
                for (int j = i + 1; j < word.Length; j++)
                {
                    string front = word[..i];
                    string mid = word[i..j];
                    string rear = word[j..];

                    if (noun.Contains(front) && noun.Contains(mid) && postPosition.Contains(rear))
                    {
                        return new() { front, mid };
                    }
                }
            }
            #endregion

            return new();
        }

        private static void initializeSetByFile(HashSet<string> set, string path)
        {
            foreach (string line in File.ReadLines(path))
            {
                string word = line.Trim();
                Debug.Assert(!set.Contains(word));
                set.Add(word);
            }
        }
    }
}
