using System.Diagnostics;

namespace SentimentAnalysis
{
    public class KnnModel : Model
    {
        private readonly Dictionary<string, int> maxFrequencyCache = new();

        private readonly Dictionary<string, int> minFrequencyCache = new();

        private readonly List<Comment> trainData;

        public KnnModel(List<Comment> comments)
        {
            trainData = new(comments);

            foreach (Comment comment in comments)
            {
                Debug.Assert(comment.Label != Label.UNKNOWN, "학습에 쓰일 댓글의 label은 긍정 또는 부정이어야만 합니다");

                foreach (string word in comment.Words)
                {
                    int frequency = comment.GetFrequency(word);

                    if (!maxFrequencyCache.ContainsKey(word))
                    {
                        // 캐시 추가
                        maxFrequencyCache[word] = frequency;
                        minFrequencyCache[word] = frequency;
                    }
                    else
                    {
                        // 캐시 갱신
                        if (frequency > maxFrequencyCache[word])
                        {
                            maxFrequencyCache[word] = frequency;
                        }

                        if (frequency < minFrequencyCache[word])
                        {
                            minFrequencyCache[word] = frequency;
                        }
                    }
                }
            }

            Debug.Assert(maxFrequencyCache.Keys.Count == minFrequencyCache.Keys.Count, "캐시된 단어가 불일치합니다.");
        }

        public override Label Predict(Comment x)
        {
            PriorityQueue<Comment, double> distancePriorityQueue = new(trainData.Count);


            foreach (Comment comment in trainData)
            {
                double sumTerms = 0;
                foreach (string word in maxFrequencyCache.Keys)
                {
                    int dividend = x.GetFrequency(word) - comment.GetFrequency(word);

                    int divisor = maxFrequencyCache[word] - minFrequencyCache[word];
                    divisor = divisor == 0 ? 1 : divisor;

                    sumTerms += Math.Pow((double)dividend / divisor, 2);
                }

                double distance = Math.Sqrt(sumTerms);

                distancePriorityQueue.Enqueue(comment, distance);
            }

            const int K = 3;
            Debug.Assert(K % 2 == 1, "K값은 홀수여야 합니다.");

            int numPositiveComments = 0;
            for (int i = 0; i < K; i++)
            {
                if (distancePriorityQueue.Dequeue().Label == Label.POSITIVE)
                {
                    ++numPositiveComments;
                }
            }

            return numPositiveComments > K / 2 ? Label.POSITIVE : Label.NEAGTIVE;
        }
    }
}
