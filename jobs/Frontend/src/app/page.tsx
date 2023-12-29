import { Canvas } from '@/components'
import { Filterbar, Header, Scene, MovieList } from './components'

export default function SearchPage() {
  return (
    <main>
      <Canvas>
        <Scene />
      </Canvas>
      <div className="absolute w-full px-8 md:px-24 pt-10 md:pt-32">
        <Header />
        <Filterbar />
        <MovieList />
      </div>
    </main>
  )
}
