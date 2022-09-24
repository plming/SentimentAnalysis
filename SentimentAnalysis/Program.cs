using System.Diagnostics;

namespace SentimentAnalysis
{
    public class Program
    {
        public static void Main()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<Comment> trainDataSet = DataLoader.LoadCommentsFromCsvFile(Path.Combine(DataLoader.DATA_DIR_PATH, "train.csv"));
            List<Comment> testDataSet = DataLoader.LoadCommentsFromCsvFile(Path.Combine(DataLoader.DATA_DIR_PATH, "test.csv"));

            Knn model = new(trainDataSet);

            int numMatched = 0;
            for (int i = 0; i < testDataSet.Count; i++)
            {
                Label predict = model.Predict(testDataSet[i]);

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