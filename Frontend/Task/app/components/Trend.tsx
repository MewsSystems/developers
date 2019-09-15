import React from 'react'
import styled from 'styled-components';

import { Rate } from '@store/types';

import Icon from './ui/Icon';
import IconArrowUp from '@icons/arrow-up';

interface TrendProps {
    rate: Rate
}

const Trend: React.FC<TrendProps> = ({ rate }: TrendProps): any => {
    if (rate && rate.trend === "growing") {
        return <Icon svgPaths={IconArrowUp} style={{color: "#36b37e"}} />
    } else if (rate && rate.trend === "declining") {
        return  <Icon svgPaths={IconArrowUp} style={{color: "#cc625d", transform: "rotate(180deg)"}}/>
    } else {
        return <span style={{fontWeight: "bold"}}>—</span>
    }
};

Trend.displayName = 'Trend';

export default Trend;