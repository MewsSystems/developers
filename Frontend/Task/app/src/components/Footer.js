import React from 'react';

const Footer = () => (
  <footer className="page-footer font-small blue pt-4 bg-dark text-white">
    <div className="container-fluid text-center text-md-left">
      <div className="row">
        <div className="col-md-6 mt-md-0 mt-3">
          <h5 className="text-uppercase">Lukáš Kratochvíl</h5>
          <p><a href="mailto:kratochvil.lukas9@gmail.com">kratochvil.lukas9@gmail.com</a></p>
        </div>
        <hr className="clearfix w-100 d-md-none pb-3" />
        <div className="col-md-6 mb-md-0 mb-6">
          <h5 className="text-uppercase">FindMe</h5>
          <ul className="list-unstyled">
            <li>
              <a href="https://www.linkedin.com/in/progresak/">LinkedIn</a>
            </li>
            <li>
              <a href="https://github.com/progresak">GitHub</a>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </footer>
);

export default Footer;
