using System.Diagnostics;

namespace SentimentAnalysis
{
    public static class Tokenizer
    {
        private static readonly HashSet<string> noun = DataLoader.LoadWordsFromCsvFile(Path.Combine(DataLoader.DATA_DIR_PATH, "noun.csv"));
        private static readonly HashSet<string> postPosition = DataLoader.LoadWordsFromCsvFile(Path.Combine(DataLoader.DATA_DIR_PATH, "postPosition.csv"));
        private static readonly HashSet<string> verb = DataLoader.LoadWordsFromCsvFile(Path.Combine(DataLoader.DATA_DIR_PATH, "joyolist.csv"));

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
    }
}
