import { Movie } from "@/types/Movie";

export default function MovieDetail({ movie }: { movie: Movie }) {
    const { title, overview, poster_path } = movie;

    return (
        <div className="w-full md:w-2/3 mx-auto p-8 max-w-3xl border border-indigo-300 rounded-2xl hover:shadow-xl hover:shadow-indigo-50 flex flex-col items-center bg-white text-black">
            <img
                src={`https://image.tmdb.org/t/p/original/${poster_path}`}
                className="shadow rounded-lg overflow-hidden border"
                alt={title}
            />
            <div className="mt-8">
                <h4 className="font-bold">{title}</h4>
                <p className="mt-2">{overview}</p>
            </div>
        </div>
    );
}