/*
 |-----------------------------------------------------------------------------
 | app/(routes)/(shell)layout.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import Header from '@repo/ui/components/organisms/Header';
import { twJoin } from 'tailwind-merge';

import { TLayoutProps } from '../types';

const Layout = ({ children }: TLayoutProps): ReactElement => {
	return (
		<div className="bg-grey-200 flex min-h-screen">
			<div className="grow">
				<Header title="Movies" />

				<main
					className={twJoin(
						'grow',
						'min-h-[calc(100vh-76px)]',
						'py-md',
						'relative',
						'space-y-md',
					)}
				>
					{children}
				</main>
			</div>
		</div>
	);
};

export default Layout;
