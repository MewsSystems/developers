/*
 |-----------------------------------------------------------------------------
 | app/layout.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactNode } from 'react';

import type { Metadata, Viewport } from 'next';
import { Inter } from 'next/font/google';
import { twJoin } from 'tailwind-merge';

import ReactQueryProvider from '@/components/providers/ReactQueryProvider';

import '../globals.css';
import '@repo/ui/styles.css';

const primary = Inter({
	subsets: ['latin'],
	variable: '--font-primary',
	weight: ['100', '200', '300', '400', '500', '600', '700', '800', '900'],
});

interface IRootLayoutProps {
	children?: ReactNode;
}

export const metadata: Metadata = {
	title: {
		default: 'Movies',
		template: `%s | Movies`,
	},
};

export const viewport: Viewport = {
	initialScale: 1,
	width: 'device-width',
};

const RootLayout = ({ children }: Readonly<IRootLayoutProps>) => {
	return (
		<html className={primary.variable} lang="en">
			<body
				className={twJoin(
					'flex',
					'flex-col',
					'font-primary',
					'min-h-screen',
					'relative',
					'selection:bg-primary',
					'selection:text-white',
				)}
			>
				<ReactQueryProvider>{children}</ReactQueryProvider>
			</body>
		</html>
	);
};

export default RootLayout;
