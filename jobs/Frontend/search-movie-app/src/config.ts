declare global {
  interface ImportMetaEnv {
    readonly VITE_API_KEY: string;
  }

  interface ImportMeta {
    readonly env: ImportMetaEnv;
  }
}

export const API_KEY_MOVIE_SEARCH = import.meta.env.VITE_API_KEY;
