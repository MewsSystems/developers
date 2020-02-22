import { Input } from 'components/Input'
import React, { useState, useCallback, useRef } from 'react'
import { useTranslation } from 'react-i18next'

export interface SearchProps {
  value?: string
  onChange?: (value: string) => void
  onPressEnter?: (value: string) => void
  className?: string
}

export const Search: React.FC<SearchProps> = ({
  value = '',
  onChange,
  onPressEnter,
  className,
}) => {
  const { t } = useTranslation('search')
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
      if (key === 'Enter' && onPressEnter) onPressEnter(searchValue)
    },
    [onPressEnter, searchValue]
  )

  const handleClear = useCallback(() => {
    setSearchValue('')
    onChange && onChange('')

    inputRef.current && inputRef.current.focus()
  }, [onChange])

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
