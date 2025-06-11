/*
 |-----------------------------------------------------------------------------
 | app/robots.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { MetadataRoute } from 'next';

const robots = (): MetadataRoute.Robots => {
	return {
		rules: [
			{
				userAgent: '*',
				disallow: '/',
			},
		],
	};
};

export default robots;
