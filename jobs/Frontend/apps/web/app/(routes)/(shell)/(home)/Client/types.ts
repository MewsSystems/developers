/*
 |-----------------------------------------------------------------------------
 | app/(routes)/(shell)/(home)/Client/types.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

interface IFormData {
	search?: string;
}

interface IRowData {
	name: string;
	uid: string;
}

type TUseDataParams = {};

export type { IFormData, IRowData, TUseDataParams };
