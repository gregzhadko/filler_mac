using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public State ReviewState
        {
            get => _reviewState;
            set => _reviewState = value;
        }

        public static readonly string[] DefaultReviewers = { "fomin", "tatarintsev", "sivykh", "zhadko" };
        private State _reviewState;

        public static Reviewer[] NewReviewers() => DefaultReviewers.Select(r => new Reviewer(r)).ToArray();
    }
}
