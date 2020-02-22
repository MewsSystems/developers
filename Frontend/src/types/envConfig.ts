export interface EnvConfigProps {
  GA_TRACKING_CODE: string
}

declare global {
  interface Window {
    _envConfig: EnvConfigProps
  }
}
