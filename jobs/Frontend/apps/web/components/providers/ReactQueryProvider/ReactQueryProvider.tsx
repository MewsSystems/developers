/*
 |-----------------------------------------------------------------------------
 | components/providers/ReactQueryProvider/ReactQueryProvider.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

'use client';

import type { ReactElement } from 'react';

import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

import type { IReactQueryProviderProps } from './types';

const ReactQueryProvider = ({
	children,
}: IReactQueryProviderProps): ReactElement => {
	const queryClient = new QueryClient();

	return (
		<QueryClientProvider client={queryClient}>
			{children}
			<ReactQueryDevtools initialIsOpen={false} />
		</QueryClientProvider>
	);
};

export default ReactQueryProvider;
