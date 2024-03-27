/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_MOVIE_DB_API_KEY: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
