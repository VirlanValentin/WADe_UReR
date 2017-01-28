using System.Collections.Generic;

namespace Framework.Common
{
    public class MovieModel
    {
        public string Resource { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string GenreResource { get; set; }
        public string GenreLabel { get; set; }

        public List<string> GenreLabelsList { get; set; } = new List<string>();
        public string ImdbLink { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is MovieModel))
                return false;

            var movie = obj as MovieModel;
            return this.Resource.Equals(movie.Resource);
        }
    }
}