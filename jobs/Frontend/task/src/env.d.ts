// used to create typed .env variables
interface ImportMetaEnv {
  VITE_MOVIES_API_KEY: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
