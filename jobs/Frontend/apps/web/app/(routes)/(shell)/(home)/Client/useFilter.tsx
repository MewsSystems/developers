/*
 |-----------------------------------------------------------------------------
 | app/(routes)/(shell)/(home)/Client/useFilter.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { useState } from 'react';

import type { IPerson } from '@/lib/types';

const useFilter = ({ people }: any) => {
	const [filterByApi, setFilterByApi] = useState<boolean>(true);
	const [filterByFn, setFilterByFn] = useState<boolean>(false);
	const [query, setQuery] = useState<string>('');

	const handleFilterByApi = () => {
		setFilterByApi(true);
		setFilterByFn(false);
	};

	const handleFilterByFn = () => {
		setFilterByApi(false);
		setFilterByFn(true);
	};

	const filteredPeople = people.filter((person: IPerson) => {
		return person.name.toLowerCase().includes(query.toLowerCase());
	});

	return {
		filterByApi,
		filterByFn,
		filteredPeople,
		handleFilterByApi,
		handleFilterByFn,
		setQuery,
	};
};

export { useFilter };
