/*
 |-----------------------------------------------------------------------------
 | app/(routes)/(shell)/(home)/Client/Client.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

'use client';

import type { ReactElement } from 'react';

import { Controller } from 'react-hook-form';
import DataGrid from '@repo/ui/components/organisms/DataGrid';
import Field from '@repo/ui/components/molecules/Field';
import Form from '@repo/ui/components/organisms/Form';
import Input from '@repo/ui/components/atoms/Input';
import LoaderComponent from '@repo/ui/components/atoms/Loader';
import dynamic from 'next/dynamic';

import { useData } from './useData';
import { useFilter } from './useFilter';
import { useForm } from './useForm';
import { twJoin } from 'tailwind-merge';

const Loader = dynamic(() => import('@/components/Loader'));
const Notifications = dynamic(() => import('@/components/Notifications'));

const Client = (): ReactElement => {
	const {
		columns,
		isError: dataIsError,
		isLoading: dataIsLoading,
		message: dataMessage,
		people,
	} = useData();

	const {
		control,
		errors,
		handleSubmit,
		isError: formIsError,
		isInvalid,
		isLoading: formIsLoading,
		isSuccess,
		message: formMessage,
		onChange,
		onError,
		onSubmit,
		people: peopleFiltered,
		query,
	} = useForm();

	const {
		filterByApi,
		filterByFn,
		filteredPeople,
		handleFilterByApi,
		handleFilterByFn,
		setQuery,
	} = useFilter({ people });

	const isError: boolean = dataIsError || formIsError;
	const message: string = dataMessage || formMessage;

	if (dataIsError) {
		return <Notifications isError={isError} message={message} />;
	}

	if (dataIsLoading) {
		return <Loader />;
	}

	return (
		<>
			<div className="mb-md">
				<button
					className="text-primary link-no-underline cursor-pointer"
					onClick={handleFilterByApi}
				>
					Filter by API
				</button>
				<span className="text-secondary px-md inline-block">|</span>
				<button
					className="text-primary link-no-underline cursor-pointer"
					onClick={handleFilterByFn}
				>
					Filter by Function
				</button>
			</div>

			{filterByApi && (
				<Form
					classNames={twJoin(
						'bg-white',
						'p-md',
						'rounded-xl',
						'space-y-sm',
					)}
					onChange={onChange}
					onSubmit={handleSubmit(onSubmit, onError)}
				>
					{(formIsError || isInvalid || isSuccess) && (
						<Notifications
							errors={errors}
							isError={isError}
							isInvalid={isInvalid}
							isSuccess={isSuccess}
							message={message}
						/>
					)}

					<Controller
						control={control}
						name="search"
						render={({ field: { name, onBlur, onChange } }) => (
							<Field
								error={errors[name]?.message}
								hint="Press enter after entering your search term"
								id={name}
								label="Search"
							>
								<Input
									id={name}
									name={name}
									onBlur={onBlur}
									onChange={onChange}
									placeholder="Enter search term"
									type="text"
								/>
							</Field>
						)}
					/>
				</Form>
			)}

			{filterByFn && (
				<div
					className={twJoin(
						'bg-white',
						'p-md',
						'rounded-xl',
						'space-y-sm',
					)}
				>
					<Field id="search" label="Search">
						<Input
							id="search"
							name="search"
							onChange={(e) => setQuery(e.target.value)}
							placeholder="Enter search term"
							type="text"
						/>
					</Field>
				</div>
			)}

			{formIsLoading ? (
				<div className="mt-md flex justify-center">
					<LoaderComponent />
				</div>
			) : (
				<DataGrid
					classNames="mt-md"
					columns={columns}
					data={query ? peopleFiltered : filteredPeople || people}
				/>
			)}
		</>
	);
};

export default Client;
