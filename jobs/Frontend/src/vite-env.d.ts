/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_MOVIE_DB_API_KEY: string;
  readonly VITE_MOVIE_DB_BASE_URL: string;
  readonly VITE_MOVIE_DB_IMAGE_BASE_URL: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
