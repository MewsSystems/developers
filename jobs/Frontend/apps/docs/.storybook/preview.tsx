/*
 |-----------------------------------------------------------------------------
 | .storybook/preview.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ElementType } from 'react';

import type { Preview, ReactRenderer } from '@storybook/react';
import { withThemeByClassName } from '@storybook/addon-themes';

import '../stories/globals.css';
import '@repo/ui/styles.css';

export const argTypes = {
	children: {
		table: {
			disable: true,
		},
	},
	classNames: {
		table: {
			disable: true,
		},
	},
	Icon: {
		table: {
			disable: true,
		},
	},
	onBlur: {
		table: {
			disable: true,
		},
	},
	onChange: {
		table: {
			disable: true,
		},
	},
	onClick: {
		table: {
			disable: true,
		},
	},
};

export const decorators = [
	(Story: ElementType) => <Story />,
	withThemeByClassName<ReactRenderer>({
		defaultTheme: 'light',
		themes: {
			dark: 'dark',
			light: 'light',
		},
	}),
];

const preview: Preview = {
	parameters: {
		backgrounds: {
			default: 'dark',
			values: [
				{
					name: 'dark',
					value: '#1b1c1d',
				},
				{
					name: 'light',
					value: '#ffffff',
				},
			],
		},

		controls: {
			matchers: {
				color: /(background|color)$/i,
				date: /Date$/,
			},
		},

		options: {
			storySort: {
				order: [
					'Introduction',
					'Design tokens',
					['Introduction'],
					'Components',
					[
						'Introduction',
						'Atoms',
						['Introduction'],
						'Molecules',
						['Introduction'],
						'Organisms',
						['Introduction'],
					],
				],
			},
		},

		status: {
			statuses: {
				backlog: {
					background: '#1ea7fd',
					color: '#000000',
					description: 'Component required',
				},
				draft: {
					background: '#e81c4f',
					color: '#000000',
					description: 'Work in progress',
				},
				ga: {
					background: '#19cf86',
					color: '#000000',
					description: 'General access',
				},
				published: {
					background: '#19cf86',
					color: '#000000',
					description: 'General access',
				},
				rc: {
					background: '#ed734a',
					color: '#000000',
					description: 'Release candidate',
				},
				wip: {
					background: '#e81c4f',
					color: '#000000',
					description: 'Work in progress',
				},
			},
		},

		viewport: {
			viewports: {
				'2xl': {
					name: 'screen-2xl',
					styles: {
						height: '800px',
						width: '1280px',
					},
				},
				xl: {
					name: 'screen-xl',
					styles: {
						height: '768px',
						width: '1024px',
					},
				},
				lg: {
					name: 'screen-lg',
					styles: {
						height: '1024px',
						width: '768px',
					},
				},
				md: {
					name: 'screen-md',
					styles: {
						height: '926px',
						width: '428px',
					},
				},
				sm: {
					name: 'screen-sm',
					styles: {
						height: '844px',
						width: '390px',
					},
				},
				xs: {
					name: 'screen-xs',
					styles: {
						height: '667px',
						width: '375px',
					},
				},
			},
		},
	},
};

export default preview;
