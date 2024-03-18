import React from 'react'
import { createRoot } from 'react-dom/client'
import { MoviesList } from './components'

const container = document.getElementById('root') as HTMLElement
const root = createRoot(container)

root.render(
  <React.StrictMode>
    <MoviesList />
  </React.StrictMode>
)
