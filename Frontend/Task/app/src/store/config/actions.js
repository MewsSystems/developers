export const LOAD_CONFIG = 'config/LOAD'

export const loadConfig = config => ({
  type: LOAD_CONFIG,
  payload: config,
})
