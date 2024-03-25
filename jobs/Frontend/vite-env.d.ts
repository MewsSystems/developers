/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_API_KEY: number
}

interface ImportMeta {
    readonly env: ImportMetaEnv
}
