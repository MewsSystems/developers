/*
 |-----------------------------------------------------------------------------
 | app/(routes)/(home)/page.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import Client from './Client';
import Container from '@ui/components/layouts/Container';
import type { Metadata } from 'next';
import Title from '@ui/components/atoms/Title';

export const metadata: Metadata = {
	title: 'Home',
};

const Page = async (): Promise<ReactElement> => {
	return (
		<>
			<header>
				<Container>
					<Title level={1} size="xl">
						Now playing
					</Title>
				</Container>
			</header>

			<section>
				<Container>
					<Client />
				</Container>
			</section>
		</>
	);
};

export default Page;
