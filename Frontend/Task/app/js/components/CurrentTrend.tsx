import { Trend } from '../enums/Trend';
import * as React from 'react';

export const CurrentTrend = ({ trend }: { trend: Trend }) => <span className="font-weight-bold">
	{trend === Trend.GROWING ? <span className="text-success">↑</span> : (trend === Trend.DECLINING ? <span className="text-danger">↓</span> : (trend === Trend.STAGNATING ? '--' : ''))}
</span>;