namespace SentimentAnalysis
{
    public class Comment
    {
        public string Text { get; private set; }

        public Label Label { get; private set; }

        public HashSet<string> Words
        {
            get => new(wordFrequency.Keys);
        }

        private readonly Dictionary<string, int> wordFrequency = new();

        public Comment(string text, Label label)
        {
            Text = text;
            Label = label;

            foreach (string word in text.Split(" "))
            {
                foreach (string noun in Tokenizer.ExtractNouns(word))
                {
                    if (wordFrequency.ContainsKey(noun))
                    {
                        wordFrequency[noun]++;
                    }
                    else
                    {
                        wordFrequency[noun] = 1;
                    }
                }
            }
        }

        public int GetFrequency(string word)
        {
            return wordFrequency.GetValueOrDefault(word, 0);
        }
    }
}
