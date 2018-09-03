import React from 'react';
import ReactDOM from 'react-dom';

import Root from './Root';
import { endpoint, interval } from './config';

function render(Component) {
  ReactDOM.render(<Component />, document.getElementById('app-root'));
}

render(Root);

if (module.hot) {
  module.hot.accept('./Root', () => {
    render(Root);
  });
}
