/*
 |-----------------------------------------------------------------------------
 | src/components/organisms/DataGrid/types.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { AgGridReactProps } from 'ag-grid-react';

interface IDataGridProps {
	classNames?: string;
	columns: AgGridReactProps['columnDefs'];
	data: AgGridReactProps['rowData'];
}

type TUseDataGridParams = {
	length: number;
};

export type { IDataGridProps, TUseDataGridParams };
