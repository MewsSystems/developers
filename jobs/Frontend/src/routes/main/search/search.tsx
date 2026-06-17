import { SyntheticEvent } from 'react'
import { useDebouncedCallback } from 'use-debounce'
import css from './search.module.css'
import logo from '@static/mewies.png'

interface SearchProps {
  onChange: (query: string) => void
}

const Search = ({ onChange }: SearchProps) => {
  const handleChange = useDebouncedCallback(onChange, 500)
  return (
    <div className={css.wrapper}>
      <img src={logo} className={css.logo} height="16" />
      <input
        className={css.input}
        onChange={(ev: SyntheticEvent<HTMLInputElement>) => {
          handleChange(ev.currentTarget.value)
        }}
        autoFocus
        placeholder="Search for movie..."
      />
    </div>
  )
}

export default Search
