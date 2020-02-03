import axios from "axios";

const APIHelper = {};

APIHelper.fetchMovies = (searchTerm, page) => {
  return axios
    .get(
      `${constants.APIURL}search/movie?api_key=${constants.APIKey}&query=${searchTerm}&page=${page}`
    )
    .catch(function(error) {
      console.log(
        "There has been a problem with your fetch operation: " + error.message
      );
      throw error;
    });
};

APIHelper.fetchMovieDetail = id => {
  return axios
    .get(
      `${constants.APIURL}movie/${id}?api_key=${constants.APIKey}&language=en-US`
    )
    .catch(function(error) {
      console.log(
        "There has been a problem with your fetch operation: " + error.message
      );
      throw error;
    });
};

const constants = {
  APIKey: "03b8572954325680265531140190fd2a",
  APIURL: "https://api.themoviedb.org/3/"
};

export default APIHelper;
