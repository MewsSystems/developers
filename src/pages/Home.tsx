import { MovieSearch } from '@/features/MovieSearch/MovieSearch'

export const Home = () => {
  return (
    <main className="flex items-center justify-center min-h-screen px-4">
      <section className="max-w-4xl w-full bg-white shadow-lg rounded-2xl p-6 md:p-10">
        <MovieSearch />
      </section>
    </main>
  )
}
