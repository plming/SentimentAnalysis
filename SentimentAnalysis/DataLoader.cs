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
    }
}
