/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Loader/Loader.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import { twJoin } from 'tailwind-merge';

const Loader = (): ReactElement => {
	return (
		<div className="size-11" data-testid="loader">
			<span className="sr-only">Loading...</span>

			{[1, 2, 3, 4, 5].map((index: number) => (
				<div
					key={index}
					className={twJoin(
						'animate-stretch',
						'bg-primary',
						'inline-block',
						'h-full',
						'mx-px',
						'w-1.5',
						index === 2 && 'animation-delay-[-1.1s]',
						index === 3 && 'animation-delay-[-1s]',
						index === 4 && 'animation-delay-[-0.9s]',
						index === 5 && 'animation-delay-[-0.8s]',
					)}
				></div>
			))}
		</div>
	);
};

export default Loader;
