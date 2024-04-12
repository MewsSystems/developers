import { useEffect, useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import { Button, CssBaseline, ThemeProvider } from '@mui/material'
import './App.css'
import { muiTheme } from './themes'
import { CssPriorityProvider } from './providers'
import { getMovieSearchList } from './api/movies'

function App() {
    const [count, setCount] = useState(0)

    const getBatmanMovies = async () => {
        const response = await getMovieSearchList('batman begins')
        console.log(response)
    }

    useEffect(() => {
        getBatmanMovies()
    }, [])

    return (
        <>
            <CssPriorityProvider>
                <CssBaseline />
                <ThemeProvider theme={muiTheme}>
                    <div>
                        <a
                            href='https://vitejs.dev'
                            target='_blank'
                        >
                            <img
                                src={viteLogo}
                                className='logo'
                                alt='Vite logo'
                            />
                        </a>
                        <a
                            href='https://react.dev'
                            target='_blank'
                        >
                            <img
                                src={reactLogo}
                                className='logo react'
                                alt='React logo'
                            />
                        </a>
                    </div>
                    <h1>Vite + React</h1>
                    <div className='card'>
                        <Button
                            variant='contained'
                            className='px-12'
                            onClick={() => setCount((count) => count + 1)}
                        >
                            count is {count}
                        </Button>
                        <p>
                            Edit <code>src/App.tsx</code> and save to test HMR
                        </p>
                    </div>
                    <p className='read-the-docs'>
                        Click on the Vite and React logos to learn more
                    </p>
                </ThemeProvider>
            </CssPriorityProvider>
        </>
    )
}

export default App
