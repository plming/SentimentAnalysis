namespace SentimentAnalysis
{
    public class ScoreModel : Model
    {
        private readonly Dictionary<string, int> polarityScores;

        public ScoreModel()
        {
            polarityScores = DataLoader.LoadPolarityScoresFromCsvFile(Path.Combine(DataLoader.DATA_DIR_PATH, "polarity_scores.csv"));
        }

        public override Label Predict(Comment x)
        {

            throw new NotImplementedException();
        }
    }
}
