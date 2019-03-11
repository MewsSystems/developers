module.exports = {
  presets: [
    ['@babel/preset-env', {
      debug: false,
      useBuiltIns: 'usage',
      modules: 'commonjs',
      targets: {
        browsers: ['> 1%', 'last 2 versions', 'ie >= 11'],
      },
    }],
    ['@babel/preset-react'],
    ['@babel/preset-typescript'],
  ],
};
