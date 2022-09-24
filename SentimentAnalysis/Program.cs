using System.Diagnostics;

namespace SentimentAnalysis
{
    public class Program
    {
        public static void Main()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<Comment> trainData = DataLoader.LoadCommentsFromFile(Path.Combine(DataLoader.DATA_DIR_PATH, "train.csv"));
            List<Comment> testData = DataLoader.LoadCommentsFromFile(Path.Combine(DataLoader.DATA_DIR_PATH, "test.csv"));

            Knn model = new(trainData);

            int numMatched = 0;
            for (int i = 0; i < testData.Count; i++)
            {
                Label predict = model.Predict(testData[i]);

                if (testData[i].Label == predict)
                {
                    numMatched++;
                }
                else
                {
                    Console.WriteLine($"#{i} 예측: {predict} / 실제: {testData[i].Label}");
                }
            }
            stopwatch.Stop();

            Console.WriteLine($"정확도(%): {(double)numMatched / testData.Count * 100}");
            Console.WriteLine($"수행 시간: {stopwatch.Elapsed}");
        }
    }
}