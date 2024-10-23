import Axios from 'axios';

const baseUrl = import.meta.env.VITE_MOVIE_DB_IMAGE_PATH;

const imagesApiClient = Axios.create({
  baseURL: `${baseUrl}`,
});

export { imagesApiClient };
