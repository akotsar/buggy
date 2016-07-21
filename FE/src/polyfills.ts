import 'es6-shim';
import 'reflect-metadata';
require('zone.js/dist/zone');

import 'ts-helpers';
import 'intl';
import 'intl/locale-data/jsonp/en.js';

if (!global.Intl) {
      require('intl');
      require('intl/locale-data/jsonp/en.js');
}

if (process.env.ENV === 'build') {
  // Production

} else {
  // Development

  Error['stackTraceLimit'] = Infinity;

  require('zone.js/dist/long-stack-trace-zone');
}
