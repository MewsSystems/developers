import {rateTrend} from '../const';


export const getPairName = pair => `${pair[0].name}/${pair[1].name}`;
export const getPairTrend = (val, prev) => {
  if (!val || !prev || val === prev) {
    return rateTrend.stagnating;
  }

  return val > prev ? rateTrend.growing : rateTrend.declining;
};
