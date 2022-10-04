using System.Diagnostics;

namespace SentimentAnalysis
{
    public class CommentRepository
    {
        private readonly List<Comment> comments;

        public CommentRepository(string path)
        {
            Random random = new();

            // load commments and shuffle
            comments = DataLoader.LoadCommentsFromCsvFile(path)
                                 .OrderBy(_ => random.Next())
                                 .ToList();

        }

        public void Split(double ratio, out List<Comment> trainData, out List<Comment> testData)
        {
            Debug.Assert(ratio >= 0 && ratio <= 1);

            trainData = new List<Comment>();
            testData = new List<Comment>();

            for (int i = 0; i < comments.Count; i++)
            {
                if (i < comments.Count * ratio)
                {
                    trainData.Add(comments[i]);
                }
                else
                {
                    testData.Add(comments[i]);
                }
            }

            Debug.Assert(trainData.Count + testData.Count == comments.Count);
        }

        public int CountByLabel(Label label)
        {
            return comments.Where(c => c.Label == label).Count();
        }
    }
}

