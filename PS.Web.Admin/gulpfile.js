var gulp = require('gulp');
var concat = require('gulp-concat');
var minify = require('gulp-minify');
var cleanCss = require('gulp-clean-css');
var imagemin = import('gulp-imagemin');;

gulp.task('pack-js', function () {
    return gulp.src(['./wwwroot/js/AppView.js', 'wwwroot/js/main.js'])
        .pipe(minify({
            ext: {
                min: '.js'
            },
            noSource: true
        }))
        .pipe(gulp.dest('./wwwroot/js/dist/'));
});

gulp.task('pack-css', function () {
    return gulp.src(['./wwwroot/css/root.css', './wwwroot/css/layout.css',
        './wwwroot/css/pages.css', './wwwroot/css/buttons__forms.css',
        './wwwroot/css/responsive.css'
    ])
        .pipe(concat('stylesheet.css'))
        .pipe(cleanCss())
        .pipe(gulp.dest('./wwwroot/css/dist'));
});

/*gulp.task('image-min', async function () {
    return gulp.src('./wwwroot/img/assets/*.{jpg,png,webp}')
        .pipe(imagemin())
        .pipe(gulp.dest('./wwwroot/img/dist/assets'));
});*/

gulp.task('default', gulp.series('pack-js', 'pack-css'));