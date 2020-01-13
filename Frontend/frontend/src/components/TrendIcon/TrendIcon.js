import React from 'react';
import { string, } from 'prop-types';

import { TREND_ASC, TREND_DES, TREND_EQL, } from '../../globals';
import TrendArrow from '../../atoms/Icons/TrendArrow';
import TrendCircle from '../../atoms/Icons/TrendCircle';
import StyledTrendIcon from './styles/StyledTrendIcon';


const TREND_ICONS = {
  [TREND_ASC]: (
    <StyledTrendIcon>
      <TrendArrow className="trendIcon--asc" />
    </StyledTrendIcon>
  ),
  [TREND_DES]: (
    <StyledTrendIcon>
      <TrendArrow className="trendIcon--des" />
    </StyledTrendIcon>
  ),
  [TREND_EQL]: (
    <StyledTrendIcon>
      <TrendCircle className="trendIcon--eql" />
    </StyledTrendIcon>
  ),
};


const TrendIcon = ({ trend, }) => {
  if (!trend) return null;
  if (Object.prototype.hasOwnProperty.call(TREND_ICONS, trend)) return TREND_ICONS[trend];
  return TREND_ICONS[TREND_EQL];
};


TrendIcon.propTypes = {
  trend: string,
};

TrendIcon.defaultProps = {
  trend: undefined,
};


export default TrendIcon;
