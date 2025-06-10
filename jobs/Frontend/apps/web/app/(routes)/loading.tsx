/*
 |-----------------------------------------------------------------------------
 | app/loading.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import Loader from '@repo/ui/components/atoms/Loader';
import { twJoin } from 'tailwind-merge';

const Loading = () => {
	return (
		<div
			className={twJoin(
				'bg-grey-800/25',
				'fixed',
				'flex',
				'inset-0',
				'items-center',
				'justify-center',
				'z-50',
			)}
		>
			<Loader />
		</div>
	);
};

export default Loading;
