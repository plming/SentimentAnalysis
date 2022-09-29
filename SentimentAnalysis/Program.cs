using System.Diagnostics;

namespace SentimentAnalysis
{
    public class Program
    {
        public static void Main()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            CommentRepository repository = new(Path.Combine(DataLoader.DATA_DIR_PATH, "comments.csv"));

            repository.TrySplit(ratio: 0.7,
                                out List<Comment> trainDataSet,
                                out List<Comment> testDataSet);

            Model[] models = { new KnnModel(trainDataSet), new ScoreModel() };

            foreach (Model model in models)
            {
                int numMatched = 0;

                foreach(Comment comment in testDataSet)
                {
                    Label predict = model.Predict(comment);

                    if (comment.Label == predict)
                    {
                        numMatched++;
                    }
                }

                Console.WriteLine($"[{model.GetType().Name}] 정확도(%): {(double)numMatched / testDataSet.Count * 100}");
            }

            stopwatch.Stop();
            Console.WriteLine($"긍정 갯수: {repository.Count(Label.POSITIVE)}");
            Console.WriteLine($"부정 갯수: {repository.Count(Label.NEAGTIVE)}");

            Console.WriteLine($"수행 시간: {stopwatch.Elapsed}");
        }
    }
}