import nock from 'nock'

export const apiNock = (options?: nock.Options) => {
  const { MDB_BASE_URL } = window._envConfig

  return nock(MDB_BASE_URL, options)
}

export const ACCESS_CONTROL_HEADER = { 'Access-Control-Allow-Origin': '*' }
