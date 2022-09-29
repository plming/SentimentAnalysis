using CsvHelper;
using CsvHelper.Configuration;
using System.Diagnostics;
using System.Globalization;

namespace SentimentAnalysis
{
    /// <summary>
    /// 파일로부터 데이터셋을 불러옵니다.
    /// </summary>
    public static class DataLoader
    {
        /// <summary>
        /// DataLoader 클래스가 읽을 파일이 위치하는 디렉토리 경로
        /// </summary>
        public static string DATA_DIR_PATH { get; } = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "data");

        /// <summary>
        /// csv 파일로부터 댓글을 불러옵니다.
        /// 컬럼은 label, text 순서로 있어야 합니다.
        /// label은 P 또는 N이여야만 합니다.
        /// </summary>
        /// <param name="path">csv 파일의 경로</param>
        /// <returns>파일에서 불러온 댓글 목록</returns>
        public static List<Comment> LoadCommentsFromCsvFile(string path)
        {
            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            List<Comment> comments = new();

            using (StreamReader reader = new(path))
            using (CsvReader csv = new(reader, config))
            {
                while (csv.Read())
                {
                    bool labelExists = csv.TryGetField(0, out string firstColumn);
                    Debug.Assert(labelExists);

                    bool textExists = csv.TryGetField(1, out string secondColumn);
                    Debug.Assert(textExists);

                    Label label;
                    switch (firstColumn)
                    {
                        case "P":
                            label = Label.POSITIVE;
                            break;
                        case "N":
                            label = Label.NEAGTIVE;
                            break;
                        default:
                            Debug.Fail("unknown label");
                            throw new InvalidDataException("unknown label");
                    }

                    comments.Add(new Comment(secondColumn, label));
                }
            }

            return comments;
        }

        /// <summary>
        /// csv 파일로부터 단어 집합을 불러옵니다. 컬럼은 word 1개만 있어야 합니다.
        /// </summary>
        /// <param name="path">csv 파일의 경로</param>
        /// <returns>파일에서 불러온 단어 집합</returns>
        public static HashSet<string> LoadWordsFromCsvFile(string path)
        {
            HashSet<string> words = new();

            foreach (string line in File.ReadLines(path))
            {
                const string BLANK = " ";
                Debug.Assert(line.Split(BLANK).Length == 1, "한 줄에 여러 단어가 존재합니다.");
                Debug.Assert(!words.Contains(line), "파일에 중복된 단어가 존재합니다.");

                words.Add(line);
            }

            return words;
        }

        /// <summary>
        /// csv 파일로부터 극성 점수 사전을 불러옵니다.
        /// </summary>
        /// <param name="path">csv 파일의 경로</param>
        /// <returns>단어를 key, 1이상 5이하의 점수를 value로 가지는 사전</returns>
        public static Dictionary<string, int> LoadPolarityScoresFromCsvFile(string path)
        {
            Dictionary<string, int> polarityScores = new Dictionary<string, int>();

            foreach (string line in File.ReadLines(path))
            {
                string[] splits = line.Split(',');
                Debug.Assert(splits.Length == 2, "컬럼 갯수가 맞지 않습니다.");

                string word = splits[0].Trim();
                Debug.Assert(word == splits[0], "단어에 공백이 포함되어 있습니다");

                bool isInteger = int.TryParse(splits[1], out int score);
                Debug.Assert(isInteger);

                const int BEST_SCORE = 5;
                const int WORST_SCORE = 1;
                Debug.Assert(score >= WORST_SCORE && score <= BEST_SCORE, "유효한 점수 범위를 벗어납니다.");

                polarityScores.Add(word, score);
            }

            return polarityScores;
        }
    }
}
