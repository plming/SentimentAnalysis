using System.Diagnostics;

namespace SentimentAnalysis
{
    public class Knn
    {
        /// <summary>
        /// 거리 공식을 계산하기 위한 최대 빈도수 캐시입니다.
        /// </summary>
        private readonly Dictionary<string, int> maxFrequencyCache = new();

        /// <summary>
        /// 거리 공식을 계산하기 위한 최소 빈도수 캐시입니다.
        /// </summary>
        private readonly Dictionary<string, int> minFrequencyCache = new();

        private readonly List<Comment> trainData;

        /// <summary>
        /// K-NN 모델을 생성합니다. comments는 모델의 훈련에 쓰이며, 댓글의 label은 긍정 또는 부정이어야만 합니다.
        /// </summary>
        /// <param name="comments"></param>
        public Knn(List<Comment> comments)
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

        /// <summary>
        /// 학습한 댓글에 기반하여 새로운 댓글 x의 레이블을 예측합니다.
        /// </summary>
        /// <param name="x">레이블을 예측할 댓글</param>
        /// <returns>x가 긍정적인 댓글일 경우 Label.POSITIVE, 부정적인 댓글일 경우 Label.NEGATIVE를 반환합니다.</returns>
        public Label Predict(Comment x)
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

            const int NUM_NEAREST_NEIGHBORS = 3;
            Debug.Assert(NUM_NEAREST_NEIGHBORS % 2 == 1, "K값은 홀수여야 합니다.");

            int numPositiveComments = 0;
            for (int i = 0; i < NUM_NEAREST_NEIGHBORS; i++)
            {
                if (distancePriorityQueue.Dequeue().Label == Label.POSITIVE)
                {
                    ++numPositiveComments;
                }
            }

            return numPositiveComments > NUM_NEAREST_NEIGHBORS / 2 ? Label.POSITIVE : Label.NEAGTIVE;
        }
    }
}
