/*
 |-----------------------------------------------------------------------------
 | components/Bar/Bar.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import BorderRadius from './BorderRadius';
import BorderWidth from './BorderWidth';
import type { IBarProps } from './types';
import Sizing from './Sizing';
import Spacing from './Spacing';

const Bar = ({
	height,
	label,
	size,
	variant,
	width,
}: IBarProps): ReactElement => {
	const variants = {
		borderRadius: <BorderRadius label={label} size={size} />,
		borderWidth: <BorderWidth height={height} />,
		sizing: <Sizing height={height} width={width} />,
		spacing: <Spacing width={width} />,
	};

	return variants[variant as keyof typeof variants];
};

export default Bar;
