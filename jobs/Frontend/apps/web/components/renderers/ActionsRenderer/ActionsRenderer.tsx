/*
 |-----------------------------------------------------------------------------
 | components/renderers/ActionsRenderer/ActionsRenderer.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

'use client';

import type { ReactElement } from 'react';

import type { ICellRendererParams } from '@repo/ui/components/organisms/DataGrid';
import Link from 'next/link';

const ActionsRenderer = (params: ICellRendererParams): ReactElement => {
	return (
		<div>
			<Link
				className="link-no-underline text-primary"
				href={`/people/${params.value}`}
			>
				View
			</Link>
		</div>
	);
};

export default ActionsRenderer;
