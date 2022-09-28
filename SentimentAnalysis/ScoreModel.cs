namespace SentimentAnalysis
{
    public class ScoreModel : Model
    {
        private readonly Dictionary<string, int> polarityScores = new();

        public ScoreModel()
        {
            
        }

        public override Label Predict(Comment x)
        {

            throw new NotImplementedException();
        }
    }
}
