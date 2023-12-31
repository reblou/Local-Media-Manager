﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Web;
using System.Windows.Forms;

namespace MyFlix
{
    public class SearchResponse<T>
    {
        public int page;
        public List<T> results;
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
        public string overview;
        public double popularity;
        public string poster_path;
        public double vote_average;
        public double vote_count;
    }

    public class FilmResult : Result
    {

        public string original_title;
        public string release_date;
        public string title;
        public bool video;
    }
    public class TVResult : Result
    {
        public string[] origin_country;
        public string original_name;
        public string first_air_date;
        public string name;
    }

    public class EmptyResult : Result
    {

    }

    public class TMDBApiHandler
    {
        readonly string rootUrl = "https://api.themoviedb.org";
        readonly string accessToken = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxYjkyYjcwOGRlNWM3NzE2YWExZWM4ZWM5MDU4Njg3ZiIsInN1YiI6IjYwODk1ZTI3Y2FiZmU0MDAzZmVkOGU2ZiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.ke1D7Iht78CtySek8wIUTSQf7lPWvdqbvyZn989pwjo";
        readonly string posterRootUrl = "https://image.tmdb.org/t/p/w780";
        readonly string backropRootUrl = "https://image.tmdb.org/t/p/w1280";
        readonly string posterNotFoundUrl = "/images/1024px-Filmreel-icon.png";
        HttpClient client;

        public TMDBApiHandler()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            client.DefaultRequestHeaders.Add("accept", "application/json");
        }

        public FilmResult SearchMovie(string title, string releaseYear)
        {
            SearchResponse<FilmResult> searchResponse = GetMovieSearchResultsYear(title, releaseYear);

            try
            {
                return extractTopResult(searchResponse.results) as FilmResult;
            }
            catch(NoMatchingResultsException)
            {
                return new FilmResult()
                {
                    title = title,
                    release_date = releaseYear,
                    poster_path = posterNotFoundUrl,
                    backdrop_path = "",
                    overview = "Details not found in lookup.",
                };
            }
        }

        public TVResult SearchTV(string title, string firstAirYear)
        {
            List<(string, string)> parameterPairs = new();
            parameterPairs.Add(("query", title));
            parameterPairs.Add(("year", firstAirYear));
            parameterPairs.Add(("page", "1"));
            string parameters = ParamStringBuilder(parameterPairs);

            SearchResponse<TVResult> response = SearchTVRequest(parameters);

            try
            {
                return extractTopResult(response.results) as TVResult;
            } catch(NoMatchingResultsException)
            {
                return new TVResult()
                {
                    name = title,
                    first_air_date = firstAirYear,
                    poster_path = posterNotFoundUrl,
                    backdrop_path = "",
                    overview = ""
                };
            }
        }

        private SearchResponse<FilmResult> GetMovieSearchResultsYear(string title, string releaseYear)
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

        private SearchResponse<FilmResult> SearchMovieRequest(string parameters)
        {
            string method = "/3/search/movie";
            string fullUrl = rootUrl + method + parameters;

            string json = client.GetStringAsync(fullUrl).Result;
            return JsonConvert.DeserializeObject<SearchResponse<FilmResult>>(json);
        }

        private string ParamStringBuilder(List<(string, string)> parameters)
        {
            string query = "?";

            string safeKey;
            string safeValue;

            foreach((string key, string value) in parameters) 
            {
                safeKey = HttpUtility.UrlEncode(key);
                safeValue = HttpUtility.UrlEncode(value);
                query += $"{safeKey}={safeValue}&";
            }

            // removes trailing &
            return query.Remove(query.Length - 1);
        }

        private Result extractTopResult(IReadOnlyList<Result> results)
        {
            if (results == null || results.Count <= 0)
            {
                throw new NoMatchingResultsException("No results found.");
            }

            Result topResult = results[0];

            topResult.poster_path = SafeReturnPosterUrl(posterRootUrl, topResult.poster_path);
            topResult.backdrop_path = SafeReturnPosterUrl(backropRootUrl, topResult.backdrop_path);

            return topResult;
        }

        private SearchResponse<TVResult> SearchTVRequest(string parameters)
        {
            string method = "/3/search/tv";
            string fullUrl = rootUrl + method + parameters;

            string json = client.GetStringAsync(fullUrl).Result;
            return JsonConvert.DeserializeObject<SearchResponse<TVResult>>(json);
        }

        private string SafeReturnPosterUrl(string rootUrl, string posterPath)
        {
            if (posterPath == null) return posterNotFoundUrl;

            return rootUrl + posterPath;
        }
    }
}
