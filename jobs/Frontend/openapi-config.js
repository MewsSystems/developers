const pathMatcher = (pattern) => {
  return (operationName, operationDefinition) => {
    return pattern.test(operationDefinition.path);
  };
}

const config = {
  schemaFile: 'https://developer.themoviedb.org/openapi/64542913e1f86100738e227f',
  apiFile: './src/store/slices/TMDBApi/TMDBApiSlice.ts',
  apiImport: 'TMDBApiSlice',
  outputFile: './src/store/slices/TMDBApi/GENERATED_TMDBApi.ts',
  exportName: 'TMDBApi',
  filterEndpoints: pathMatcher(/(search\/movie|3\/movie\/)/i),
  tags: true,
  hooks: { queries: true, lazyQueries: true, mutations: true },
}

export default config
