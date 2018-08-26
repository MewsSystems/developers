import React, { Fragment } from 'react';
import style from './style.scss';

type Props = {
  pairInfo: Object,
  trendDirection: string,
};
const ExchangeCard = (props: Props) => {
  const { pairInfo, trendDirection } = props;
  const { first, second, rate } = pairInfo;

  return (
    <div className={style.exchangeCard}>
      <div className={style.top}>
        <div className={style.first}>
          <div className={style.code}>{first.code}</div>
          <div className={style.name}>{first.name}</div>
        </div>
        <span className={style.arrow}>arrow_right</span>
        <div className={style.second}>
          <div className={style.code}>{second.code}</div>
          <div className={style.name}>{second.name}</div>
        </div>
      </div>
      <div className={style.bottom}>
        {rate === undefined ? (
          <div className={style.loader} />
        ) : (
          <Fragment>
            <div className={style.rate}>{rate}</div>
            <div className={style.trend}>
              {trendDirection === 'up' && (
                <span className={style.trendingUp}>trending_up</span>
              )}
              {trendDirection === 'down' && (
                <span className={style.trendingDown}>trending_down</span>
              )}
              {trendDirection === 'flat' && (
                <span className={style.trendingFlat}>trending_flat</span>
              )}
            </div>
          </Fragment>
        )}
      </div>
    </div>
  );
};

export default ExchangeCard;
