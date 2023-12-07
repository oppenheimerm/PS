/// <binding Clean='imgCompress' />


var gulp = require('gulp');
var concat = require('gulp-concat');
var cleanCss = require('gulp-clean-css');
var imagemin = import('gulp-imagemin');



gulp.task('pack-css', function () {
    return gulp.src([
        './wwwroot/css/root.css',
        './wwwroot/css/layout.css',
        './wwwroot/css/pages.css',
        './wwwroot/css/buttons__forms.css',
        './wwwroot/css/responsive.css'
    ])
        .pipe(concat('stylesheet.css'))
        .pipe(cleanCss())
        .pipe(gulp.dest('./wwwroot/css/dist'));
});

function imgCompress() {
    return gulp
        .src('./wwwroot/img/assets/*')
        .pipe(imagemin([
            gulpImagemin.mozjpeg({ quality: 75, progressive: true }),
            gulpImagemin.optipng({ optimizationLevel: 5 })
        ]))
        .pipe(gulp.dest('./wwwroot/img/dist/assets'))
};

gulp.task("imgCompress", imgCompress);
