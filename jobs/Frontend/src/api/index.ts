import { API_KEY } from '@/constants/api'
import { Movie, MovieApiResponse } from './types'
import { baseUrl } from '@/constants/api'

function buildUrl(url: string): URL {
	

	const urlConstructor = new URL(`${baseUrl}${url}`)
	urlConstructor.searchParams.set('language', 'en-US')
	urlConstructor.searchParams.set('api_key', API_KEY)
	return urlConstructor
}

function get(url: string): Promise<Response> {
	const options = { method: 'GET', headers: { accept: 'application/json' } }

	const urlConstructor = buildUrl(url)
	return fetch(urlConstructor.toString(), options)
}

export async function trending(params: { page: number }): Promise<MovieApiResponse<Movie[]>> {
	const response = await get(`/trending/movie/day?page=${params.page}`)
	return response.json()
}

export async function search(params: { page: number, query: string }): Promise<MovieApiResponse<Movie[]>> {
	const response = await get(`/search/movie?page=${params.page}&query=${params.query}`)
	return response.json()
}

export async function movie(params: { id: number }): Promise<Movie> {
	const response = await get(`/movie/${params.id}`)
	return response.json()
}
