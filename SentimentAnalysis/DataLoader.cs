using CsvHelper;
using CsvHelper.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace SentimentAnalysis
{
    public static class DataLoader
    {
        public static readonly string DATA_DIRECTORY = "data";

        public static List<Comment> LoadCommentsFromCsvFile()
        {
            /*
             * TODO: 현재 System.Text.Json이 이모지 코드포인트를 지원하지 않음.
             * 추후 지원될 경우 csv 의존성 제거하기
             */
            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            List<Comment> comments = new();

            using (StreamReader reader = new(Path.Combine(DATA_DIRECTORY, "comments.csv")))
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

        public static HashSet<string> LoadNouns()
        {
            return loadWordsFromJson("nouns.json");
        }

        public static HashSet<string> LoadPostPositions()
        {
            return loadWordsFromJson("post_positions.json");
        }

        public static HashSet<string> LoadEndings()
        {
            return loadWordsFromJson("endings.json");
        }

        public static Dictionary<string, int> LoadPolarityScores()
        {
            string path = Path.Combine(DATA_DIRECTORY, "polarity_scores.json");
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Dictionary<string, int>>(json, options);
        }

        private static readonly JsonSerializerOptions options = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        };

        private static HashSet<string> loadWordsFromJson(string fileName)
        {
            string json = File.ReadAllText(Path.Combine(DATA_DIRECTORY, fileName));
            var words = JsonSerializer.Deserialize<List<string>>(json, options);

            return new HashSet<string>(words);
        }
    }
}
