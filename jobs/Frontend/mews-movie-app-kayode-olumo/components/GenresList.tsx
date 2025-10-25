interface Genre {
  id: number
  name: string
}

interface GenresListProps {
  genres: Genre[]
}

export function GenresList({ genres }: GenresListProps) {
  if (!genres || genres.length === 0) return null

  return (
    <div>
      <h3 className="text-lg sm:text-xl font-bold mb-3">Genres</h3>
      <div className="flex flex-wrap gap-2">
        {genres.map((genre) => (
          <span key={genre.id} className="px-4 py-2 bg-secondary border-2 rounded-full text-sm font-medium">
            {genre.name}
          </span>
        ))}
      </div>
    </div>
  )
}
