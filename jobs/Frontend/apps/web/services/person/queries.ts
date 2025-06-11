/*
 |-----------------------------------------------------------------------------
 | services/person/queries.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

/**
 * Find person
 *
 * @param query
 */
const findPerson = async (query: string) => {
	try {
		const response = await fetch(
			`${process.env.NEXT_PUBLIC_API_URL}/people?name=${query}`,
			{
				method: 'GET',
				headers: {
					'Content-Type': 'application/json',
				},
			},
		);

		if (response.status === 401) {
			console.error(
				`Error: ${response.statusText || 'Error authenticating user'}`,
			);
			return;
		}

		const data = await response.json();

		if (data) {
			return data;
		}
	} catch (error) {
		console.error(`Error: ${error}`);
	}
};

/**
 * Get person
 *
 * @param uid
 */
const getPerson = async (uid: string) => {
	try {
		const response = await fetch(
			`${process.env.NEXT_PUBLIC_API_URL}/people/${uid}`,
			{
				method: 'GET',
				headers: {
					'Content-Type': 'application/json',
				},
			},
		);

		if (response.status === 401) {
			console.error(
				`Error: ${response.statusText || 'Error authenticating user'}`,
			);
			return;
		}

		const data = await response.json();

		if (data) {
			return data;
		}
	} catch (error) {
		console.error(`Error: ${error}`);
	}
};

/**
 * Get all people
 */
const getAllPeople = async () => {
	try {
		const response = await fetch(
			`${process.env.NEXT_PUBLIC_API_URL}/people`,
			{
				method: 'GET',
				headers: {
					'Content-Type': 'application/json',
				},
			},
		);

		if (response.status === 401) {
			console.error(
				`Error: ${response.statusText || 'Error authenticating user'}`,
			);
			return;
		}

		const data = await response.json();

		if (data) {
			return data;
		}
	} catch (error) {
		console.error(`Error: ${error}`);
	}
};

export { findPerson, getPerson, getAllPeople };
