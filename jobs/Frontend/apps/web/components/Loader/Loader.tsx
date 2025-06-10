/*
 |-----------------------------------------------------------------------------
 | components/Loader/Loader.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import LoaderComponent from '@repo/ui/components/atoms/Loader';
import { twJoin } from 'tailwind-merge';

const Loader = (): ReactElement => {
	return (
		<div
			className={twJoin(
				'absolute',
				'bg-black/25',
				'flex',
				'inset-0',
				'items-center',
				'justify-center',
			)}
		>
			<LoaderComponent />
		</div>
	);
};

export default Loader;
