/*
 |-----------------------------------------------------------------------------
 | app/(routes)/(shell)/people/[uid]/Client/useData.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { useEffect, useState } from 'react';

import { useParams } from 'next/navigation';
import { useQuery } from '@tanstack/react-query';

import { getPerson } from '@/services/person/queries';

import type { IPerson } from '@/lib/types';

const useData = () => {
	const params = useParams<{ uid: string }>();

	const [message, setMessage] = useState<string>('');
	const [person, setPerson] = useState<IPerson>({
		name: '',
		uid: '',
	});

	const { data, isError, isLoading } = useQuery({
		queryKey: ['person', params.uid],
		queryFn: () => getPerson(params.uid),
	});

	useEffect(() => {
		if (data) {
			const { result } = data;

			setPerson(result.properties);
		}

		if (isError) {
			setMessage('Error retrieving data');
		}
	}, [data, isError]);

	return { isError, isLoading, message, person };
};

export { useData };
