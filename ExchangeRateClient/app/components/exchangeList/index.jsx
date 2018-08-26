// @flow
import React from 'react';
import style from './style.scss';

type Props = {
  children: any,
};
const ExchangeList = (props: Props) => {
  const { children } = props;
  return <div className={style.exchangeList}>{children}</div>;
};

export default ExchangeList;
