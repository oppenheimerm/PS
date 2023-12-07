/// <binding Clean='imgCompress' />
'use strict';

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var _gulpImagemin = require('gulp-imagemin');

var _gulpImagemin2 = _interopRequireDefault(_gulpImagemin);

var gulp = require('gulp');
var concat = require('gulp-concat');
var cleanCss = require('gulp-clean-css');

gulp.task('pack-css', function () {
    return gulp.src(['./wwwroot/css/root.css', './wwwroot/css/layout.css', './wwwroot/css/pages.css', './wwwroot/css/buttons__forms.css', './wwwroot/css/responsive.css']).pipe(concat('stylesheet.css')).pipe(cleanCss()).pipe(gulp.dest('./wwwroot/css/dist'));
});

function imgCompress() {
    return gulp.src('./wwwroot/img/assets/*').pipe((0, _gulpImagemin2['default'])([gulpImagemin.mozjpeg({ quality: 75, progressive: true }), gulpImagemin.optipng({ optimizationLevel: 5 })])).pipe(gulp.dest('./wwwroot/img/dist/assets'));
};

gulp.task("imgCompress", imgCompress);

