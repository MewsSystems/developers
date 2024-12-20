import axios from 'axios';

const API_KEY = import.meta.env.VITE_TMDB_API_KEY as string;
const BASE_URL = import.meta.env.VITE_TMDB_API_URL as string;
const IMAGE_URL = import.meta.env.VITE_TMDB_API_IMG_URL as string;

export const searchMovies = async (query: string, page: number = 1) => {
  const response = await axios.get(`${BASE_URL}/search/movie`, {
    params: {
      api_key: API_KEY,
      query: query,
      page: page,
    },
  });

  return response.data;
};

export const getMovieDetails = async (id: string) => {
  const [detailsResponse, creditsResponse] = await Promise.all([
    axios.get(`${BASE_URL}/movie/${id}`, {
      params: {
        api_key: API_KEY,
      },
    }),
    axios.get(`${BASE_URL}/movie/${id}/credits`, {
      params: {
        api_key: API_KEY,
      },
    }),
  ]);

  return {
    ...detailsResponse.data,
    credits: creditsResponse.data,
  };
};

export const getMovieImageUrl = (path: string | null) => {
  return path ? `${IMAGE_URL}${path}` : "/assets/image-placeholder.png";
};
