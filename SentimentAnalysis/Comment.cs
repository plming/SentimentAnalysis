namespace SentimentAnalysis
{
    public class Comment
    {
        /// <summary>
        /// 댓글의 내용
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// 댓글의 레이블
        /// </summary>
        public Label Label { get; private set; }

        /// <summary>
        /// 댓글 내 단어 집합을 반환합니다.
        /// </summary>
        /// <returns>댓글 내 단어 집합</returns>
        public HashSet<string> Words
        {
            get => new(wordFrequency.Keys);
        }

        private readonly Dictionary<string, int> wordFrequency = new();

        /// <summary>
        /// 댓글 객체를 초기화합니다.
        /// </summary>
        /// <param name="text">댓글의 내용</param>
        /// <param name="label">댓글의 레이블</param>
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

        /// <summary>
        /// 댓글 내 단어의 빈도수를 반환합니다.
        /// </summary>
        /// <param name="word">빈도수를 조회할 단어</param>
        /// <returns>단어의 빈도수를 반환합니다. 해당 단어가 없을 경우 0을 반환합니다.</returns>
        public int GetFrequency(string word)
        {
            return wordFrequency.GetValueOrDefault(word, 0);
        }
    }
}
