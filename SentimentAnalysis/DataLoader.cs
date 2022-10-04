using CsvHelper;
using CsvHelper.Configuration;
using System.Diagnostics;
using System.Globalization;

namespace SentimentAnalysis
{
    public static class DataLoader
    {
        public static string DATA_DIR_PATH { get; } = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "data");

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

        public static Dictionary<string, int> LoadPolarityScoresFromCsvFile(string path)
        {
            Dictionary<string, int> polarityScores = new();

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
