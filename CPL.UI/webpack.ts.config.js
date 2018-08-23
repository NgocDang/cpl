const path = require('path');

var _tsPath = path.resolve(__dirname, 'src/ts/dashboard/');
var _jsPath = 'js/dashboard/';

var ts = function (_fileName) {
    return {
        entry: path.resolve(_tsPath, _fileName + '.ts'),
        output: {
            filename: _jsPath + _fileName + '.js'
        },
        module: {
            rules: [
                {
                    test: /\.tsx?$/,
                    use: 'ts-loader',
                    exclude: /node_modules/
                }
            ]
        },
        resolve: {
            extensions: ['.tsx', '.ts', '.js']
        },
        mode: 'production',
        stats: { colors: true }
    };
}

module.exports = [
    ts('cpl.layout'),
    ts('cpl.admin')
];