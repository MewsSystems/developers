import React from 'react';
import style from './style.scss';

const Loader = () => (
  <div className={style.loadingPage}>
    <div className={style.loaderWrapper}>
      <div className={style.loader}>Loading...</div>
    </div>
    <div className={style.loadingMessage}>Fetching Exchange Rates</div>
  </div>
);

export default Loader;
