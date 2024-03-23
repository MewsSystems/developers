import Search from "./Search";

export default function Header() {
    return (
        <div className="bg-gray-900 py-20">
            <div className="mx-auto px-6">
                <h1 className="text-4xl text-white font-bold mb-6">
                    Movie search
                    <br className="hidden md:block" />
                    <span className="text-indigo-500">by Sarah Ozatici</span>
                    <div className="mt-4">
                        <Search placeholder={"search on a movie"} />
                    </div>
                </h1>
            </div>
        </div>
    )
}
