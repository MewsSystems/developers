import React, { useEffect, useState } from 'react'
import { useDebounce } from './useDebounce'

export interface Suggestion {
    label: string
    value: string
}

export enum KeyCodes {
    Enter = 13,
    ArrowUp = 38,
    ArrowDown = 40,
}

interface AutosuggestSettings<K> {
    // GET request with query as a param - returning any data
    onFetch(query: string): Promise<K>
    onSelect?(suggestion: Suggestion): void
    onClear?(): void
    // Map function which maps any payload from fetch to Suggestion[]
    onPipeData(data: K): Suggestion[]
    initialSuggestions?: Suggestion[]
    initialSelection?: Suggestion
}

export const useAutosuggest = <K>(options: AutosuggestSettings<K>) => {
    const [suggestions, setSuggestions] = useState<Suggestion[]>([])
    const [selected, setSelected] = useState<Suggestion | null>(null)
    const [query, setQuery] = useState<string>('')

    // Im using debounce to not shoot API every keystroke
    const debouncedQuery = useDebounce(query, 300)

    const fetchSuggestions = async (): Promise<K> => {
        return await options.onFetch(query)
    }

    const prepareSuggestions = (data: Suggestion[], filterBy: string) => {
        const filteredData = data.filter(suggestion =>
            suggestion.value.toUpperCase().includes(filterBy.toUpperCase())
        )
        query && setSuggestions(filteredData)
    }

    const handleSearchQuery = (e: React.FormEvent<HTMLInputElement>) => {
        setQuery(e.currentTarget.value)
    }

    const handleSelect = (suggestion: Suggestion) => {
        setSelected(suggestion)
        options.onSelect && options.onSelect(suggestion)
        clearSuggestions()
    }

    const clearSuggestions = () => {
        setSuggestions([])
        setQuery('')
    }

    const resetSelected = () => {
        setSelected(null)
    }

    useEffect(() => {
        const { initialSuggestions } = options
        initialSuggestions && prepareSuggestions(initialSuggestions, query)
    }, [query])

    useEffect(() => {
        const { onFetch, onPipeData } = options
        if (debouncedQuery && onFetch) {
            fetchSuggestions()
                .then(data => {
                    const mappedSuggestions = onPipeData(data)
                    setSuggestions(mappedSuggestions)
                })
                .catch(err => console.error(err))
        } else {
            setSuggestions([])
        }
    }, [debouncedQuery])

    return {
        suggestions,
        selected,
        handleSearchQuery,
        clearSuggestions,
        resetSelected,
        handleSelect,
    }
}
