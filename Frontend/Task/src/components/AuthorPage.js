import React from 'react';

const AuthorPage = () => (
  <section>
    <p>Author: <a href="https://www.mjesiolowski.pl/" target="_blank">Maciej Jesiołowski</a></p>
    <p>For: <a href="https://www.mewssystems.com/" target="_blank">Mews Systems</a></p>
    <p>{new Date().getFullYear()}</p>
  </section>
);

export default AuthorPage;
