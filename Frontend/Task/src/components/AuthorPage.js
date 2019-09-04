import React from 'react';

const AuthorPage = () => (
  <section className="author">
    <p className="content__text">Author: <a href="https://www.mjesiolowski.pl/" target="_blank" className="author__link">Maciej Jesiołowski</a></p>
    <p className="content__text">For: <a href="https://www.mewssystems.com/" target="_blank" className="author__link">Mews Systems</a></p>
    <p className="content__text">{new Date().getFullYear()}</p>
  </section>
);

export default AuthorPage;
