'use client'

import { ChangeEvent, ChangeEventHandler, useEffect, useState } from 'react'
import {
  StyledInput,
  Wrapper,
  StyledMagnifyingGlassIcon,
} from './Searchbar.styles'
import { useDebouncedCallback } from 'use-debounce'
import { usePathname, useRouter, useSearchParams } from 'next/navigation'
import { useIsMobile } from '@/hooks'

type Props = {
  placeholder: string
  onChange: ChangeEventHandler<HTMLInputElement>
  onLoad: (_: string) => void
}

/**
 *
 * Controlled Searchbar component
 */
export const Searchbar = ({ placeholder, onChange, onLoad }: Props) => {
  const { isMobile } = useIsMobile()
  const searchParams = useSearchParams()
  const { push } = useRouter()
  const pathname = usePathname()

  const [inputValue, setInputValue] = useState('')

  // load query from url
  useEffect(() => {
    const query = searchParams.get('query') ?? ''
    setInputValue(query)

    onLoad(query)
  }, [onLoad, searchParams])

  const handleSearch = useDebouncedCallback(
    (event: ChangeEvent<HTMLInputElement>) => {
      const query = event.target.value
      const params = new URLSearchParams(searchParams)

      // update query in url
      if (query) {
        params.set('query', query)
      } else {
        params.delete('query')
      }
      push(`${pathname}?${params.toString()}`)

      onChange(event)
    },
    300,
  )

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const query = event.target.value
    setInputValue(query)

    handleSearch(event)
  }

  return (
    <Wrapper>
      <StyledMagnifyingGlassIcon $isMobile={isMobile} />
      <StyledInput
        name="searchbar"
        autoFocus
        $isMobile={isMobile}
        placeholder={placeholder}
        value={inputValue}
        onChange={handleChange}
      />
    </Wrapper>
  )
}
