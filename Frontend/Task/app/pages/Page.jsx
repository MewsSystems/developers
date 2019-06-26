import React from 'react';
import PropTypes from 'prop-types';

import './Page.scss';

const Page = (props) => (
  <div className="Page">
    <header className="Page__header">Exchange Rates Client</header>
    <div className="Page__body">{props.children}</div>
    <footer className="Page__footer">&copy; 2019</footer>
  </div>
);

Page.propTypes = {
  children: PropTypes.element
}

export default Page;