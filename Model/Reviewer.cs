using System.Linq;

namespace Model
{
    public class Reviewer
    {
        public Reviewer(string author, State reviewState = State.Unknown)
        {
            Author = author;
            ReviewState = reviewState;
        }

        public string Author { get; set; }

        public State ReviewState { get; set; }

        public static readonly string[] DefaultReviewers = { "fomin", "tatarintsev", "sivykh", "zhadko" };

        public static Reviewer[] NewReviewers() => DefaultReviewers.Select(r => new Reviewer(r)).ToArray();
    }
}
