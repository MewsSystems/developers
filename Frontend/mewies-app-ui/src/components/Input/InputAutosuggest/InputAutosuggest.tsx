import React, { KeyboardEvent, useEffect, useState } from 'react'
import { InputProps, InputText } from '../InputText/InputText'
import { KeyCodes, Suggestion } from '../../../utils/hooks/useAutosuggest'
import {
    InputAutosuggestWrapper,
    StyledSuggestion,
    SuggestionList,
} from '../Input.styles'

interface InputAutosuggestProps extends InputProps {
    suggestions: Suggestion[]
    selected?: Suggestion
    onClear?(): void
    onSelect(suggestion: Suggestion): void
}

interface SuggestionProps {
    tabIndex: number
    label: string
    selected?: boolean
    onSelect(): void
}

const SuggestionElement: React.FC<SuggestionProps> = props => (
    <StyledSuggestion
        tabIndex={props.tabIndex}
        onClick={props.onSelect}
        aria-label={props.label}
        isSelected={props.selected}
    >
        <p>{props.label}</p>
    </StyledSuggestion>
)

export const InputAutosuggest: React.FC<InputAutosuggestProps> = props => {
    const [activeSelection, setActive] = useState(0)

    const handleKeyDown = (e: KeyboardEvent<HTMLLIElement>) => {
        if (e.keyCode === KeyCodes.Enter) {
            e.preventDefault()
            try {
                handleSelectSuggestion(props.suggestions[activeSelection])()
            } catch (err) {
                console.error(err)
            }
            return
        }
        if (e.keyCode === KeyCodes.ArrowUp) {
            activeSelection >= 0 && setActive(activeSelection - 1)
            return
        }
        if (e.keyCode === KeyCodes.ArrowDown) {
            setActive(activeSelection + 1)
            return
        }
    }

    const handleSelectSuggestion = (suggestion: Suggestion) => {
        return () => props.onSelect(suggestion)
    }

    const renderSuggestions = () =>
        props.suggestions.map((suggestion: Suggestion, idx: number) => {
            return (
                <SuggestionElement
                    label={suggestion.label}
                    tabIndex={idx}
                    key={idx}
                    selected={idx === activeSelection}
                    onSelect={handleSelectSuggestion(suggestion)}
                />
            )
        })

    useEffect(() => {
        setActive(0)
    }, [props.suggestions])

    return (
        <InputAutosuggestWrapper>
            <InputText
                onKeyDown={handleKeyDown}
                value={props.selected?.value}
                {...props}
            />
            {props.suggestions.length > 0 && (
                <SuggestionList>{renderSuggestions()}</SuggestionList>
            )}
        </InputAutosuggestWrapper>
    )
}
