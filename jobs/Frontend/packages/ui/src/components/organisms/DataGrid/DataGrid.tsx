/*
 |-----------------------------------------------------------------------------
 | src/components/organisms/DataGrid/DataGrid.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

'use client';

import type { ReactElement } from 'react';

import { AllCommunityModule, ModuleRegistry } from 'ag-grid-community';
import { AgGridReact } from 'ag-grid-react';
import { twMerge } from 'tailwind-merge';

import type { IDataGridProps } from './types';
import { strapless } from './theme';
import { useDataGrid } from './hooks';

ModuleRegistry.registerModules([AllCommunityModule]);

const DataGrid = ({
	classNames,
	columns,
	data,
}: IDataGridProps): ReactElement => {
	const { defaultColDef, height, onGridReady, onPaginationChanged } =
		useDataGrid({ length: data?.length || 0 });

	return (
		<div
			className={twMerge('flex', 'flex-col', 'gap-y-4', classNames)}
			data-testid="dataGrid"
		>
			<div className="w-full" style={{ height: height }}>
				<AgGridReact
					columnDefs={columns}
					defaultColDef={defaultColDef}
					onGridReady={onGridReady}
					onPaginationChanged={onPaginationChanged}
					pagination
					paginationPageSize={5}
					paginationPageSizeSelector={[5, 10, 25, 50, 100]}
					rowData={data}
					theme={strapless}
				/>
			</div>
		</div>
	);
};

export default DataGrid;
