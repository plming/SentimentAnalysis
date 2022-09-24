using CsvHelper;
using CsvHelper.Configuration;
using System.Diagnostics;
using System.Globalization;

namespace SentimentAnalysis
{
    public static class DataLoader
    {
        public static string DATA_DIR_PATH { get; } = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "data");

        /// <summary>
        /// csv 파일로부터 댓글을 불러옵니다.
        /// csv 파일의 컬럼은 label, text 순서입니다.
        /// </summary>
        /// <param name="path">csv 파일의 경로</param>
        /// <returns></returns>
        public static List<Comment> LoadCommentsFromFile(string path)
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
                    bool labelExists = csv.TryGetField(0, out string labelString);
                    Debug.Assert(labelExists);

                    bool textExists = csv.TryGetField(1, out string text);
                    Debug.Assert(textExists);

                    Label label;
                    switch (labelString)
                    {
                        case "P":
                            label = Label.POSITIVE;
                            break;
                        case "N":
                            label = Label.NEAGTIVE;
                            break;
                        default:
                            Debug.Fail("unknown label");
                            throw new InvalidDataException();
                    }

                    comments.Add(new Comment(text, label));
                }
            }

            return comments;
        }
    }
}
