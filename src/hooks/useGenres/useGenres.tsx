import { useState, useEffect } from "react";
import axios from "axios";

export interface Genre {
  id: number;
  name: string;
}

export interface GenreResponse {
  genres: Genre[];
}

interface UseGenresReturn {
  genres: Genre[];
}

const API_KEY = "03b8572954325680265531140190fd2a";
const API_URL = "https://api.themoviedb.org/3/genre/movie/list";

export const useGenres = ():UseGenresReturn => {
  const [genres, setGenres] = useState<Genre[]>([]);

  useEffect(() => {
    const fetchGenres = async () => {
      try {
        const response = await axios.get<GenreResponse>(API_URL, {
          params: {
            api_key: API_KEY,
            language: "en-US",
          },
        });
        setGenres(response.data.genres);
      } catch (error) {
        console.error("Error fetching genres:", error);
      }
    };

    fetchGenres();
  }, []);
  return { genres };
};