import Link from 'next/link'

export default function MovieDetailNotFound() {
  return (
    <div className="text-white text-center">
      <h1 className="text-7xl">Movie probably does not exist. 🥺</h1>
      <br />
      <h2 className="text-6xl">
        Would you like to see{' '}
        <Link className="underline hover:bg-sky-700" href="/">
          all movies
        </Link>{' '}
        instead? 😊
      </h2>
    </div>
  )
}
