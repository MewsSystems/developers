/*
 |-----------------------------------------------------------------------------
 | app/(routes)/(shell)/(home)/Client/useData.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { useEffect, useState } from 'react';

import type { ColDef } from '@ui/components/organisms/DataGrid';
import { getAllPeople } from '@/services/person/queries';
import { useQuery } from '@tanstack/react-query';

import type { IRowData } from './types';
import ActionsRenderer from '@/components/renderers/ActionsRenderer';

const useData = () => {
	const [columns] = useState<ColDef<IRowData>[]>([
		{
			field: 'name',
			headerName: 'Name',
		},
		{
			field: 'uid',
			headerName: '',
			sortable: false,
			cellRenderer: ActionsRenderer,
		},
	]);

	const [message, setMessage] = useState<string>('');
	// const [movies, setMovies] = useState<IRowData[]>([]);
	const [people, setPeople] = useState<IRowData[]>([]);

	const { data, isError, isLoading } = useQuery({
		queryKey: ['people'],
		queryFn: () => getAllPeople(),
	});

	useEffect(() => {
		if (data) {
			const { results } = data;

			setPeople(results);
		}

		if (isError) {
			setMessage('Error retrieving data');
		}
	}, [data, isError]);

	return { columns, isError, isLoading, message, people };
};

export { useData };
