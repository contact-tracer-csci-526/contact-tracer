const { buildLogger } = require('jege/server');
const cheerio = require('cheerio');
const del = require('del');
const fs = require('fs');
const gulp = require('gulp');
const path = require('path');
const showdown = require('showdown');

const buildLog = buildLogger('[contact-tracers]');

const paths = {
  docs: path.resolve(__dirname, '../docs'),
  public: path.resolve(__dirname, '../doc-generator/public'),
  root: path.resolve(__dirname, '..'),
  webgl: path.resolve(__dirname, '../webgl'),
};

gulp.task('clean', () => {
  const cleanPaths = [
    `${paths.docs}/**/*`,
  ];

  buildLog('clean', 'cleanPaths: %j', cleanPaths);

  return del(cleanPaths, {
    force: true,
  });
});

gulp.task('copy-webgl', () => {
  const srcPath = `${paths.webgl}/**/*`;
  const destPath = `${paths.docs}`;
  buildLog('copy-webgl', 'srcPath: %s, destPath: %s', srcPath, destPath);

  return gulp.src(srcPath)
    .pipe(gulp.dest(destPath));
});

gulp.task('copy-public', () => {
  const srcPath = `${paths.public}/**/*`;
  const destPath = `${paths.docs}`;
  buildLog('copy-public', 'srcPath: %s, destPath: %s', srcPath, destPath);

  return gulp.src(srcPath)
    .pipe(gulp.dest(destPath));
});

gulp.task('copy-index-html', () => {
  const srcPath = `${paths.docs}/index.html`;
  const destPath = `${paths.docs}/index_original.html`;
  buildLog('copy-index-html', 'srcPath: %s, destPath: %s', srcPath, destPath);
  fs.copyFileSync(srcPath, destPath);

  return Promise.resolve();
});

gulp.task('modify-html', () => {
  const srcPath = path.resolve(paths.docs, 'index.html');
  const readmePath = path.resolve(paths.root, 'README.md');
  buildLog('modify-html', 'srcPath: %s', srcPath);

  const indexHtml = fs.readFileSync(srcPath).toString();
  const $ = cheerio.load(indexHtml);
  const md = new showdown.Converter();
  const readme = fs.readFileSync(readmePath).toString();
  const readmeHtml = md.makeHtml(readme);

  const GameContainer = {
    width: '90vw',
    height: '56.25vw',
    maxWidth: '585px',
    maxHeight: '406.25px',
  };

  const headHtml = `
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans+JP:wght@400;700&display=swap" rel="stylesheet">
`;

  const scriptHtml = `
<script>
</script>
`;
  const styleHtml = `
<style>
  html {
    background-color: rgb(242, 242, 242);
  }
  html, body {
    margin: 0;
    padding: 0;
    font-family: Helvetica, Arial, "sans-serif";
    line-height: 1.5;
  }
  body {
    font-size: 16px;
  }
  a, a:visited {
    color: #0F79D0;
    text-decoration: none;
  }
  a:hover {
    color: #0F79D0;
    text-decoration: underline;
  }
  h1, h2 {
    font-family: "Noto Sans JP", serif;
  }
  h1 {
    font-size: 32px;
    margin-top: 1.1em;
    margin-bottom: 0.2em;
  }
  h2 {
    border-bottom: 1px solid #d9d9d9;
    font-size: 28px;
    margin-top: 1.1em;
    margin-bottom: 0.2em;
  }
  ul {
    padding-left: 26px;
  }
  #wrap {
    align-items: center;
    display: flex;
    flex-direction: column;
  }
  @media (max-width: 480px) {
    body {
      font-size: 14px;
    }
  }
  .warn {
    color: rgb(91, 91, 91);
    font-size: 0.92em;
    line-height: 1.45;
    padding: 12px 0;
    text-align: center;
  }
  .warn p {
    font-weight: 300;
    margin: 0;
    padding: 0;
  }
  .warn a {
    color: inherit;
  }
  .warn span {
    color: rgb(15, 121, 208);
  }
  .warn i {
    border-color: rgb(70, 70, 70);
    border-style: solid;
    border-width: 0px 1px 1px 0px;
    display: inline-block;
    margin-left: 6px;
    padding: 3px;
    transform: rotate(45deg) translate(-0.15em, -0.15em);
  }
  .webgl-content {
    align-items: center;
    display: flex;
    flex-direction: column;
    padding: 0.2em 0;
    position: static;
    transform: none;
    width: 100%;
  }
  .footer {
  }
  .footer .title {
    display: none;
  }
  .webgl-content .footer .webgl-logo {
    display: none;
  }
  #desc {
    margin: 10px 0 60px 0;
  }
  .content-area {
    max-width: 650px;
    padding: 12px;
  }
  .media-container {
    display: flex;
    justify-content: center;
    padding: 10px 0;
  }
</style>
`;

  const warnHtml = `
<div class="warn">
  <p>
    <b>This application is designed to run in mobile devices that support WebGL</b>
  </p>
  <p>
    <a href="#contacttracers">
      <b>Contact Tracers</b> - Read the <span>description</span> below<i></i>
    </a>
  </p>
</div>
`;

  const descHtml = `
<div id="desc" class="content-area">
${readmeHtml}
</div>
`;

  $('title').html('Contact Tracers: Survival from virus pandemic');
  $('head').append(styleHtml);
  $('head').append(headHtml);
  $('head').append(scriptHtml);
  $('body').prepend('<div id="wrap"></div>');
  const webglContent = $('.webgl-content');

  $('#wrap').prepend(warnHtml);
  $('#wrap').append(webglContent);
  $('#wrap').append(descHtml);
  $('img').parent().addClass('media-container');
  const nextHtml = $.html();

  fs.writeFileSync(srcPath, nextHtml, {
    flag: 'w',
  });
  return Promise.resolve();
});

gulp.task('build', gulp.series('clean', 'copy-webgl', 'copy-public',
                               'copy-index-html', 'modify-html'));

if (require.main === module) {
  const buildTask = gulp.task('build');
  buildTask();
}
