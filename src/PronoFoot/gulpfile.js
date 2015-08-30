/// <binding BeforeBuild='default' />
var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var minifyCSS = require('gulp-minify-css');
var copy = require('gulp-copy');
var bower = require('gulp-bower');
var sourcemaps = require('gulp-sourcemaps');
var size = require("gulp-size");
var merge = require("merge-stream");

var config = {
    bundles: [
        {
            name: 'jquery-bundle.min.js',
            sources: [
                'bower_components/jquery/jquery.min.js'
            ]
        },
        {
            name: 'jquery-validate-bundle.min.js',
            sources: [
                'bower_components/jquery-validation/dist/jquery.validate.min.js',
                'bower_components/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js'
            ]
        },

        {
            name: 'bootstrap-bundle.min.js',
            sources: [
                'bower_components/bootstrap/dist/js/bootstrap.min.js'
            ]
        },

        {
            name: 'knockout-bundle.min.js',
            sources: [
                'bower_components/knockout/index.js',
                'App/bindings/*.js'
            ]
        },

        {
            name: 'moment-bundle.min.js',
            sources: [
                'bower_components/moment/min/moment.min.js'
            ]
        }
    ],

    js: 'Scripts/dist/',

    //Bootstrap CSS and Fonts
    bootstrapcss: 'bower_components/bootstrap/dist/css/bootstrap.css',
    boostrapfonts: 'bower_components/bootstrap/dist/fonts/**/*.*',

    appcss: 'Content/master.css',
    fontsout: 'Content/fonts',
    cssout: 'Content/css'
}

gulp.task('clean-js', function (cb) {
    del(config.bundles.map(function (bundle) { return config.js + bundle.name; }), cb);
});

gulp.task('build-js', function () {
    var tasks = config.bundles.map(function (bundle) {
        return gulp
            .src(bundle.sources)
            .pipe(concat(bundle.name))
            .pipe(size({
                title: "Before: " + bundle.name
            }))
            .pipe(uglify())
            .pipe(size({
                title: "After:  " + bundle.name
            }))
            .pipe(gulp.dest(config.js));
    });

    return merge(tasks);
});

gulp.task('clean-css', function (cb) {
    del(config.cssout + '/*.*', cb);
});

gulp.task('build-css', function () {
    return gulp.src([config.bootstrapcss, config.appcss])
     .pipe(concat('app.css'))
     .pipe(gulp.dest(config.cssout))
     .pipe(minifyCSS())
     .pipe(concat('app.min.css'))
     .pipe(gulp.dest(config.cssout));
});

gulp.task('clean-fonts', function (cb) {
    del(config.fontsout + '/*.*', cb);
});

gulp.task('build-fonts', function () {
    return gulp.src(config.boostrapfonts)
      .pipe(gulp.dest(config.fontsout));
});

gulp.task('clean', ['clean-js', 'clean-css', 'clean-fonts'], function () {

});

gulp.task('build', ['build-js', 'build-css', 'build-fonts'], function () {

});

gulp.task('bower-install', function () {
    return bower();
});

gulp.task('default', ['clean', 'bower-install', 'build'], function () {
    // place code for your default task here
});