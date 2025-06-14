interface ImportMetaEnv {
    readonly VITE_TMDB_API_KEY: string;
}
  
interface ImportMeta {
    readonly env: ImportMetaEnv;
}

declare module "*.svg" {
  const content: string;
  export default content;
}