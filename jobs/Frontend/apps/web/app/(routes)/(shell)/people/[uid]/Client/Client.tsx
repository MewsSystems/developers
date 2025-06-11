/*
 |-----------------------------------------------------------------------------
 | app/(routes)/(shell)/people/[uid]/Client/Client.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

'use client';

import type { ReactElement } from 'react';

import Field from '@repo/ui/components/molecules/Field';
import Form from '@repo/ui/components/organisms/Form';
import Input from '@repo/ui/components/atoms/Input';
import dynamic from 'next/dynamic';
import { twJoin } from 'tailwind-merge';

import { useData } from './useData';

const Loader = dynamic(() => import('@/components/Loader'));
const Notifications = dynamic(() => import('@/components/Notifications'));

const Client = (): ReactElement => {
	const { isError, isLoading, message, person } = useData();

	if (isLoading) {
		return <Loader />;
	}

	return (
		<Form
			classNames={twJoin('bg-white', 'p-md', 'rounded-xl', 'space-y-sm')}
		>
			{isError && <Notifications isError={isError} message={message} />}

			<Field id="name" label="Name">
				<Input
					defaultValue={person.name}
					id="name"
					isReadonly
					name="name"
					type="text"
				/>
			</Field>

			<Field id="gender" label="Gender">
				<Input
					defaultValue={person.gender}
					id="gender"
					isReadonly
					name="gender"
					type="text"
				/>
			</Field>

			<Field id="skin-colour" label="Skin Colour">
				<Input
					defaultValue={person.skin_color}
					id="skin-colour"
					isReadonly
					name="skin colour"
					type="text"
				/>
			</Field>

			<Field id="hair-colour" label="Hair Colour">
				<Input
					defaultValue={person.hair_color}
					id="hair-color"
					isReadonly
					name="hair colour"
					type="text"
				/>
			</Field>

			<Field id="height" label="Height">
				<Input
					defaultValue={person.height}
					id="height"
					isReadonly
					name="height"
					type="number"
				/>
			</Field>

			<Field id="eye-colour" label="Eye colour">
				<Input
					defaultValue={person.eye_color}
					id="eye-colour"
					isReadonly
					name="eye colour"
					type="text"
				/>
			</Field>

			<Field id="mass" label="Mass">
				<Input
					defaultValue={person.mass}
					id="mass"
					isReadonly
					name="mass"
					type="number"
				/>
			</Field>

			<Field id="birth-year" label="Birth year">
				<Input
					defaultValue={person.birth_year}
					id="birth-year"
					isReadonly
					name="birth year"
					type="text"
				/>
			</Field>
		</Form>
	);
};

export default Client;
