const path = require("path");

module.exports = {
    entry: path.resolve(__dirname, "app/app.js"),
    output: {
        path: path.resolve(__dirname, "app/assets"),
        filename: "bundle.js",
        publicPath: "assets"
    },
    devServer: {
        inline: true,
        contentBase: "./app",
        port: 3030
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /(node_modules)/,
                loader: "babel-loader",
                query: {
                    presets: ["latest", "stage-0"]
                }
            }
        ]
    }
};
