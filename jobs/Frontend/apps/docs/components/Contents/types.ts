/*
 |-----------------------------------------------------------------------------
 | components/layouts/Contents/types.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

type TItem = {
	label: string;
	path: string;
};

interface IContentsProps {
	items: TItem[];
}

export type { IContentsProps, TItem };
