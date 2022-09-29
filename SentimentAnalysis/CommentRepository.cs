using System;
using System.Diagnostics;

namespace SentimentAnalysis
{
    public class CommentRepository
    {
        private List<Comment> comments;

        public CommentRepository(string path)
        {
            Random random = new();

            // load commments and shuffle
            comments = DataLoader.LoadCommentsFromCsvFile(path)
                                 .OrderBy(_ => random.Next())
                                 .ToList();

        }

        public bool TrySplit(double ratio, out List<Comment> trainData, out List<Comment> testData)
        {
            trainData = new();
            testData = new();

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

            return true;
        }

        public int Count(Label label)
        {
            return comments.Where(c => c.Label == label).Count();
        }
    }
}

