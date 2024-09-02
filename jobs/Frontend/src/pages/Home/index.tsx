import { useNavigate } from "react-router-dom"

export const HomePage = () => {
  const navigate = useNavigate()

  return (
    <div className="flex h-full flex-col items-center justify-center">
      <h1 className="text-3xl font-bold">Welcome!</h1>
      <button
        onClick={() => navigate("/movies")}
        className="mt-4 text-xl text-sky-500 hover:text-sky-600 hover:underline"
      >
        Search for Movies
      </button>
    </div>
  )
}

export default HomePage
