/*
 |-----------------------------------------------------------------------------
 | lib/types/person.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

interface IPerson {
	birth_year?: string;
	eye_color?: string;
	gender?: string;
	hair_color?: string;
	height?: string;
	mass?: string;
	name: string;
	skin_color?: string;
	uid: string;
}

export type { IPerson };
