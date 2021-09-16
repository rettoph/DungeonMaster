const path = require('path');
const VueLoaderPlugin = require('vue-loader/lib/plugin');

module.exports = {
    entry: {
        "theme": './ClientData/Scripts/Shared/Theme/index.js',
        "guilds-index": './ClientData/Scripts/Guilds/Index/index.js',
        "guilds-categories": './ClientData/Scripts/Guilds/Categories/index.js',
        "music-index": './ClientData/Scripts/Music/Index/index.js',
    },
    resolve: {
        alias: {
            'vue$': 'vue/dist/vue.esm.js'
        }
    },
    module: {
        rules: [
            {
                test: /\.vue?$/,
                loader: 'vue-loader'
            },
            {
                test: /\.css$/,
                use: [
                    'vue-style-loader',
                    'css-loader'
                ]
            },
            {
                test: /\.scss$/,
                use: [
                    'vue-style-loader',
                    'css-loader',
                    'sass-loader'
                ]
            }
        ]
    },
    plugins: [
        new VueLoaderPlugin(),
    ],
    output: {
        // The format for the outputted files
        filename: '[name].js',
        // Put the files in "wwwroot/js/"
        path: path.resolve(__dirname, 'wwwroot/js/')
    }
};