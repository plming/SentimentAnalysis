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
            int sumFrequency = 0;
            int sumScoreFrequencyProduct = 0;
            foreach (string word in x.Words)
            {
                int frequency = x.GetFrequency(word);
                
                sumFrequency += frequency;
                sumScoreFrequencyProduct += polarityScores.GetValueOrDefault(word, 0) * frequency;
            }

            const double THRESHOLD = 0.4;
            return (double)sumScoreFrequencyProduct / sumFrequency > THRESHOLD ? Label.POSITIVE : Label.NEAGTIVE;
        }
    }
}
