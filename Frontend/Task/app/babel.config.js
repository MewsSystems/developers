module.exports = () => {
  const presets = [
    [
      '@babel/env',
      {
        targets: {},
        useBuiltIns: 'usage',
      },
    ],
  ]

  return { presets }
}
