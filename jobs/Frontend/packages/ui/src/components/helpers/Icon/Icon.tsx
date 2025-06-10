/*
 |-----------------------------------------------------------------------------
 | src/components/helpers/Icon/Icon.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import type { IIconProps } from './types';

const Icon = ({ classNames, icons, name }: IIconProps): ReactElement => {
	// @ts-expect-error TODO: Type definition
	const SelectedIcon = icons[name];

	return <SelectedIcon className={classNames} name={name} />;
};

export default Icon;
