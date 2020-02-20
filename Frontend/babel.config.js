module.exports = {
    presets: [
        '@babel/preset-react',
        '@babel/preset-typescript',
        [
            '@babel/preset-env',
            {
                modules: false,
                targets: '> 2%, not dead',
            },
        ],
    ],
    plugins: [
        '@babel/plugin-transform-runtime',
    ],
    env: {
        development: {
            plugins: [
                'react-hot-loader/babel',
            ],
        },
    },
};
