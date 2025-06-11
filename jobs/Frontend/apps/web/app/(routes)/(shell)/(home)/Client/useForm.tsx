/*
 |-----------------------------------------------------------------------------
 | app/(routes)/(shell)/(home)/Client/useForm.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 |
 | Some description...
 */

import { useEffect, useState } from 'react';

import { type ObjectSchema, object, string } from 'yup';
import {
	type SubmitHandler,
	useForm as useReactHookForm,
} from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';

import { type IFormData } from './types';
import { findPerson } from '@/services/person/queries';
import { useQuery } from '@tanstack/react-query';

const useForm = () => {
	const [isError, setIsError] = useState<boolean>(false);
	const [isInvalid, setIsInvalid] = useState<boolean>(false);
	const [isSuccess, setIsSuccess] = useState<boolean>(false);
	const [message, setMessage] = useState<string>('');
	const [people, setPeople] = useState<{}[]>([]);
	const [query, setQuery] = useState<string>('');

	const { data, isLoading } = useQuery({
		queryKey: ['find', query],
		queryFn: () => findPerson(query),
		enabled: !!query,
	});

	const schema: ObjectSchema<IFormData> = object({
		search: string(),
	}).required();

	const {
		control: control,
		formState: { errors },
		handleSubmit,
	} = useReactHookForm<IFormData>({
		mode: 'onBlur',
		resolver: yupResolver(schema),
	});

	const onChange = () => {
		setIsInvalid(false);
		setIsSuccess(false);
		setMessage('');
	};

	const onError = () => {
		setIsInvalid(true);
	};

	const onSubmit: SubmitHandler<IFormData> = async (data, e) => {
		e?.preventDefault();

		if (data.search !== undefined) {
			setQuery(data.search);
		}
	};

	useEffect(() => {
		if (data) {
			const { result } = data;

			const resultFormatted = result.map((person: any) => ({
				name: person.properties.name,
				uid: person.uid,
			}));

			setPeople(resultFormatted);
		}
	}, [data]);

	return {
		control,
		errors,
		handleSubmit,
		isError,
		isInvalid,
		isLoading,
		isSuccess,
		message,
		onChange,
		onError,
		onSubmit,
		people,
		query,
	};
};

export { useForm };
