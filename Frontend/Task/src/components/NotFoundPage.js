import React from 'react';
import { Link } from 'react-router-dom';

const NotFoundPage = () => (
  <div className="content__text">
    404 - <Link to="/" className="content__link">Go home</Link>
  </div>
);

export default NotFoundPage;
