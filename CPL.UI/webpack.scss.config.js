const path = require('path');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

var _scssPath = path.resolve(__dirname, 'src/scss/dashboard/');
var _cssPath = 'css/dashboard/';

var scss = function (_fileName) {
    return {
        entry: path.resolve(_scssPath, _fileName + '.scss'),
        output: {
            filename: _cssPath + _fileName + '.css'
        },
        module: {
            rules: [{
                test: /\.scss$/,
                use: ExtractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: ['css-loader', 'sass-loader']
                })
            }]
        },
        plugins: [
            new ExtractTextPlugin(_cssPath + _fileName + '.css')
        ],
        mode: 'production',
        stats: { colors: true }
    };
}

module.exports = [
    scss('cpl.layout'),
    scss('cpl.admin')
];