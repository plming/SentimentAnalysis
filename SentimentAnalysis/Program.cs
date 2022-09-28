using System.Diagnostics;

namespace SentimentAnalysis
{
    public class Program
    {
        public static void Main()
        {
            Random random = new Random();
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<Comment> comments = DataLoader.LoadCommentsFromCsvFile(@"");
     
            comments = comments.OrderBy(_ => random.Next()).ToList();
            Console.WriteLine("댓글 데이터를 섞었습니다");

            var positiveComments = comments.Where(c => c.Label == Label.POSITIVE);
            var negativeComments = comments.Where(c => c.Label == Label.NEAGTIVE);

            int minCount = Math.Min(positiveComments.Count(), negativeComments.Count());

            #region SPLIT_INTO_TRAIN_TEST
            const float TRAIN_RATIO = 0.8F;
            List<Comment> trainDataSet = new List<Comment>();
            List<Comment> testDataSet = new List<Comment>();
            for (int i = 0; i < comments.Count; i++)
            {
                if (i < comments.Count * TRAIN_RATIO)
                {
                    trainDataSet.Add(comments[i]);
                }
                else
                {
                    testDataSet.Add(comments[i]);
                }
            }
            #endregion

            Model knnModel = new KnnModel(trainDataSet);
            Model scoreModel = new ScoreModel();

            int numMatched = 0;
            for (int i = 0; i < testDataSet.Count; i++)
            {
                Label predict = knnModel.Predict(testDataSet[i]);

                if (testDataSet[i].Label == predict)
                {
                    numMatched++;
                }
                else
                {
                    Console.WriteLine($"#{i} 예측: {predict} / 실제: {testDataSet[i].Label}");
                }
            }
            stopwatch.Stop();

            Console.WriteLine($"정확도(%): {(double)numMatched / testDataSet.Count * 100}");
            Console.WriteLine($"수행 시간: {stopwatch.Elapsed}");
        }
    }
}