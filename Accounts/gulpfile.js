/// <binding AfterBuild='default' />
'use strict';

var gulp = require('gulp');
var rename = require('gulp-rename');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
var minifyCSS = require('gulp-minify-css');

var destination = './content/';
var cleanCSS = require('gulp-clean-css');


gulp.task('default', function() {
  return gulp.src(destination +'*.css')
    .pipe(minifyCSS())
    .pipe(concat('style.min.css')) 
    .pipe(gulp.dest(destination));
});