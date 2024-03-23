import { Movie } from "@/types/Movie";
import Link from "next/link"

export default function MovieTile({ movie }: { movie: Movie }) {
    const overview = `${movie.overview?.split(/\s+/).slice(0, 30).join(' ') + '...'}`

    return (
        <div className="p-8 max-w-lg border border-indigo-300 rounded-2xl hover:shadow-xl hover:shadow-indigo-50 flex flex-col items-center bg-white text-black">
            <img
                src={`https://image.tmdb.org/t/p/original/${movie.poster_path}`}
                className="shadow rounded-lg overflow-hidden border"
                alt={movie.title}
            />
            <div className="mt-8">
                <h4 className="font-bold">{movie.title}</h4>
                <p className="mt-2">{overview}</p>
                <Link href={`/${movie.id}`}>
                    <span className="mt-2 text-indigo-600 hover:text-indigo-800">See more</span>
                </Link>
            </div>
        </div>
    );
};
