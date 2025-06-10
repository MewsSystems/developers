/*
 |-----------------------------------------------------------------------------
 | src/components/organisms/DataGrid/hooks.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { type SetStateAction, useCallback, useState } from 'react';

import type { ColDef, GridApi } from 'ag-grid-community';

import type { TUseDataGridParams } from './types';

const useDataGrid = ({ length }: TUseDataGridParams) => {
	const [gridApi, setGridApi] = useState<GridApi | null>(null);
	const [pageSize, setPageSize] = useState<number>(0);

	console.log('Debug', length);

	const onGridReady = useCallback(
		// eslint-disable-next-line @typescript-eslint/no-explicit-any
		(params: { api: SetStateAction<GridApi<any> | null> }) => {
			setGridApi(params.api);
		},
		[],
	);

	const onPaginationChanged = useCallback(() => {
		if (gridApi) {
			const pageSize = gridApi.paginationGetPageSize();
			console.log('Debug', pageSize);
			setPageSize(pageSize);
		}
	}, [gridApi]);

	const defaultColDef: ColDef = {
		flex: 1,
	};

	const height =
		length === 0 ? 'calc(93px + 42px' : `calc(93px + (42px * ${pageSize}))`;

	return {
		defaultColDef,
		height,
		onGridReady,
		onPaginationChanged,
	};
};

export { useDataGrid };
