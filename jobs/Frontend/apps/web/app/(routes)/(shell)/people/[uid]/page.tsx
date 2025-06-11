/*
 |-----------------------------------------------------------------------------
 | app/(routes)/(shell)/people/[uid]/page.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import Container from '@repo/ui/components/layouts/Container';
import Link from 'next/link';
import type { Metadata } from 'next';
import Title from '@repo/ui/components/atoms/Title';

import { getPerson } from '@/services/person/queries';

import Client from './Client';

export const generateMetadata = async ({
	params,
}: {
	params: Promise<{ uid: string }>;
}): Promise<Metadata> => {
	const uid: string = (await params).uid;

	const { result } = await getPerson(uid);

	return {
		title: `${result.properties.name} - People`,
	};
};

const Page = async ({
	params,
}: {
	params: Promise<{ uid: string }>;
}): Promise<ReactElement> => {
	const uid: string = (await params).uid;

	const { result } = await getPerson(uid);

	return (
		<>
			<header>
				<Container classNames="space-y-md">
					<Link
						className="link-no-underline text-primary inline-block"
						href="/"
					>
						&#10094; Back
					</Link>

					<Title level={1} size="xl" text={result.properties.name} />
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
