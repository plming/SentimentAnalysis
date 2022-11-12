using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace SentimentAnalysis
{
    public class Program
    {

        public static void Main()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();


            Console.WriteLine($"수행 시간: {stopwatch.Elapsed}");
            CommentRepository repository = new();


            Console.WriteLine($"수행 시간: {stopwatch.Elapsed}");
            repository.Split(ratio: 0.7, out var trainDataSet, out var testDataSet);


            Console.WriteLine($"수행 시간: {stopwatch.Elapsed}");
            Model[] models = { new KnnModel(trainDataSet), new ScoreModel() };
            foreach (Model model in models)
            {
                int numMatched = 0;

                foreach (Comment comment in testDataSet)
                {
                    Label predict = model.Predict(comment);

                    if (comment.Label == predict)
                    {
                        numMatched++;
                    }
                }


                Console.WriteLine($"수행 시간: {stopwatch.Elapsed}");
                Console.WriteLine($"[{model.GetType().Name}] 정확도(%): {(double)numMatched / testDataSet.Count * 100}");
            }

            stopwatch.Stop();
            Console.WriteLine($"긍정 갯수: {repository.CountByLabel(Label.POSITIVE)}");
            Console.WriteLine($"부정 갯수: {repository.CountByLabel(Label.NEAGTIVE)}");

            Console.WriteLine($"수행 시간: {stopwatch.Elapsed}");
        }
    }
}