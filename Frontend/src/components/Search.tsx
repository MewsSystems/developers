import { Input } from 'components/Input'
import React, { useState, useCallback, useRef } from 'react'
import { useTranslation } from 'react-i18next'
import { useTrackEvent } from 'hooks/useTrackEvent'
import { EVENT_CATEGORY, EVENT_ACTION } from 'constants/tracking'

export interface SearchProps {
  value?: string
  onChange?: (value: string) => void
  onClear?: () => void
  onPressEnter?: (value: string) => void
  className?: string
}

export const Search: React.FC<SearchProps> = ({
  value = '',
  onChange,
  onClear,
  onPressEnter,
  className,
}) => {
  const { t } = useTranslation('search')
  const { setTrackEvent } = useTrackEvent()
  const [searchValue, setSearchValue] = useState(value)
  const inputRef = useRef<HTMLInputElement>()

  const handleChange = useCallback(
    ({ target: { value } }: React.ChangeEvent<HTMLInputElement>) => {
      setSearchValue(value)
      onChange && onChange(value)
    },
    [onChange]
  )

  const handleKeyDown = useCallback(
    ({ key }: React.KeyboardEvent<HTMLInputElement>) => {
      if (key === 'Enter' && onPressEnter) {
        onPressEnter(searchValue)

        setTrackEvent({
          eventCategory: EVENT_CATEGORY.SEARCH,
          eventAction: EVENT_ACTION.SEARCH.SUBMIT,
          eventLabel: searchValue,
        })
      }
    },
    [onPressEnter, searchValue, setTrackEvent]
  )

  const handleClear = useCallback(() => {
    setSearchValue('')

    setTrackEvent({
      eventCategory: EVENT_CATEGORY.SEARCH,
      eventAction: EVENT_ACTION.SEARCH.CLEAR,
    })

    onClear && onClear()

    inputRef.current && inputRef.current.focus()
  }, [onClear, setTrackEvent])

  return (
    <Input
      ref={inputRef}
      fullWidth
      allowClear
      inputSize="large"
      type="text"
      placeholder={t('placeholder')}
      value={searchValue}
      onClear={handleClear}
      onChange={handleChange}
      onKeyDown={handleKeyDown}
      className={className}
    />
  )
}
