namespace SentimentAnalysis
{
    public abstract class Model
    {
        public abstract Label Predict(Comment x);

    }
}
