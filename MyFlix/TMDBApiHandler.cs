using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Windows.Forms;

namespace MyFlix
{
    public class SearchResponse
    {
        public int page;
        public List<Result> results;
        public int total_pages;
        public int total_results;
    }

    public class Result
    {
        public bool adult;
        public string backdrop_path;
        public int[] genre_ids;
        public int id;
        public string original_language;
        public string original_title;
        public string overview;
        public double popularity;
        public string poster_path;
        public string release_date;
        public string title;
        public bool video;
        public double vote_average;
        public double vote_count;
    }

    public class EmptyResult : Result
    {

    }

    public class TMDBApiHandler
    {
        readonly string key = "1b92b708de5c7716aa1ec8ec9058687f";
        readonly string rootUrl = "https://api.themoviedb.org";
        readonly string accessToken = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxYjkyYjcwOGRlNWM3NzE2YWExZWM4ZWM5MDU4Njg3ZiIsInN1YiI6IjYwODk1ZTI3Y2FiZmU0MDAzZmVkOGU2ZiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.ke1D7Iht78CtySek8wIUTSQf7lPWvdqbvyZn989pwjo";

        //TODO: get a smaller size
        readonly string posterRootUrl = "https://image.tmdb.org/t/p/w600_and_h900_bestv2";
        HttpClient client;

        public TMDBApiHandler()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            client.DefaultRequestHeaders.Add("accept", "application/json");
        }

        public Result SearchMovieTitleOnly(string title)
        {
            return SearchMovie(title, "");
        }

        public Result SearchMovie(string title, string releaseYear)
        {
            SearchResponse searchResponse = new();

            if (string.IsNullOrEmpty(releaseYear))
            {
                searchResponse = GetMovieSearchResults(title);
            }
            else
            {
                searchResponse = GetMovieSearchResultsYear(title, releaseYear);
            }

            return extractTopResult(searchResponse.results);
        }

        private SearchResponse GetMovieSearchResultsYear(string title, string releaseYear)
        {
            List<(string, string)> parameterPairs = new();
            parameterPairs.Add(("query", title));
            parameterPairs.Add(("page", "1"));
            if (!string.IsNullOrEmpty(releaseYear))
            {
                parameterPairs.Add(("primary_release_year", releaseYear));
            }

            string paramters = ParamStringBuilder(parameterPairs);

            return SearchMovieRequest(paramters);
        }

        private SearchResponse GetMovieSearchResults(string title)
        {
            return GetMovieSearchResultsYear(title, "");
        }

        private SearchResponse SearchMovieRequest(string parameters)
        {
            string method = "/3/search/movie";
            string fullUrl = rootUrl + method + parameters;

            string json = client.GetStringAsync(fullUrl).Result;
            return JsonConvert.DeserializeObject<SearchResponse>(json);
        }

        private string ParamStringBuilder(List<(string, string)> parameters)
        {
            string query = "?";

            foreach((string key, string value) in parameters) 
            {
                query += $"{key}={value}&";
            }

            // remove trailing &
            return query.Remove(query.Length - 1);
        }

        private Result extractTopResult(List<Result> results)
        {
            if (results == null || results.Count <= 0)
            {
                return new EmptyResult();
            }

            Result topResult = results[0];

            topResult.poster_path = posterRootUrl + topResult.poster_path;
            topResult.backdrop_path = posterRootUrl + topResult.backdrop_path;

            return topResult;
        }

        private Result GetMostPopularResult(List<Result> results)
        {
            Result mostPopularResult = new Result() { popularity = 0};
            foreach (var result in results)
            {
                if (result.popularity > mostPopularResult.popularity)
                {
                    mostPopularResult = result;
                }
            }

            return mostPopularResult;
        }
    }
}
