﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace MyFlix
{
    public class SearchResponse
    {
        public int page;
        public List<Result> results;
        public int total_pages;
        public int total_results;

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
    }

    public class TMDBApiHandler
    {
        string key = "1b92b708de5c7716aa1ec8ec9058687f";
        string rootUrl = "https://api.themoviedb.org";
        string accessToken = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxYjkyYjcwOGRlNWM3NzE2YWExZWM4ZWM5MDU4Njg3ZiIsInN1YiI6IjYwODk1ZTI3Y2FiZmU0MDAzZmVkOGU2ZiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.ke1D7Iht78CtySek8wIUTSQf7lPWvdqbvyZn989pwjo";
        HttpClient client;

        public TMDBApiHandler()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            client.DefaultRequestHeaders.Add("accept", "application/json");
        }

        public Video SearchMovieToVideo(string movieName)
        {
            SearchResponse searchResponse = GetMovieSearchResults(movieName);

            if (searchResponse.results.Count <= 0) return new Video();

            SearchResponse.Result topResult = searchResponse.results[0];

            Video video = new Video()
            {
                description = topResult.overview,
                posterURL = "https://image.tmdb.org/t/p/original" + topResult.poster_path,
                backdropURL = topResult.backdrop_path
            };
            return video;
        }

        public SearchResponse GetMovieSearchResults(string movieName)
        {
            string method = "/3/search/movie";

            string fullUrl = rootUrl + method + $"?query={movieName}&page=1";

            string json = client.GetStringAsync(fullUrl).Result;
            return JsonConvert.DeserializeObject<SearchResponse>(json);
        }
    }
}
