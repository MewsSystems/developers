import path from 'path'
import merge from 'webpack-merge'
import HtmlWebpackPlugin from 'html-webpack-plugin'
import CircularDependencyPlugin from 'circular-dependency-plugin'
import {BundleAnalyzerPlugin} from 'webpack-bundle-analyzer'
import MiniCssExtractPlugin from 'mini-css-extract-plugin'

const target = process.env.npm_lifecycle_event

const common = {
	devServer: {
		contentBase: `build`,
	},
	entry: [`@babel/polyfill`, path.join(process.cwd(), `src/index.jsx`)],
	output: {
		path: path.resolve(process.cwd(), `build`),
		publicPath: `/`,
	},
	module: {
		rules: [
			{
				test: /\.js|x$/,
				exclude: /node_modules/,
				use: {loader: `babel-loader`}
			},
			{
				test: /\.(png|jpe?g|gif|woff2?|ttf|eot)$/,
				loader: `url-loader`,
			},
			{
				test: /\.css$/,
				use: [
					{loader: MiniCssExtractPlugin.loader},
					`css-loader`]
			},
			{
				type: `javascript/auto`,
				test: /manifest\.json/,
				use: [{
					loader: `file-loader`,
					options: {
						name: `./[name].[ext]`
					}
				}]
			},
		]
	},
	plugins: [
		new HtmlWebpackPlugin({
			inject: true,
			filename: `index.html`,
			template: `src/index.html`,
			chunks: [`main`]
		}),
	],
	resolve: {
		modules: [`src`, `node_modules`],
		extensions: [`.js`, `.jsx`],
		mainFields: [
			`browser`,
			`jsnext:main`,
			`main`
		],
		alias: {moment: `moment/moment.js`}
	},
	target: `web`,
	optimization: {
		splitChunks: {
			cacheGroups: {
				vendors: {
					test: /[\\/]node_modules[\\/]/,
					chunks: `initial`,
					name: `vendor`,
				},
			},
		},
	},
}

let result = {}

switch (target) {
case `webpack:analize`:
	common.plugins.push(new BundleAnalyzerPlugin())
case `build`:
	result = merge(common, {
		output: {
			filename: `[name].[chunkhash].js`,
			chunkFilename: `[name].[chunkhash].chunk.js`
		},
		mode: `production`,
		plugins: [
			new MiniCssExtractPlugin({
				// Options similar to the same options in webpackOptions.output
				// both options are optional
				filename: `[name].[chunkhash].css`,
				chunkFilename: `[name].[chunkhash].chunk.css`,
			}),
		],
	})
	break
case `start`:
case `webpack:watch`:
	result = merge(common, {
		output: {
			filename: `[name].js`,
			chunkFilename: `[name].chunk.js`,
			crossOriginLoading: `anonymous`
		},
		mode: `development`,
		plugins: [
			new CircularDependencyPlugin({
				exclude: /a\.js|node_modules/,
				failOnError: false
			}),
			new MiniCssExtractPlugin({
				// Options similar to the same options in webpackOptions.output
				// both options are optional
				filename: `[name].css`,
				chunkFilename: `[name].chunk.css`,
			}),
		],
	})
	break
default:
	result = common
}

export default result
