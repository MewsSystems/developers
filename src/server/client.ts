import axios from "axios";
import { env } from "~/env";

export const tmdbClient = axios.create({
  baseURL: env.TMDB_URL,
  params: {
    api_key: env.TMDB_API_KEY,
  },
});
